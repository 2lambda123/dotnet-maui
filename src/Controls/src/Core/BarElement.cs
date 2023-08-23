#nullable disable
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	static class BarElement
	{
		/// <summary>Bindable property for <see cref="IBarElement.BarBackgroundColor"/>.</summary>
		public static readonly BindableProperty BarBackgroundColorProperty =
			BindableProperty.Create(nameof(IBarElement.BarBackgroundColor), typeof(Color), typeof(IBarElement), default(Color));

		/// <summary>Bindable property for <see cref="IBarElement.BarBackground"/>.</summary>
		public static readonly BindableProperty BarBackgroundProperty =
			BindableProperty.Create(nameof(IBarElement.BarBackground), typeof(Brush), typeof(IBarElement), default(Brush));

		/// <summary>Bindable property for <see cref="IBarElement.BarTextColor"/>.</summary>
		public static readonly BindableProperty BarTextColorProperty =
			BindableProperty.Create(nameof(IBarElement.BarTextColor), typeof(Color), typeof(IBarElement), default(Color));

		/// <summary>Bindable property for <see cref="IBarElement.BarIndicatorColor"/>.</summary>
		public static readonly BindableProperty BarIndicatorColorProperty =
			BindableProperty.Create(nameof(IBarElement.BarIndicatorColor), typeof(Color), typeof(IBarElement), default(Color));

		/// <summary>Bindable property for <see cref="IBarElement.BarTabMode"/>.</summary>
		public static readonly BindableProperty BarTabModeProperty =
			BindableProperty.Create(nameof(IBarElement.BarTabMode), typeof(BarTabMode), typeof(IBarElement), BarTabMode.Auto);
	}
}