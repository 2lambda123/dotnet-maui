using System;
using Microsoft.Maui.Animations;

namespace Microsoft.Maui
{
	public interface IMauiContext
	{
		IServiceProvider Services { get; }

		IMauiHandlersServiceProvider Handlers { get; }

#if __ANDROID__
		global::Android.Content.Context? Context { get; }
#endif
	}

	public interface IMauiApplicationContext : IMauiContext
	{
#if __IOS__
		UIKit.UIApplicationDelegate? ApplicationDelegate { get; }
#elif WINDOWS
		UI.Xaml.Application? Application { get; }
#endif
	}

	public interface IMauiWindowContext : IMauiContext
	{
#if __IOS__
		UIKit.UIWindow? Window { get; }
#elif WINDOWS
		UI.Xaml.Window? Window { get; }
#endif
	}

	// TODO: This will be replaced with scoped services
	internal interface IScopedMauiContext : IMauiContext
	{
		IAnimationManager AnimationManager { get; }

#if __ANDROID__
		Android.Views.LayoutInflater? LayoutInflater { get; }
		AndroidX.Fragment.App.FragmentManager? FragmentManager { get; }
#elif WINDOWS
		WindowManager WindowManager { get; }
#endif
	}
}