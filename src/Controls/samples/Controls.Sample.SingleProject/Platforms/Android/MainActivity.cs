using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;

using AndroidX.Window.Layout;
using AndroidX.Window.Java.Layout;
using Java.Interop;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using System;
using Android.Content.Res;

namespace Maui.Controls.Sample.Droid
{
	[Activity(
		Label = "@string/app_name",
		Theme = "@style/Maui.SplashTheme",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize
		| ConfigChanges.Orientation
		| ConfigChanges.ScreenLayout
		| ConfigChanges.UiMode
		| ConfigChanges.SmallestScreenSize
		| ConfigChanges.KeyboardHidden
		)]
	[IntentFilter(
		new[] { Microsoft.Maui.Essentials.Platform.Intent.ActionAppAction },
		Categories = new[] { Android.Content.Intent.CategoryDefault })]
	public class MainActivity : MauiAppCompatActivity, AndroidX.Core.Util.IConsumer
		, Microsoft.Maui.Controls.DualScreen.IFoldableContext
	{
		WindowInfoTrackerCallbackAdapter wir;
		IWindowMetricsCalculator wmc;

		#region IFoldableContext properties
		public bool isSeparating { get; set; }
		public Rectangle FoldingFeatureBounds { get; set; }
		public Rectangle WindowBounds { get; set; }
		public event EventHandler<Microsoft.Maui.Controls.DualScreen.FoldEventArgs> FoldingFeatureChanged;
		#endregion

		protected override void OnCreate(Bundle savedInstanceState)
		{
			Android.Util.Log.Debug("JWM", "Activity.OnCreate - DualScreenService.Init");
			Microsoft.Maui.Controls.DualScreen.DualScreenService.Init(this);
			base.OnCreate(savedInstanceState);

			wir = new WindowInfoTrackerCallbackAdapter(WindowInfoTracker.Companion.GetOrCreate(this));
			wmc = WindowMetricsCalculator.Companion.OrCreate; // HACK: source method is `getOrCreate`, binding generator munges this badly :(
		}

		protected override void OnStart()
		{
			base.OnStart();
			wir.AddWindowLayoutInfoListener(this, runOnUiThreadExecutor(), this); // `this` is the IConsumer implementation
		}

		protected override void OnStop()
		{
			base.OnStop();
			wir.RemoveWindowLayoutInfoListener(this);
		}

		
		#region Used by WindowInfoRepository callback
		Java.Util.Concurrent.IExecutor runOnUiThreadExecutor()
		{
			return new MyExecutor();
		}
		class MyExecutor : Java.Lang.Object, Java.Util.Concurrent.IExecutor
		{
			Handler handler = new Handler(Looper.MainLooper);
			public void Execute(Java.Lang.IRunnable r)
			{
				handler.Post(r);
			}
		}

		public void Accept(Java.Lang.Object windowLayoutInfo)  // Object will be WindowLayoutInfo
		{
			var newLayoutInfo = windowLayoutInfo as WindowLayoutInfo;

			var curWinBounds = wmc.ComputeCurrentWindowMetrics(this).Bounds;
			WindowBounds = new Rectangle(curWinBounds.Left, curWinBounds.Top,
										curWinBounds.Width(), curWinBounds.Height());

			Android.Util.Log.Info("JWM", "=== LayoutStateChangeCallback.Accept");
			Android.Util.Log.Info("JWM", "    "+newLayoutInfo.ToString());

			isSeparating = false; // we don't know if we'll find a displayFeature of not
			FoldingFeatureBounds = Rectangle.Zero;

			foreach (var displayFeature in newLayoutInfo.DisplayFeatures)
			{
				var foldingFeature = displayFeature.JavaCast<IFoldingFeature>();

				if (foldingFeature != null) // HACK: requires JavaCast as shown above
				{
					isSeparating = foldingFeature.IsSeparating;

					FoldingFeatureBounds = new Rectangle(foldingFeature.Bounds.Left, foldingFeature.Bounds.Top, 
														foldingFeature.Bounds.Width(), foldingFeature.Bounds.Height());

					Android.Util.Log.Info("JWM", "\n    IsSeparating: " + foldingFeature.IsSeparating
							+ "\n    Orientation: " + foldingFeature.Orientation  // FoldingFeature.OrientationVertical or Horizontal
							+ "\n    State: " + foldingFeature.State // FoldingFeature.StateFlat or StateHalfOpened
					);
				}
				else
				{
					Android.Util.Log.Info("JWM", "DisplayFeature is not a fold or hinge (shouldn't happen currently)");
				}
			}
			Android.Util.Log.Info("JWM", "=== FoldingFeatureChanged?.Invoke");
			FoldingFeatureChanged?.Invoke(this, new Microsoft.Maui.Controls.DualScreen.FoldEventArgs()
			{
				isSeparating = isSeparating,
				FoldingFeatureBounds = FoldingFeatureBounds,
				WindowBounds = WindowBounds
			});
		}
		#endregion
	}
}