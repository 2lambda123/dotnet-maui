#nullable disable
using System;
using System.ComponentModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls
{
	/// <include file="../../docs/Microsoft.Maui.Controls/ContentPresenter.xml" path="Type[@FullName='Microsoft.Maui.Controls.ContentPresenter']/Docs/*" />
	public class ContentPresenter : Compatibility.Layout, IContentView
	{
		/// <include file="../../docs/Microsoft.Maui.Controls/ContentPresenter.xml" path="//Member[@MemberName='ContentProperty']/Docs/*" />
		public static BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View),
			typeof(ContentPresenter), null, propertyChanged: OnContentChanged);

		/// <include file="../../docs/Microsoft.Maui.Controls/ContentPresenter.xml" path="//Member[@MemberName='.ctor']/Docs/*" />
		public ContentPresenter()
		{
			SetBinding(
				ContentProperty,
				TypedBinding.ForSingleNestingLevel(
					nameof(IContentView.Content),
					static (IContentView view) => view.Content,
					static (view, val) =>
					{
						if (view is RadioButton radioButton)
						{
							radioButton.Content = val;
						}
						else if (view is ContentPresenter cotnentPresenter)
						{
							cotnentPresenter.Content = val as View;
						}
						else if (view is ContentPage contentPage)
						{
							contentPage.Content = val as View;
						}
						else if (view is ContentView contentView)
						{
							contentView.Content = val as View;
						}
						else if (view is ScrollView scrollView)
						{
							scrollView.Content = val as View;
						}
						else if (view is Border border)
						{
							border.Content = val as View;
						}
					},
					source: RelativeBindingSource.TemplatedParent,
					converter: new ContentConverter(),
					converterParameter: this));
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/ContentPresenter.xml" path="//Member[@MemberName='Content']/Docs/*" />
		public View Content
		{
			get { return (View)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		object IContentView.Content => Content;
		IView IContentView.PresentedContent => Content;

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			for (var i = 0; i < LogicalChildrenInternal.Count; i++)
			{
				Element element = LogicalChildrenInternal[i];
				var child = element as View;
				if (child != null)
					LayoutChildIntoBoundingRegion(child, new Rect(x, y, width, height));
			}
		}

		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			double widthRequest = WidthRequest;
			double heightRequest = HeightRequest;
			var childRequest = new SizeRequest();
			if ((widthRequest == -1 || heightRequest == -1) && Content != null)
			{
				childRequest = Content.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
			}

			return new SizeRequest
			{
				Request = new Size { Width = widthRequest != -1 ? widthRequest : childRequest.Request.Width, Height = heightRequest != -1 ? heightRequest : childRequest.Request.Height },
				Minimum = childRequest.Minimum
			};
		}

		internal virtual void Clear()
		{
			Content = null;
		}

		internal override void ComputeConstraintForView(View view)
		{
			bool isFixedHorizontally = (Constraint & LayoutConstraint.HorizontallyFixed) != 0;
			bool isFixedVertically = (Constraint & LayoutConstraint.VerticallyFixed) != 0;

			var result = LayoutConstraint.None;
			if (isFixedVertically && view.VerticalOptions.Alignment == LayoutAlignment.Fill)
				result |= LayoutConstraint.VerticallyFixed;
			if (isFixedHorizontally && view.HorizontalOptions.Alignment == LayoutAlignment.Fill)
				result |= LayoutConstraint.HorizontallyFixed;
			view.ComputedConstraint = result;
		}

		internal override void SetChildInheritedBindingContext(Element child, object context)
		{
			// We never want to use the standard inheritance mechanism, we will get this set by our parent
		}

		static async void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var self = (ContentPresenter)bindable;

			var oldView = (View)oldValue;
			var newView = (View)newValue;
			if (oldView != null)
			{
				self.InternalChildren.Remove(oldView);
				oldView.ParentOverride = null;
			}

			if (newView != null)
			{
				self.InternalChildren.Add(newView);
				newView.ParentOverride = await TemplateUtilities.FindTemplatedParentAsync((Element)bindable);
			}
		}

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			return this.ComputeDesiredSize(widthConstraint, heightConstraint);
		}

		Size ICrossPlatformLayout.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
		{
			return this.MeasureContent(widthConstraint, heightConstraint);
		}

		protected override Size ArrangeOverride(Rect bounds)
		{
			Frame = this.ComputeFrame(bounds);
			Handler?.PlatformArrange(Frame);
			return Frame.Size;
		}

		Size ICrossPlatformLayout.CrossPlatformArrange(Rect bounds)
		{
			this.ArrangeContent(bounds);
			return bounds.Size;
		}
	}
}