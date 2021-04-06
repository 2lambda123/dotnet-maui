using System;
using Microsoft.Extensions.DependencyInjection;
using UIKit;

namespace Microsoft.Maui.Handlers
{
	public partial class ButtonHandler : ViewHandler<IButton, UIButton>
	{
		static readonly UIControlState[] ControlStates = { UIControlState.Normal, UIControlState.Highlighted, UIControlState.Disabled };

		static UIColor? ButtonTextColorDefaultDisabled;
		static UIColor? ButtonTextColorDefaultHighlighted;
		static UIColor? ButtonTextColorDefaultNormal;

		protected override UIButton CreateNativeView()
		{
			SetControlPropertiesFromProxy();
			return new UIButton(UIButtonType.System);
		}

		protected override void ConnectHandler(UIButton nativeView)
		{
			nativeView.TouchUpInside += OnButtonTouchUpInside;
			nativeView.TouchUpOutside += OnButtonTouchUpOutside;
			nativeView.TouchDown += OnButtonTouchDown;

			base.ConnectHandler(nativeView);
		}

		protected override void DisconnectHandler(UIButton nativeView)
		{
			nativeView.TouchUpInside -= OnButtonTouchUpInside;
			nativeView.TouchUpOutside -= OnButtonTouchUpOutside;
			nativeView.TouchDown -= OnButtonTouchDown;

			base.DisconnectHandler(nativeView);
		}

		protected override void SetupDefaults(UIButton nativeView)
		{
			ButtonTextColorDefaultNormal = nativeView.TitleColor(UIControlState.Normal);
			ButtonTextColorDefaultHighlighted = nativeView.TitleColor(UIControlState.Highlighted);
			ButtonTextColorDefaultDisabled = nativeView.TitleColor(UIControlState.Disabled);

			base.SetupDefaults(nativeView);
		}

		public static void MapBackgroundColor(ButtonHandler handler, IButton button)
		{
			handler.NativeView?.UpdateBackgroundColor(button);
		}

		public static void MapText(ButtonHandler handler, IButton button)
		{
			handler.NativeView?.UpdateText(button);
		}

		public static void MapTextColor(ButtonHandler handler, IButton button)
		{
			handler.NativeView?.UpdateTextColor(button, ButtonTextColorDefaultNormal, ButtonTextColorDefaultHighlighted, ButtonTextColorDefaultDisabled);
		}

		public static void MapCharacterSpacing(ButtonHandler handler, IButton button)
		{
			handler.TypedNativeView?.UpdateCharacterSpacing(button);
		}

		public static void MapPadding(ButtonHandler handler, IButton button)
		{
			handler.NativeView?.UpdatePadding(button);
		}

		public static void MapFont(ButtonHandler handler, IButton button)
		{
			_ = handler.Services ?? throw new InvalidOperationException($"{nameof(Services)} should have been set by base class.");

			var fontManager = handler.Services.GetRequiredService<IFontManager>();

			handler.NativeView?.UpdateFont(button, fontManager);
		}

		void SetControlPropertiesFromProxy()
		{
			if (NativeView == null)
				return;

			foreach (UIControlState uiControlState in ControlStates)
			{
				NativeView.SetTitleColor(UIButton.Appearance.TitleColor(uiControlState), uiControlState); // If new values are null, old values are preserved.
				NativeView.SetTitleShadowColor(UIButton.Appearance.TitleShadowColor(uiControlState), uiControlState);
				NativeView.SetBackgroundImage(UIButton.Appearance.BackgroundImageForState(uiControlState), uiControlState);
			}
		}

		void OnButtonTouchUpInside(object? sender, EventArgs e)
		{
			VirtualView?.Released();
			VirtualView?.Clicked();
		}

		void OnButtonTouchUpOutside(object? sender, EventArgs e)
		{
			VirtualView?.Released();
		}

		void OnButtonTouchDown(object? sender, EventArgs e)
		{
			VirtualView?.Pressed();
		}
	}
}