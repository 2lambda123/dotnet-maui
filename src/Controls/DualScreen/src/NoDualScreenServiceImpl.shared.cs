using System;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Essentials;

namespace Microsoft.Maui.Foldable
{
    internal class NoDualScreenServiceImpl : IFoldableService
    {
		static Lazy<NoDualScreenServiceImpl> _Instance = new Lazy<NoDualScreenServiceImpl>(() => new NoDualScreenServiceImpl());
        public static NoDualScreenServiceImpl Instance => _Instance.Value;

		readonly WeakEventManager _onScreenChangedEventManager = new WeakEventManager();
		readonly WeakEventManager _onLayoutChangedEventManager = new WeakEventManager();
		public NoDualScreenServiceImpl()
        {
			System.Diagnostics.Debug.Write("NoDualScreenServiceImpl.ctor", "JWM");
			DeviceDisplay.MainDisplayInfoChanged += OnDeviceInfoChanged;
		}

		public Task<int> GetHingeAngleAsync() => Task.FromResult(0);

		public bool IsSpanned => false;

        public bool IsLandscape => DeviceDisplay.MainDisplayInfo.Orientation.IsLandscape();
		
		public DisplayInfo DeviceInfo => DeviceDisplay.MainDisplayInfo;

		public event EventHandler OnScreenChanged
		{
			add { _onScreenChangedEventManager.AddEventHandler(value); }
			remove { _onScreenChangedEventManager.RemoveEventHandler(value); }
		}
		public event EventHandler<FoldEventArgs> OnLayoutChanged
		{
			add { _onLayoutChangedEventManager.AddEventHandler(value); }
			remove { _onLayoutChangedEventManager.RemoveEventHandler(value); }
		}
		public void Dispose()
        {
        }

		public Size ScaledScreenSize => DeviceDisplay.MainDisplayInfo.GetScaledScreenSize();
		public Rect GetHinge()
        {
            return Rect.Zero;
        }

        public Point? GetLocationOnScreen(VisualElement visualElement)
        {
            return null;
        }

		public object WatchForChangesOnLayout(VisualElement visualElement, Action action)
		{
			if (action == null)
				return null;

			EventHandler<EventArg<VisualElement>> layoutUpdated = (_, __) =>
			{
				action();
			};

			visualElement.BatchCommitted += layoutUpdated;
			return layoutUpdated;
		}

		public void StopWatchingForChangesOnLayout(VisualElement visualElement, object handle)
		{
			if (handle is EventHandler<EventArg<VisualElement>> handler)
				visualElement.BatchCommitted -= handler;
		}

		void OnDeviceInfoChanged(object sender, DisplayInfoChangedEventArgs e)
		{
			_onScreenChangedEventManager.HandleEvent(this, e, nameof(OnScreenChanged));
		}
	}
}
