using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.DualScreen;
using Microsoft.Maui.Graphics;
using AView = Android.Views.View;

[assembly: Dependency(typeof(DualScreenService.DualScreenServiceImpl))]

namespace Microsoft.Maui.Controls.DualScreen
{
	public class DualScreenService
	{
		[Internals.Preserve(Conditional = true)]
		public DualScreenService()
		{

		}

		public static void Init(IFoldableContext activity)
		{
			global::Android.Util.Log.Debug("JWM", "DualScreenService.Init - Android detected");
			DependencyService.Register<DualScreenServiceImpl>();
			DualScreenServiceImpl.Init(activity);
		}

		internal class DualScreenServiceImpl : IDualScreenService //HACK: FOLDABLE, Platform.Android.DualScreen.IDualScreenService
		{
			ScreenHelper _helper;
			//[Obsolete("Should not care about specific device model")]
			//bool _isDuo = false;
			[Obsolete("Should not care about specific device model")]
			bool IsDuo => _isSpanned; //HACK:FOLDABLE (_helper == null || _HingeService == null || _mainActivity == null || _singleUseHingeSensor == null) ? false : _isDuo;
			HingeSensor _singleUseHingeSensor;
			static IFoldableContext _mainActivity;
			static DualScreenServiceImpl _HingeService;
			//[Obsolete("don't rely on custom 'landscape'")]
			bool _isLandscape;
			Size _pixelScreenSize;
			object _hingeAngleLock = new object();
			TaskCompletionSource<int> _gettingHingeAngle;
			bool _isSpanned;
			Rectangle _hingeDp = Rectangle.Zero;

			internal static Activity MainActivity => (_mainActivity as Activity);

			static HingeSensor DefaultHingeSensor;
			readonly WeakEventManager _onScreenChangedEventManager = new WeakEventManager();

			public event EventHandler<FoldEventArgs> OnLayoutChanged;

			[Internals.Preserve(Conditional = true)]
			public DualScreenServiceImpl()
			{
				//HACK:FOLDABLE 
				global::Android.Util.Log.Debug("JWM", "DualScreenServiceImpl.ctor - Android detected");

				_HingeService = this;
				if (_mainActivity != null)
				{ 
					Init(_mainActivity);
					(_mainActivity as IFoldableContext).FoldingFeatureChanged += DualScreenServiceImpl_FoldingFeatureChanged;
				}
			}

			private void DualScreenServiceImpl_FoldingFeatureChanged(object sender, FoldEventArgs ea)
			{
				global::Android.Util.Log.Debug("JWM", "DualScreenServiceImpl.DualScreenServiceImpl_FoldingFeatureChanged");
				global::Android.Util.Log.Debug("JWM", "   " + ea);

				_isLandscape = (ea.WindowBounds.Width >= ea.WindowBounds.Height);
				_isSpanned = ea.isSeparating;

				_helper.FoldingFeatureBounds = ea.FoldingFeatureBounds;
				_helper.WindowBounds = ea.WindowBounds;
				_helper.IsSpanned = ea.isSeparating;

				using (global::Android.Util.DisplayMetrics display = (_mainActivity as Activity).Resources.DisplayMetrics)
				{
					var scalingFactor = display.Density;
					_pixelScreenSize = new Size(ea.WindowBounds.Width, ea.WindowBounds.Height);
					var newSize = new Size(_pixelScreenSize.Width / scalingFactor, _pixelScreenSize.Height / scalingFactor);

					if (newSize != ScaledScreenSize)
					{
						ScaledScreenSize = newSize;
					}
				}

				//TODO: fix this?
				Update(); //HACK:FOLDABLE enabling this prevents the hinge being shown?!?
				//_helper.Update();
				OnLayoutChanged?.Invoke(sender, ea);
			}

			public static void Init(IFoldableContext activity)
			{
				//HACK:FOLDABLE 
				
				global::Android.Util.Log.Debug("JWM", "DualScreenServiceImpl.Init - Android detected");

				if (_HingeService == null)
				{
					_mainActivity = activity;
					return;
				}

				if (activity == _mainActivity && _HingeService._helper != null)
				{
					_HingeService?.Update();
					return;
				}

				if (_mainActivity is IDeviceInfoProvider oldDeviceInfoProvider)
					oldDeviceInfoProvider.ConfigurationChanged -= _HingeService.ConfigurationChanged;

				_mainActivity = activity;

				if (_mainActivity == null)
					return;

				
				//bool isDuo = _HingeService._isDuo = ScreenHelper.IsDualScreenDevice(_mainActivity);
				//if (!isDuo)
				//{
					if (_mainActivity is IDeviceInfoProvider infoProvider)
					{
						infoProvider.ConfigurationChanged += _HingeService.ConfigurationChanged;
					}

					//return;
				//}

				var screenHelper = _HingeService._helper ?? new ScreenHelper(activity);
				//isDuo = screenHelper.Initialize(_mainActivity);
				//_HingeService._isDuo = isDuo;

				//if (!isDuo)
				//{
				//	_HingeService._helper = null;
				//	_HingeService.SetupHingeSensors(null);
				//	return;
				//}

				// Hinge service is set up for every device - figure out how to NOT do that (based on hinge existing?)
				_HingeService._helper = screenHelper;
				_HingeService.SetupHingeSensors(_mainActivity as Activity);
				if (_mainActivity is IDeviceInfoProvider newDeviceInfoProvider)
				{
					newDeviceInfoProvider.ConfigurationChanged += _HingeService.ConfigurationChanged;
				}

				_HingeService?.Update();
			}


			public Size ScaledScreenSize
			{
				get;
				private set;
			}

			public event EventHandler OnScreenChanged
			{
				add { _onScreenChangedEventManager.AddEventHandler(value); }
				remove { _onScreenChangedEventManager.RemoveEventHandler(value); }
			}

			void Update()
			{
				
				_isSpanned = _helper?.IsDualMode ?? false; //HACK:FOLDABLE /*IsDuo &&*/

				global::Android.Util.Log.Debug("JWM", "DualScreenServiceImpl.Update _isSpanned:" + _isSpanned);

				// Hinge
				if (!_isSpanned)
				{
					_hingeDp = Rectangle.Zero;
				}
				else // IsSpanned
				{
					var hinge = _helper.GetHingeBoundsDip();

					if (hinge == null || !IsSpanned)
					{
						_hingeDp = Rectangle.Zero;
					}
					else
					{
						_hingeDp = new Rectangle((hinge.Left), (hinge.Top), (hinge.Width()), (hinge.Height()));
					}
				}


				//HACK:FOLDABLE
				using (global::Android.Util.DisplayMetrics display = (_mainActivity as Activity).Resources.DisplayMetrics)
				{
					var scalingFactor = display.Density;
					_pixelScreenSize = new Size(display.WidthPixels, display.HeightPixels);
					var newSize = new Size(_pixelScreenSize.Width / scalingFactor, _pixelScreenSize.Height / scalingFactor);

					if (newSize != ScaledScreenSize)
					{
						ScaledScreenSize = newSize;
						//screenChanged = true;
					}
				}

				// Is Landscape
				//if (!IsDuo)
				//{
				//if (_mainActivity == null)
				//	_isLandscape = false;
				//else
				//{
				//	//var orientation = (_mainActivity as Activity).Resources.Configuration.Orientation;
				//	//_isLandscape = (orientation == global::Android.Content.Res.Orientation.Landscape);
				//	_isLandscape = (_mainActivity.WindowBounds.Width >= _mainActivity.WindowBounds.Height);
				//}
				//}
				//else
				//{

				//	var rotation = ScreenHelper.GetRotation(_helper.Activity);
				//	_isLandscape = (rotation == SurfaceOrientation.Rotation270 || rotation == SurfaceOrientation.Rotation90);
				//}
				global::Android.Util.Log.Debug("JWM", "                             _isLandscape:" + _isLandscape);
			}

			public bool IsSpanned => _isSpanned;

			public Task<int> GetHingeAngleAsync()
			{
				//if (!IsDuo)
				//	return Task.FromResult(0);

				Task<int> returnValue = null;
				lock (_hingeAngleLock)
				{
					if (_gettingHingeAngle == null)
					{
						_gettingHingeAngle = new TaskCompletionSource<int>();
						StartListeningForHingeChanges();
					}

					returnValue = _gettingHingeAngle.Task;
				}

				return returnValue;
			}

			public Rectangle GetHinge() => _hingeDp;
			[Obsolete("shouldn't matter")]
			public bool IsDualScreenDevice => IsDuo;
			public bool IsLandscape
			{ //=> _isLandscape;
				get {
					if (_mainActivity == null)
						return false;
					else
					{
						return (_mainActivity.WindowBounds.Width >= _mainActivity.WindowBounds.Height);
					}
				}
				
			}

			public Point? GetLocationOnScreen(VisualElement visualElement)
			{
				//HACK:FOLDABLE var view = Platform.Android.Platform.GetRenderer(visualElement);
				//var androidView = view?.View;
				var androidView = visualElement.Handler?.NativeView as AView;

				if (!androidView.IsAlive())
					return null;

				int[] location = new int[2];
				androidView.GetLocationOnScreen(location);
				return new Point(androidView.Context.FromPixels(location[0]), androidView.Context.FromPixels(location[1]));
			}

			public object WatchForChangesOnLayout(VisualElement visualElement, Action action)
			{
				if (action == null)
					return null;

				//HACK:FOLDABLE var view = Platform.Android.Platform.GetRenderer(visualElement);
				//var androidView = view?.View;
				var androidView = visualElement.Handler?.NativeView as AView;

				if (androidView == null || !androidView.IsAlive())
					return null;

				var listener = new DualScreenGlobalLayoutListener(action, androidView);

				var table = new System.Runtime.CompilerServices.ConditionalWeakTable<AView, DualScreenGlobalLayoutListener>();
				androidView.ViewTreeObserver.AddOnGlobalLayoutListener(listener);
				table.Add(androidView, listener);
				return table;
			}

			public void StopWatchingForChangesOnLayout(VisualElement visualElement, object handle)
			{
				if (!(handle is System.Runtime.CompilerServices.ConditionalWeakTable<AView, DualScreenGlobalLayoutListener> table))
					return;

				DualScreenGlobalLayoutListener ggl = null;
				//HACK:FOLDABLE var view = Platform.Android.Platform.GetRenderer(visualElement);
				//var androidView = view?.View;
				var androidView = visualElement.Handler?.NativeView as AView;

				if (androidView == null || !(table.TryGetValue(androidView, out ggl)))
				{
					foreach (var pair in table)
						ggl = pair.Value;
				}

				if (ggl == null)
					return;

				try
				{
					ggl.Invalidate();
				}
				catch
				{
					// just in case something along the call path here is disposed of
				}

				if (androidView == null || !androidView.IsAlive())
					return;

				try
				{
					androidView.ViewTreeObserver.RemoveOnGlobalLayoutListener(ggl);
				}
				catch
				{
					// just in case something along the call path here is disposed of
				}
			}

			class DualScreenGlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
			{
				WeakReference<Action> _callback;
				WeakReference<AView> _view;

				public DualScreenGlobalLayoutListener(Action callback, AView view)
				{
					_callback = new WeakReference<Action>(callback);
					_view = new WeakReference<AView>(view);
				}

				public void OnGlobalLayout()
				{
					if (_view == null || _callback == null)
						return;

					Action invokeMe = null;
					AView view = null;

					if (!_view.TryGetTarget(out view) || !view.IsAlive())
					{
						Invalidate(view);
					}
					else if (_callback.TryGetTarget(out invokeMe))
					{
						invokeMe();
					}
					else
					{
						Invalidate(view);
					}
				}

				protected override void Dispose(bool disposing)
				{
					if (disposing)
						Invalidate(null);

					base.Dispose(disposing);
				}

				internal void Invalidate()
				{
					AView view = null;
					_view.TryGetTarget(out view);
					Invalidate(view);
				}

				// I don't want our code to dispose of this class I'd rather just let the natural
				// process manage the life cycle so we don't dispose of this too early
				void Invalidate(AView androidView)
				{
					if (androidView.IsAlive())
					{
						try
						{
							androidView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
						}
						catch
						{
							// just in case something along the call path here is disposed of
						}
					}

					try
					{
						// If the androidView itself becomes disposed of the listener will survive the life of the view
						// and it will get moved to the root views tree observer
						if (this.IsAlive())
							(_mainActivity as Activity)?.Window?.DecorView?.RootView?.ViewTreeObserver?.RemoveOnGlobalLayoutListener(this);
					}
					catch
					{
						// just in case something along the call path here is disposed of
					}

					_callback = null;
					_view = null;
				}
			}

			static EventHandler<HingeSensor.HingeSensorChangedEventArgs> _hingeAngleChanged;
			static int subscriberCount;
			static object hingeAngleLock = new object();

			public static event EventHandler<HingeSensor.HingeSensorChangedEventArgs> HingeAngleChanged
			{
				add
				{
					if (DefaultHingeSensor == null)
						return;

					ProcessHingeAngleSubscriberCount(Interlocked.Increment(ref subscriberCount));
					_hingeAngleChanged += value;
				}
				remove
				{
					if (DefaultHingeSensor == null)
						return;

					ProcessHingeAngleSubscriberCount(Interlocked.Decrement(ref subscriberCount));
					_hingeAngleChanged -= value;
				}
			}

			static void ProcessHingeAngleSubscriberCount(int subscriberCount)
			{
				var sensor = DefaultHingeSensor;
				if (sensor == null)
					return;

				lock (hingeAngleLock)
				{
					if (subscriberCount == 1)
					{
						sensor.StartListening();
					}
					else if (subscriberCount == 0)
					{
						sensor.StopListening();
					}
				}
			}

			void DefaultHingeSensorOnSensorChanged(object sender, HingeSensor.HingeSensorChangedEventArgs e)
			{
				_hingeAngleChanged?.Invoke(this, e);
			}

			void SetupHingeSensors(global::Android.Content.Context context)
			{
				if (context == null)
				{
					if (DefaultHingeSensor != null)
						DefaultHingeSensor.OnSensorChanged -= DefaultHingeSensorOnSensorChanged;

					_singleUseHingeSensor = null;
					DefaultHingeSensor = null;
				}
				else
				{
					_singleUseHingeSensor = new HingeSensor(context);
					DefaultHingeSensor = new HingeSensor(context);
					DefaultHingeSensor.OnSensorChanged += DefaultHingeSensorOnSensorChanged;
				}
			}
						
			void ConfigurationChanged(object sender, EventArgs e)
			{
				global::Android.Util.Log.Debug("JWM", "DualScreenServiceImpl.ConfigurationChanged IGNORE ConfigurationChanged");
				return;
				////if (IsDuo)
				////{
				//_helper?.Update();
				////}

				//bool wasLandscape = IsLandscape;
				//Update();

				//bool screenChanged = false;
				//if (wasLandscape != IsLandscape)
				//{
				//	screenChanged = true;
				//}

				//if (_mainActivity != null)
				//{
				//	using (global::Android.Util.DisplayMetrics display = (_mainActivity as Activity).Resources.DisplayMetrics)
				//	{
				//		var scalingFactor = display.Density;
				//		_pixelScreenSize = new Size(display.WidthPixels, display.HeightPixels);
				//		var newSize = new Size(_pixelScreenSize.Width / scalingFactor, _pixelScreenSize.Height / scalingFactor);

				//		if (newSize != ScaledScreenSize)
				//		{
				//			ScaledScreenSize = newSize;
				//			screenChanged = true;
				//		}
				//	}
				//}

				//if (screenChanged)
				//	_onScreenChangedEventManager.HandleEvent(this, e, nameof(OnScreenChanged));
			}


			void StartListeningForHingeChanges()
			{
				//if (!IsDuo)
				//	return;

				_singleUseHingeSensor.OnSensorChanged += OnSensorChanged;
				_singleUseHingeSensor.StartListening();
			}

			void StopListeningForHingeChanges()
			{
				//if (!IsDuo)
				//	return;

				_singleUseHingeSensor.OnSensorChanged -= OnSensorChanged;
				_singleUseHingeSensor.StopListening();
			}

			void OnSensorChanged(object sender, HingeSensor.HingeSensorChangedEventArgs e)
			{
				SetHingeAngle(e.HingeAngle);
			}

			void SetHingeAngle(int hingeAngle)
			{
				TaskCompletionSource<int> toSet = null;
				lock (_hingeAngleLock)
				{
					StopListeningForHingeChanges();
					toSet = _gettingHingeAngle;
					_gettingHingeAngle = null;
				}

				if (toSet != null)
					toSet.SetResult(hingeAngle);
			}
		}
	}

	//HACK:FOLDABLE added this from Microsoft.Maui namespace, because otherwise it
	// wasn't getting picked up as valid extension methods otherwise...
	static class JavaObjectExtensions
	{
		public static bool IsDisposed(this Java.Lang.Object obj)
		{
			return obj.Handle == IntPtr.Zero;
		}

		public static bool IsDisposed(this global::Android.Runtime.IJavaObject obj)
		{
			return obj.Handle == IntPtr.Zero;
		}

		public static bool IsAlive(this Java.Lang.Object obj)
		{
			if (obj == null)
				return false;

			return !obj.IsDisposed();
		}

		public static bool IsAlive(this global::Android.Runtime.IJavaObject obj)
		{
			if (obj == null)
				return false;

			return !obj.IsDisposed();
		}
	}
}