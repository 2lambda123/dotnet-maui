using System;

namespace Microsoft.Maui.Essentials
{
	public static partial class MainThread
	{
		static void PlatformBeginInvokeOnMainThread(Action action) =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		static bool PlatformIsMainThread =>
			throw ExceptionUtils.NotSupportedOrImplementedException;
	}
}
