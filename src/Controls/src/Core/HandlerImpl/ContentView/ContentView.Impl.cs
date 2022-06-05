﻿namespace Microsoft.Maui.Controls
{
	public partial class ContentView : IContentView
	{
		object IContentView.Content => Content;

		IView IContentView.PresentedContent => ((this as IControlTemplated).TemplateRoot as IView) ?? Content;

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Handler?.UpdateValue(nameof(Content));
		}
	}
}
