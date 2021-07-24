﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Microsoft.Maui.Controls
{
	public partial class NavigationPage : INavigationView
	{
		Thickness IView.Margin => Thickness.Zero;

		partial void Init()
		{
			PushRequested += (_, args) =>
			{
				var request = new MauiNavigationRequestedEventArgs(args.Page, args.BeforePage, args.Animated);
				Handler?.Invoke(nameof(INavigationView.PushAsync), request);

			};

			PopRequested += (_, args) =>
			{
				var request = new MauiNavigationRequestedEventArgs(args.Page, args.BeforePage, args.Animated);
				Handler?.Invoke(nameof(INavigationView.PopAsync), request);
			};
		}

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (Content is IFrameworkElement frameworkElement)
			{
				frameworkElement.Measure(widthConstraint, heightConstraint);
			}

			return new Size(widthConstraint, heightConstraint);
		}

		protected override Size ArrangeOverride(Rectangle bounds)
		{
			// Update the Bounds (Frame) for this page
			Layout(bounds);

			if (Content is IFrameworkElement element and VisualElement visualElement)
			{
				visualElement.Frame = element.ComputeFrame(bounds);
				element.Handler?.NativeArrange(visualElement.Frame);
			}

			return Frame.Size;
		}

		void INavigationView.InsertPageBefore(IView page, IView before)
		{
			throw new NotImplementedException();
		}

		Task<IView> INavigationView.PopAsync() =>
			(this as INavigationView).PopAsync(true);

		async Task<IView> INavigationView.PopAsync(bool animated)
		{
			var thing = await this.PopAsync(animated);
			return thing;
		}

		Task<IView> INavigationView.PopModalAsync()
		{
			throw new NotImplementedException();
		}

		Task<IView> INavigationView.PopModalAsync(bool animated)
		{
			throw new NotImplementedException();
		}

		Task INavigationView.PushAsync(IView page) =>
			(this as INavigationView).PushAsync(page, true);

		Task INavigationView.PushAsync(IView page, bool animated)
		{
			return this.PushAsync((Page)page, animated);
		}

		Task INavigationView.PushModalAsync(IView page)
		{
			throw new NotImplementedException();
		}

		Task INavigationView.PushModalAsync(IView page, bool animated)
		{
			throw new NotImplementedException();
		}

		void INavigationView.RemovePage(IView page)
		{
			throw new NotImplementedException();
		}

		IFrameworkElement Content =>
			this.CurrentPage;

		IReadOnlyList<IView> INavigationView.ModalStack => throw new NotImplementedException();

		IReadOnlyList<IView> INavigationView.NavigationStack => throw new NotImplementedException();
	}

}
