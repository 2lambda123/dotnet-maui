using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Essentials;

namespace Microsoft.Maui.Controls.DualScreen
{
    internal class NoDualScreenServiceImpl : IDualScreenService
    {
		static Lazy<NoDualScreenServiceImpl> _Instance = new Lazy<NoDualScreenServiceImpl>(() => new NoDualScreenServiceImpl());
        public static NoDualScreenServiceImpl Instance => _Instance.Value;

		readonly WeakEventManager _onScreenChangedEventManager = new WeakEventManager();
		readonly WeakEventManager _onLayoutChangedEventManager = new WeakEventManager();
		public NoDualScreenServiceImpl()
        {
			//HACK:FOLDABLE 
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
		public Rectangle GetHinge()
        {
            return Rectangle.Zero;
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
