﻿using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.ControlGallery.iOS;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform;
using ObjCRuntime;
using UIKit;
using static Microsoft.Maui.Controls.Compatibility.ControlGallery.Issues.Issue6368;

[assembly: ExportRenderer(typeof(CustomView), typeof(CustomRenderer))]
namespace Microsoft.Maui.Controls.Compatibility.ControlGallery.iOS
{
	public class CustomRenderer : ViewRenderer<CustomView, UIView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<CustomView> e)
		{
			base.OnElementChanged(e);

			//
			// --- Important --- //
			//
			// This is a WRONG Pattern!
			//Pattern taken from: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/view
			if (this.Control == null)
			{
				// Instantiate the native control and assign it to the Control property with
				// the SetPlatformControl method
				UIView myView = new UIView();
				this.SetPlatformControl(myView);
			}

			if (e.OldElement != null)
			{
				// Unsubscribe from event handlers and cleanup any resources
			}

			if (e.NewElement != null)
			{
				// Configure the control and subscribe to event handlers
			}
		}
	}
}
