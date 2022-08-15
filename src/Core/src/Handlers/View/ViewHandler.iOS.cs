﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using PlatformView = UIKit.UIView;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler
	{

		[System.Runtime.Versioning.SupportedOSPlatform("ios13.0")]
		public static void MapContextFlyout(IViewHandler handler, IView view)
		{
#if MACCATALYST
			if (view is IContextFlyoutContainer contextFlyoutContainer)
			{
				MapContextFlyout(handler, contextFlyoutContainer);
			}
#endif
		}

#if MACCATALYST

		// Store a reference to the platform delegate so that it is not garbage collected
		IUIContextMenuInteractionDelegate? _uiContextMenuInteractionDelegate;

		[System.Runtime.Versioning.SupportedOSPlatform("ios13.0")]
		internal static void MapContextFlyout(IElementHandler handler, IContextFlyoutContainer contextFlyoutContainer)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"The handler's {nameof(handler.MauiContext)} cannot be null.");

			if (contextFlyoutContainer.ContextFlyout != null)
			{
				var contextFlyoutHandler = contextFlyoutContainer.ContextFlyout.ToHandler(handler.MauiContext);
				var contextFlyoutPlatformView = contextFlyoutHandler.PlatformView;

				if (handler.PlatformView is PlatformView uiView &&
					contextFlyoutPlatformView is UIMenu uiMenu &&
					handler is ViewHandler viewHandlerObj)
				{
					viewHandlerObj._uiContextMenuInteractionDelegate = new FlyoutUIContextMenuInteractionDelegate(
						() =>
						{
							return UIContextMenuConfiguration.Create(
								identifier: null,
								previewProvider: null,
								actionProvider: _ => uiMenu);
						});

					var newFlyout = new UIContextMenuInteraction(
						@delegate: viewHandlerObj._uiContextMenuInteractionDelegate);

					uiView.AddInteraction(newFlyout);
				}
			}
		}

		sealed class FlyoutUIContextMenuInteractionDelegate : NSObject, IUIContextMenuInteractionDelegate
		{
			private readonly Func<UIContextMenuConfiguration> _menuConfigurationFunc;

			public FlyoutUIContextMenuInteractionDelegate(Func<UIContextMenuConfiguration> menuConfigurationFunc)
			{
				_menuConfigurationFunc = menuConfigurationFunc;
			}

			public UIContextMenuConfiguration? GetConfigurationForMenu(UIContextMenuInteraction interaction, CGPoint location)
			{
				return _menuConfigurationFunc();
			}
		}
#endif

		static partial void MappingFrame(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
			handler.ToPlatform().UpdateBackgroundLayerFrame();
		}

		public static void MapTranslationX(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapTranslationY(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapScale(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapScaleX(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapScaleY(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapRotation(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapRotationX(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapRotationY(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapAnchorX(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapAnchorY(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		internal static void UpdateTransformation(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public virtual bool NeedsContainer
		{
			get
			{
				return VirtualView?.Clip != null || VirtualView?.Shadow != null || (VirtualView as IBorder)?.Border != null;
			}
		}
	}
}