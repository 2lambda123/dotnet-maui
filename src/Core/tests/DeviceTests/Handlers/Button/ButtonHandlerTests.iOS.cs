using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;
using UIKit;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ButtonHandlerTests
	{
		[Theory(DisplayName = "Font Family Initializes Correctly")]
		[InlineData(null)]
		[InlineData("Times New Roman")]
		[InlineData("Dokdo")]
		public async Task FontFamilyInitializesCorrectly(string family)
		{
			var button = new ButtonStub
			{
				Text = "Test",
				Font = Font.OfSize(family, 10)
			};

			var nativeFont = await GetValueAsync(button, handler => GetNativeButton(handler).Font);

			var fontManager = App.Services.GetRequiredService<IFontManager>();

			var expectedNativeFont = fontManager.GetFont(Font.OfSize(family, 0.0));

			Assert.Equal(expectedNativeFont.FamilyName, nativeFont.FamilyName);
			if (string.IsNullOrEmpty(family))
				Assert.Equal(fontManager.DefaultFont.FamilyName, nativeFont.FamilyName);
			else
				Assert.NotEqual(fontManager.DefaultFont.FamilyName, nativeFont.FamilyName);
		}

		UIButton GetNativeButton(ButtonHandler buttonHandler) =>
			(UIButton)buttonHandler.View;

		string GetNativeText(ButtonHandler buttonHandler) =>
			GetNativeButton(buttonHandler).CurrentTitle;

		Color GetNativeTextColor(ButtonHandler buttonHandler) =>
			GetNativeButton(buttonHandler).CurrentTitleColor.ToColor();

		Task PerformClick(IButton button)
		{
			return InvokeOnMainThreadAsync(() =>
			{
				GetNativeButton(CreateHandler(button)).SendActionForControlEvents(UIControlEvent.TouchUpInside);
			});
		}

		double GetNativeUnscaledFontSize(ButtonHandler buttonHandler) =>
			GetNativeButton(buttonHandler).TitleLabel.Font.PointSize;

		bool GetNativeIsBold(ButtonHandler buttonHandler) =>
			GetNativeButton(buttonHandler).TitleLabel.Font.FontDescriptor.SymbolicTraits.HasFlag(UIFontDescriptorSymbolicTraits.Bold);

		bool GetNativeIsItalic(ButtonHandler buttonHandler) =>
			GetNativeButton(buttonHandler).TitleLabel.Font.FontDescriptor.SymbolicTraits.HasFlag(UIFontDescriptorSymbolicTraits.Italic);
	}
}