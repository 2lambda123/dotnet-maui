using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.AppiumTests.Issues
{
	public class Issue21717 : _IssuesUITest
	{
		public override string Issue => "[Android] Entry & Picker VerticalTextAlignment ignored";

		public Issue21717(TestDevice device) : base(device) { }

		[Test]
		public void VerticalTextAlignmentShouldWork()
		{
			this.IgnoreIfPlatforms(new[] { TestDevice.Mac, TestDevice.iOS, TestDevice.Windows });

			VerifyScreenshot();
		}
	}
}
