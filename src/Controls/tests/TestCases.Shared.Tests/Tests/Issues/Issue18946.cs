using NUnit.Framework;
using NUnit.Framework.Legacy;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue18946 : _IssuesUITest
	{
		public override string Issue => "Shell Toolbar items not displayed";

		public Issue18946(TestDevice device) : base(device)
		{ }

		[Test]
		public void ToolbarItemsShouldBeVisible()
		{
			var label = App.WaitForElement("label");
			_ = App.WaitForElement("image");

			ClassicAssert.True(label.GetText() == "Hello");
		}
	}
}
