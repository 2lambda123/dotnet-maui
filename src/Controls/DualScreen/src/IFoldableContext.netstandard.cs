﻿using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.DualScreen
{
	public delegate void FoldingFeatureChangedHandler(object sender, System.EventArgs ea);

	public interface IFoldableContext
	{
		bool isSeparating { get; }
		Rectangle FoldingFeatureBounds { get; }
		Rectangle WindowBounds { get; }
		event System.EventHandler<FoldEventArgs> FoldingFeatureChanged;
	}
}
