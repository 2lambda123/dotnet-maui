using System;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;

namespace Microsoft.Maui
{
	public partial class MauiAppCompatActivity : AppCompatActivity
	{
		// Override this if you want to handle the default Android behavior of restoring fragments on an application restart
		protected virtual bool AllowFragmentRestore => false;

		protected override void OnCreate(Bundle? savedInstanceState)
		{
			if (!AllowFragmentRestore)
			{
				// Remove the automatically persisted fragment structure; we don't need them
				// because we're rebuilding everything from scratch. This saves a bit of memory
				// and prevents loading errors from child fragment managers
				savedInstanceState?.Remove("android:support:fragments");
				savedInstanceState?.Remove("androidx.lifecycle.BundlableSavedStateRegistry.key");
			}

			// If the theme has the maui_splash attribute, change the theme
			if (Theme.TryResolveAttribute(Resource.Attribute.maui_splash))
			{
				SetTheme(Resource.Style.Maui_MainTheme_NoActionBar);
			}

			base.OnCreate(savedInstanceState);

			var mauiApp = MauiApplication.Current.Application;
			if (mauiApp == null)
				throw new InvalidOperationException($"The {nameof(IApplication)} instance was not found.");

			var services = MauiApplication.Current.Services;
			if (mauiApp == null)
				throw new InvalidOperationException($"The {nameof(IServiceProvider)} instance was not found.");

			var mauiContext = new MauiContext(services, this);

			// TODO Fix once we have multiple windows
			IWindow window;
			if (mauiApp.Windows.Count > 0)
			{
				window = mauiApp.Windows[0];
			}
			else
			{
				var state = new ActivationState(mauiContext, savedInstanceState);
				window = mauiApp.CreateWindow(state);
			}

			SetContentView(window.View.ToContainerView(mauiContext));

			//TODO MAUI
			// Allow users to customize the toolbarid?
			if (Theme.TryResolveAttribute(Resource.Attribute.windowActionBar, out var windowActionBar) &&
				windowActionBar == false)
			{
				var toolbar = FindViewById<Toolbar>(Resource.Id.maui_toolbar);
				if (toolbar != null)
					SetSupportActionBar(toolbar);
			}
		}
	}
}