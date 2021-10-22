// Contains .NET 6-related Cake targets

var ext = IsRunningOnWindows() ? ".exe" : "";
var dotnetPath = $"./bin/dotnet/dotnet{ext}";

// Tasks for CI

Task("dotnet")
    .Description("Provisions .NET 6 into bin/dotnet based on eng/Versions.props")
    .Does(() =>
    {
        if (!localDotnet) 
            return;

        DotNetCoreBuild("./src/DotNet/DotNet.csproj", new DotNetCoreBuildSettings
        {
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .EnableBinaryLogger($"{logDirectory}/dotnet-{configuration}.binlog")
                .SetConfiguration(configuration),
        });
    });

Task("dotnet-local-workloads")
    .Does(() =>
    {
        if (!localDotnet) 
            return;
        
        DotNetCoreBuild("./src/DotNet/DotNet.csproj", new DotNetCoreBuildSettings
        {
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .EnableBinaryLogger($"{logDirectory}/dotnet-{configuration}.binlog")
                .SetConfiguration(configuration)
                .WithProperty("InstallWorkloadPacks", "false"),
        });

        DotNetCoreBuild("./src/DotNet/DotNet.csproj", new DotNetCoreBuildSettings
        {
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .EnableBinaryLogger($"{logDirectory}/dotnet-install-{configuration}.binlog")
                .SetConfiguration(configuration)
                .WithTarget("Install"),
            ToolPath = dotnetPath,
        });
    });

Task("dotnet-buildtasks")
    .IsDependentOn("dotnet")
    .Does(() =>
    {
        RunMSBuildWithDotNet("./Microsoft.Maui.BuildTasks-net6.slnf");
    });

Task("dotnet-build")
    .IsDependentOn("dotnet")
    .Description("Build the solutions")
    .Does(() =>
    {
        RunMSBuildWithDotNet("./Microsoft.Maui.BuildTasks-net6.slnf");
        if (IsRunningOnWindows())
            RunMSBuildWithDotNet("./Microsoft.Maui-net6.sln");
        else
            RunMSBuildWithDotNet("./Microsoft.Maui-mac.slnf");
    });

Task("dotnet-samples")
    .Does(() =>
    {
        RunMSBuildWithDotNet("./Microsoft.Maui.Samples-net6.slnf", new Dictionary<string, string> {
            ["UseWorkload"] = bool.TrueString,
        });
    });

Task("dotnet-templates")
    .Does(() =>
    {
        if (localDotnet)
            SetDotNetEnvironmentVariables();

        var dn = localDotnet ? dotnetPath : "dotnet";

        CleanDirectories("../templatesTest/");

        // Create empty Directory.Build.props/targets
        EnsureDirectoryExists(Directory("../templatesTest/"));
        FileWriteText(File("../templatesTest/Directory.Build.props"), "<Project/>");
        FileWriteText(File("../templatesTest/Directory.Build.targets"), "<Project/>");
        CopyFileToDirectory(File("./NuGet.config"), Directory("../templatesTest/"));

        var properties = new Dictionary<string, string> {
            // Properties that ensure we don't use cached packages, and *only* the empty NuGet.config
            { "RestoreNoCache", "true" },
            { "RestorePackagesPath", MakeAbsolute(File("../templatesTest/packages")).FullPath },
            { "RestoreConfigFile", MakeAbsolute(File("../templatesTest/nuget.config")).FullPath },

            // Avoid iOS build warning as error on Windows: There is no available connection to the Mac. Task 'VerifyXcodeVersion' will not be executed
            { "CustomBeforeMicrosoftCSharpTargets", MakeAbsolute(File("./src/Templates/TemplateTestExtraTargets.targets")).FullPath },
        };

        foreach (var template in new [] { "maui", "maui-blazor", "mauilib" })
        {
            var name = template.Replace("-", "") + " Space-Dash";
            StartProcess(dn, $"new {template} -o \"../templatesTest/{name}\"");

            RunMSBuildWithDotNet($"../templatesTest/{name}", properties, warningsAsError: true);
        }
    });

Task("dotnet-test")
    .IsDependentOn("dotnet")
    .Description("Build the solutions")
    .Does(() =>
    {
        var tests = new []
        {
            "**/Controls.Core.UnitTests-net6.csproj",
            "**/Controls.Xaml.UnitTests-net6.csproj",
            "**/Core.UnitTests-net6.csproj",
            "**/Essentials.UnitTests-net6.csproj",
            "**/Resizetizer.UnitTests-net6.csproj",
            "**/Controls.Sample.Tests.csproj"
        };

        var success = true;

        foreach (var test in tests)
        {
            foreach (var project in GetFiles(test))
            {
                try
                {
                    RunTestWithLocalDotNet(project.FullPath);
                }
                catch
                {
                    success = false;
                }
            }
        }

        if (!success)
            throw new Exception("Some tests failed. Check the logs or test results.");
    });

Task("dotnet-pack")
    .Description("Build and create .NET 6 NuGet packages")
    .Does(() =>
    {
        DotNetCoreTool("pwsh", new DotNetCoreToolSettings
        {
            DiagnosticOutput = true,
            ArgumentCustomization = args => args.Append($"./eng/package.ps1 -configuration \"{configuration}\"")
        });

        // Download some additional symbols that need to be archived along with the maui symbols:
        //  - _NativeAssets.windows
        //     - libSkiaSharp.pdb
        //     - libHarfBuzzSharp.pdb
        var assetsDir = "./artifacts/additional-assets";
        var nativeAssetsVersion = XmlPeek("./eng/Microsoft.Extensions.targets", "/Project/PropertyGroup/_SkiaSharpNativeAssetsVersion");
        NuGetInstall("_NativeAssets.windows", new NuGetInstallSettings
        {
            Version = nativeAssetsVersion,
            ExcludeVersion  = true,
            OutputDirectory = assetsDir,
            Source = new[] { "https://aka.ms/skiasharp-eap/index.json" },
        });
        foreach (var nupkg in GetFiles($"{assetsDir}/**/*.nupkg"))
            DeleteFile(nupkg);
        Zip(assetsDir, $"{assetsDir}.zip");
    });

Task("dotnet-build-test")
    .IsDependentOn("dotnet")
    .IsDependentOn("dotnet-buildtasks")
    .IsDependentOn("dotnet-build")
    .IsDependentOn("dotnet-test");

// Tasks for Local Development

Task("VS-DOGFOOD")
    .Description("Provisions .NET 6 and launches an instance of Visual Studio using it.")
    .IsDependentOn("dotnet")
    .Does(() =>
    {
        StartVisualStudioForDotNet6(null);
    });

Task("VS-NET6")
    .Description("Provisions .NET 6 and launches an instance of Visual Studio using it.")
    .IsDependentOn("Clean")
    .IsDependentOn("dotnet")
    .IsDependentOn("dotnet-buildtasks")
    .Does(() =>
    {
        // VS has trouble building all the references correctly so this makes sure everything is built
        // and we're ready to go right when VS launches
        
        RunMSBuildWithDotNet("./src/Compatibility/Android.FormsViewGroup/src/Compatibility.Android.FormsViewGroup-net6.csproj");
        RunMSBuildWithDotNet("./src/Compatibility/Core/src/Compatibility-net6.csproj");
        StartVisualStudioForDotNet6();
    });

Task("VS-WINUI")
    .Description("Provisions .NET 6 and launches an instance of Visual Studio with WinUI projects.")
        .IsDependentOn("VS-NET6");
    //  .IsDependentOn("dotnet") WINUI currently can't launch application with local dotnet
    //  .IsDependentOn("dotnet-buildtasks")

Task("VS-ANDROID")
    .Description("Provisions .NET 6 and launches an instance of Visual Studio with Android projects.")
    .IsDependentOn("Clean")
    .IsDependentOn("dotnet")
    .IsDependentOn("dotnet-buildtasks")
    .Does(() =>
    {
        DotNetCoreRestore("./Microsoft.Maui-net6.sln", new DotNetCoreRestoreSettings
        {
            ToolPath = dotnetPath
        });

        // VS has trouble building all the references correctly so this makes sure everything is built
        // and we're ready to go right when VS launches
        RunMSBuildWithDotNet("./src/Controls/samples/Controls.Sample/Maui.Controls.Sample-net6.csproj");
        StartVisualStudioForDotNet6("./Microsoft.Maui.Droid.sln");
    });

Task("SAMPLE-ANDROID")
    .Description("Provisions .NET 6 and launches Android Sample.")
    .IsDependentOn("dotnet")
    .IsDependentOn("dotnet-buildtasks")
    .Does(() =>
    {
        RunMSBuildWithDotNet("./src/Controls/samples/Controls.Sample.Droid/Maui.Controls.Sample.Droid-net6.csproj", deployAndRun: true);
    });

Task("SAMPLE-IOS")
    .Description("Provisions .NET 6 and launches launches iOS Sample.")
    .IsDependentOn("dotnet")
    .IsDependentOn("dotnet-buildtasks")
    .Does(() =>
    {
        RunMSBuildWithDotNet("./src/Controls/samples/Controls.Sample.iOS/Maui.Controls.Sample.iOS-net6.csproj", deployAndRun: true);
    });

Task("SAMPLE-MAC")
    .Description("Provisions .NET 6 and launches Mac Catalyst Sample.")
    .IsDependentOn("dotnet")
    .IsDependentOn("dotnet-buildtasks")
    .Does(() =>
    {
        RunMSBuildWithDotNet("./src/Controls/samples/Controls.Sample.MacCatalyst/Maui.Controls.Sample.MacCatalyst-net6.csproj", deployAndRun: true);
    });


string FindMSBuild()
{
    if (!string.IsNullOrWhiteSpace(MSBuildExe))
        return MSBuildExe;

    if (IsRunningOnWindows())
    {
        var vsInstallation = VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild", IncludePrerelease = true });
        if (vsInstallation != null)
        {
            var path = vsInstallation.CombineWithFilePath(@"MSBuild\Current\Bin\MSBuild.exe");
            if (FileExists(path))
                return path.FullPath;

            path = vsInstallation.CombineWithFilePath(@"MSBuild\15.0\Bin\MSBuild.exe");
            if (FileExists(path))
                return path.FullPath;
        }
    }
    return "msbuild";
}

void SetDotNetEnvironmentVariables()
{
    var dotnet = MakeAbsolute(Directory("./bin/dotnet/")).ToString();

    SetEnvironmentVariable("DOTNET_INSTALL_DIR", dotnet);
    SetEnvironmentVariable("DOTNET_ROOT", dotnet);
    SetEnvironmentVariable("DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR", dotnet);
    SetEnvironmentVariable("DOTNET_MULTILEVEL_LOOKUP", "0");
    SetEnvironmentVariable("MSBuildEnableWorkloadResolver", "true");
    SetEnvironmentVariable("PATH", dotnet, prepend: true);
}

void StartVisualStudioForDotNet6(string sln = null)
{
    if (sln == null)
    {
        if (IsRunningOnWindows())
        {
            sln = "./Microsoft.Maui-net6.sln";
        }
        else
        {
            sln = "./Microsoft.Maui-mac.slnf";
        }
    }
    if (isCIBuild)
    {
        Information("This target should not run on CI.");
        return;
    }
    if(localDotnet)
    {
        SetDotNetEnvironmentVariables();
        SetEnvironmentVariable("_ExcludeMauiProjectCapability", "true");
    }
    if (IsRunningOnWindows())
    {
        bool includePrerelease = true;

        if (!String.IsNullOrEmpty(vsVersion))
            includePrerelease = (vsVersion == "preview");

        var vsLatest = VSWhereLatest(new VSWhereLatestSettings { IncludePrerelease = includePrerelease, });
        if (vsLatest == null)
            throw new Exception("Unable to find Visual Studio!");
       
        StartProcess(vsLatest.CombineWithFilePath("./Common7/IDE/devenv.exe"), sln);
    }
    else
    {
        StartProcess("open", new ProcessSettings{ Arguments = sln });
    }
}

// NOTE: These methods work as long as the "dotnet" target has already run

void RunMSBuildWithDotNet(string sln, Dictionary<string, string> properties = null, bool deployAndRun = false, bool warningsAsError = false)
{
    var name = System.IO.Path.GetFileNameWithoutExtension(sln);
    var binlog = $"\"{logDirectory}/{name}-{configuration}.binlog\"";
    
    if(localDotnet)
        SetDotNetEnvironmentVariables();

    // If we're not on Windows, use ./bin/dotnet/dotnet
    if (!IsRunningOnWindows() || deployAndRun)
    {
        var msbuildSettings = new DotNetCoreMSBuildSettings()
            .SetConfiguration(configuration)
            .EnableBinaryLogger(binlog);
        if (warningsAsError)
        {
            msbuildSettings.TreatAllWarningsAs(MSBuildTreatAllWarningsAs.Error);
        }

        if (properties != null)
        {
            foreach (var property in properties)
            {
                msbuildSettings.WithProperty(property.Key, property.Value);
            }
        }

        if (deployAndRun)
            msbuildSettings.WithTarget("Run");

        var dotnetBuildSettings = new DotNetCoreBuildSettings
        {
            MSBuildSettings = msbuildSettings,
        };

        if (localDotnet)
            dotnetBuildSettings.ToolPath = dotnetPath;

        DotNetCoreBuild(sln, dotnetBuildSettings);
    }
    else
    {
        // Otherwise we need to run MSBuild for WinUI support
        var msbuild = FindMSBuild();
        Information("Using MSBuild: {0}", msbuild);
        var msbuildSettings = new MSBuildSettings { ToolPath = msbuild }
            .WithRestore()
            .SetConfiguration(configuration)
            .EnableBinaryLogger(binlog);
        if (warningsAsError)
        {
            msbuildSettings.WarningsAsError = true;
        }

        if (properties != null)
        {
            foreach (var property in properties)
            {
                msbuildSettings.WithProperty(property.Key, property.Value);
            }
        }

        MSBuild(sln, msbuildSettings);
    }
}

void RunTestWithLocalDotNet(string csproj)
{
    var name = System.IO.Path.GetFileNameWithoutExtension(csproj);
    var binlog = $"{logDirectory}/{name}-{configuration}.binlog";
    var results = $"{name}-{configuration}.trx";

    if(localDotnet)
        SetDotNetEnvironmentVariables();

    DotNetCoreTest(csproj,
        new DotNetCoreTestSettings
        {
            Configuration = configuration,
            ToolPath = dotnetPath,
            NoBuild = true,
            Logger = $"trx;LogFileName={results}",
            ResultsDirectory = testResultsDirectory,
            ArgumentCustomization = args => args.Append($"-bl:{binlog}")
        });
}
