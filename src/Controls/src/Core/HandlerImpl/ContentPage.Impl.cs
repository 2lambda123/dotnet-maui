﻿using Microsoft.Maui.Graphics;
using Microsoft.Maui.HotReload;
using Microsoft.Maui.Layouts;

namespace Microsoft.Maui.Controls
{
	public partial class ContentPage : IContentView, HotReload.IHotReloadableView
	{
		IView IContentView.Content => Content;

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (Content is IView view)
			{
				_ = view.Measure(widthConstraint, heightConstraint);
			}

			return new Size(widthConstraint, heightConstraint);
		}

		protected override Size ArrangeOverride(Rectangle bounds)
		{
			Frame = this.ComputeFrame(bounds);

			if (Content is IFrameworkElement frameworkElement)
			{
				_ = frameworkElement.Arrange(Frame);
			}

			return Frame.Size;
		}

		protected override void InvalidateMeasureOverride()
		{
			base.InvalidateMeasureOverride();
			if (Content is IView view)
			{
				view.InvalidateMeasure();
			}
		}

		#region HotReload

		IView IReplaceableView.ReplacedView => HotReload.MauiHotReloadHelper.GetReplacedView(this) ?? this;

		HotReload.IReloadHandler HotReload.IHotReloadableView.ReloadHandler { get; set; }

		void HotReload.IHotReloadableView.TransferState(IView newView)
		{
			//TODO: Let you hot reload the the ViewModel
			//TODO: Lets do a real state transfer
			if (newView is View v)
				v.BindingContext = BindingContext;
		}

		void HotReload.IHotReloadableView.Reload()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				this.CheckHandlers();
				var reloadHandler = ((IHotReloadableView)this).ReloadHandler;
				reloadHandler?.Reload();
				//TODO: if reload handler is null, Do a manual reload?
			});
		}
		#endregion
	}
}
