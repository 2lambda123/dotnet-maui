﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Microsoft.Maui.MauiBlazorWebView.DeviceTests
{
	public partial class HandlerTestBase
	{
		protected bool GetIsAccessibilityElement(IViewHandler viewHandler)
		{
			var nativeView = ((UIView)viewHandler.NativeView);
			return nativeView.IsAccessibilityElement;
		}

		protected bool GetExcludedWithChildren(IViewHandler viewHandler)
		{
			var nativeView = ((UIView)viewHandler.NativeView);
			return nativeView.AccessibilityElementsHidden;
		}
	}
}
