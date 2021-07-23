namespace Microsoft.Maui.Controls
{
	public partial class Button : IButton
	{
		void IButton.Clicked()
		{
			(this as IButtonController).SendClicked();
		}

		void IButton.Pressed()
		{
			(this as IButtonController).SendPressed();
		}

		void IButton.Released()
		{
			(this as IButtonController).SendReleased();
		}

		Font? _font;

		Font ITextStyle.Font => _font ??= Font.OfSize(FontFamily, FontSize, enableScaling: FontScalingEnabled).WithAttributes(FontAttributes);
	}
}