﻿#nullable enable
using System;

namespace Microsoft.Maui.Handlers
{
	public partial class ScrollViewHandler
	{
		public static PropertyMapper<IScrollView, ScrollViewHandler> ScrollViewMapper = new(ViewMapper)
		{
			[nameof(IScrollView.Content)] = MapContent,
			[nameof(IScrollView.HorizontalScrollBarVisibility)] = MapHorizontalScrollBarVisibility,
			[nameof(IScrollView.VerticalScrollBarVisibility)] = MapVerticalScrollBarVisibility,
			[nameof(IScrollView.Orientation)] = MapOrientation,
#if __IOS__
			[nameof(IScrollView.ContentSize)] = MapContentSize
#endif
		};

		public static CommandMapper<IScrollView, ScrollViewHandler> ScrollViewCommandMapper = new(ViewCommandMapper)
		{
			[nameof(IScrollView.RequestScrollTo)] = MapRequestScrollTo
		};

		public ScrollViewHandler() : base(ScrollViewMapper, ScrollViewCommandMapper)
		{

		}

		public ScrollViewHandler(PropertyMapper? mapper = null) : base(mapper ?? ScrollViewMapper)
		{

		}
	}
}
