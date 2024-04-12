﻿using NUnit.Framework;
using UITest.Appium;

namespace UITests
{
	public class Issue1851 : IssuesUITest
	{
		public Issue1851(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "ObservableCollection in ListView gets Index out of range when removing item";

		[Test]
		[Category(UITestCategories.ListView)]
		[FailsOnIOS]
		public void Issue1851Test()
		{
			this.IgnoreIfPlatforms([TestDevice.Mac, TestDevice.Windows]);

			RunningApp.WaitForElement("btn");
			RunningApp.Tap("btn");
			RunningApp.WaitForElement("btn");
			RunningApp.Tap("btn");
			RunningApp.WaitForElement("btn");
		}
	}
}
