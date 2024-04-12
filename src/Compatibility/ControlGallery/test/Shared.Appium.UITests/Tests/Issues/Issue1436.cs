﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace UITests
{
	public class Issue1436 : IssuesUITest
	{
		public Issue1436(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "Button border not drawn on Android without a BorderRadius"; 
		
		[Test]
		[Category(UITestCategories.Button)]
		[FailsOnIOS]
		public void Issue1436Test()
		{
			this.IgnoreIfPlatforms([TestDevice.Android, TestDevice.Mac, TestDevice.Windows]);

			RunningApp.WaitForElement("TestReady");
			RunningApp.Screenshot("I am at Issue 1436");
		}
	}
}
