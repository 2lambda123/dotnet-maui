﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace UITests
{
	public class Bugzilla36802 : IssuesUITest
	{
		public Bugzilla36802(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "[iOS] AccessoryView Partially Hidden When Using RecycleElement and GroupShortName";

		[Test]
		[Category(UITestCategories.ListView)]
		[FailsOnIOS]
		public void Bugzilla36802Test()
		{
			RunningApp.WaitForElement("TestReady");
			RunningApp.Screenshot("AccessoryView partially hidden test");
		}
	}
}