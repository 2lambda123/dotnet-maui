namespace Microsoft.Maui.ApplicationModel.Implementations
{
	/// <include file="../../docs/Microsoft.Maui.Essentials/AppInfo.xml" path="Type[@FullName='Microsoft.Maui.Essentials.AppInfo']/Docs" />
	public class AppInfoImplementation : IAppInfo
	{
		public string PackageName => throw ExceptionUtils.NotSupportedOrImplementedException;

		public string Name => throw ExceptionUtils.NotSupportedOrImplementedException;

		public System.Version Version => Utils.ParseVersion(VersionString);

		public string VersionString => throw ExceptionUtils.NotSupportedOrImplementedException;

		public string BuildString => throw ExceptionUtils.NotSupportedOrImplementedException;

		public void ShowSettingsUI() => throw ExceptionUtils.NotSupportedOrImplementedException;

		public AppTheme RequestedTheme => AppTheme.Unspecified;

		public AppPackagingModel PackagingModel => throw ExceptionUtils.NotSupportedOrImplementedException;

		public LayoutDirection RequestedLayoutDirection => throw ExceptionUtils.NotSupportedOrImplementedException;
	}
}
