﻿#if IOS
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace UITests
{
	public class Issue2842 : IssuesUITest
	{
		public Issue2842(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "ViewCell in TableView not adapting to changed size on iOS";

		[Test]
		[Category(UITestCategories.TabbedPage)]
		[FailsOnIOS]
		public void Issue2842Test()
		{
			RunningApp.WaitForElement("btnClick");
			RunningApp.Tap("btnClick");
			RunningApp.Screenshot("Verify that the text is not on top of the image");
		}
	}
}
#endif