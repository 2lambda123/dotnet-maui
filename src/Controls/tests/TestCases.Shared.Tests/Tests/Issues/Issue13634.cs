﻿#if !MACCATALYST
using NUnit.Framework;
using NUnit.Framework.Legacy;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue13634 : _IssuesUITest
	{
		public override string Issue => "Scrolling of Editor placed in ScollView does not work";
		public Issue13634(TestDevice testDevice) : base(testDevice)
		{
		}

		[Test]
		[Category(UITestCategories.ScrollView)]
		public void ScrollEditor()
		{
			App.WaitForElement("EditorControl");
			App.ScrollDown("EditorControl", ScrollStrategy.Auto);
			var result = App.WaitForElement("EditorControl");
			ClassicAssert.AreNotEqual(result.GetRect().Y, 0);
			VerifyScreenshot();
		}
	}
}
#endif
