﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace UITests
{
	public class Issue2259 : IssuesUITest
	{
		public Issue2259(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "ListView.ScrollTo crashes app";

		[Test]
		[Category(UITestCategories.ListView)]
		[FailsOnIOS]
		public void Issue2259Tests()
		{
			this.IgnoreIfPlatforms([TestDevice.Android, TestDevice.Mac]);

			for (int i = 0; i < 20; i++)
			{
				RunningApp.Tap("AddButton");
				RunningApp.WaitForElement("Name " + (i + 1).ToString());
				RunningApp.Screenshot("Added Cell");
			}
		}
	}
}