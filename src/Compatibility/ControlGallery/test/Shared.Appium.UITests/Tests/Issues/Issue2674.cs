﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace UITests
{
	public class Issue2674 : IssuesUITest
	{
		public Issue2674(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "Exception occurs when giving null values in picker itemsource collection"; 
		
		[Test]
		[Category(UITestCategories.Picker)]
		[FailsOnIOS]
		public void Issue2674Test()
		{
			RunningApp.Screenshot("I am at Issue2674");
			RunningApp.WaitForElement("picker");
		}
	}
}