using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.AppiumTests.Issues
{
	public class Issue21240 : _IssuesUITest
	{
		public override string Issue => "FlyoutPage IsGestureEnabled not working";

		public Issue21240(TestDevice device) : base(device)
		{
		}

		[Test]
		public void FlyoutShouldNotBePresented()
		{
			this.IgnoreIfPlatforms(new TestDevice[] { TestDevice.Mac, TestDevice.Windows });

			App.WaitForElement("label");
			App.SwipeLeftToRight(1, 500);

			// The test passes if a flyout is not present
			VerifyScreenshot();
		}
	}
}
