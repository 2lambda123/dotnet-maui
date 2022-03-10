﻿using Microsoft.Maui.Graphics;
/*
 See the IFoldableContext.android.cs for implementation
 */
namespace Microsoft.Maui.Controls.DualScreen
{
	public delegate void FoldingFeatureChangedHandler(object sender, System.EventArgs ea);

	public interface IFoldableContext
	{
		bool isSeparating { get; }
		Rect FoldingFeatureBounds { get; }
		Rect WindowBounds { get; }
		float ScreenDensity { get; }
		event System.EventHandler<FoldEventArgs> FoldingFeatureChanged;
	}
}
