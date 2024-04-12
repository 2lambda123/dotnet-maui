﻿using NUnit.Framework;
using UITest.Appium;

namespace UITests
{
	public class Issue2983 : IssuesUITest
	{
		public Issue2983(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "ListView.Footer can cause NullReferenceException";

		[Test]
		[Category(UITestCategories.ListView)]
		[FailsOnIOS]
		public void TestDoesNotCrash()
		{
			RunningApp.WaitForElement("footer");
		}
	}
}