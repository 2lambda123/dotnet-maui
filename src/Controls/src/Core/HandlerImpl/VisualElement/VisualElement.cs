﻿using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.Controls
{
	/// <include file="../../../../docs/Microsoft.Maui.Controls/VisualElement.xml" path="Type[@FullName='Microsoft.Maui.Controls.VisualElement']/Docs" />
	public partial class VisualElement
	{
		/// <include file="../../../../docs/Microsoft.Maui.Controls/VisualElement.xml" path="//Member[@MemberName='ControlsVisualElementMapper']/Docs" />
		public static IPropertyMapper<IView, IViewHandler> ControlsVisualElementMapper =
			new PropertyMapper<IView, IViewHandler>(Element.ControlsElementMapper)
			{
				[nameof(BackgroundColor)] = MapBackgroundColor,
				[nameof(Page.BackgroundImageSource)] = MapBackgroundImageSource,
			};

		internal static void RemapForControls()
		{
			ViewHandler.ViewMapper = ControlsVisualElementMapper;
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls/VisualElement.xml" path="//Member[@MemberName='MapBackgroundColor']/Docs" />
		public static void MapBackgroundColor(IViewHandler handler, IView view)
		{
			handler.UpdateValue(nameof(Background));
		}

		public static void MapBackgroundImageSource(IViewHandler handler, IView view)
		{
			handler.UpdateValue(nameof(Background));
		}
	}
}
