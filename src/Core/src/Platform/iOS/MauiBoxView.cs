﻿using System.Diagnostics.CodeAnalysis;
using System;
using Microsoft.Maui.Graphics.Platform;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Platform
{
	public class MauiBoxView : PlatformGraphicsView, IUIViewLifeCycleEvents
	{
		public MauiBoxView()
		{
			BackgroundColor = UIColor.Clear;
		}

		[UnconditionalSuppressMessage(IUIViewLifeCycleEvents.UnconditionalSuppressMessage, "MA0002")]
		EventHandler? _movedToWindow;
		event EventHandler IUIViewLifeCycleEvents.MovedToWindow
		{
			add => _movedToWindow += value;
			remove => _movedToWindow -= value;
		}

		public override void MovedToWindow()
		{
			base.MovedToWindow();
			_movedToWindow?.Invoke(this, EventArgs.Empty);
		}
	}
}