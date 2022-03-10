﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.TestUtils.DeviceTests.Runners;

namespace Microsoft.Maui.DeviceTests
{
	public static class MauiProgram
	{
#if ANDROID
		public static Android.Content.Context CurrentContext { get; private set; }
#elif WINDOWS
		public static Microsoft.UI.Xaml.Window CurrentWindow { get; private set; }
#endif

		public static Application CurrentTestApp { get; private set; }

		public static MauiApp CreateMauiApp()
		{
			var appBuilder = MauiApp
				.CreateBuilder()
				.ConfigureLifecycleEvents(life =>
				{
#if ANDROID
					life.AddAndroid(android =>
					{
						android.OnCreate((a, b) => CurrentContext = a);
					});
#elif WINDOWS
					life.AddWindows(windows =>
					{
						windows.OnWindowCreated(win => CurrentWindow = win);
					});
#endif
				})
				.ConfigureTests(new TestOptions
				{
					Assemblies =
					{
						typeof(MauiProgram).Assembly,
					},
				})
				.UseHeadlessRunner(new HeadlessRunnerOptions
				{
					RequiresUIContext = true,
				})
				.UseVisualRunner();

			var mauiApp = appBuilder.Build();

			CurrentTestApp = (Application)mauiApp.Services.GetRequiredService<IApplication>();

			return mauiApp;
		}
	}
}