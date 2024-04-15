﻿using NUnit.Framework;
using UITest.Appium;

namespace UITests
{
	public class Issue2837 : IssuesUITest
	{
		const string Success = "worked";

		public Issue2837(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "Exception thrown during NavigationPage.Navigation.PopAsync";
		public override bool ResetMainPage => false;

		[Test]
		[Category(UITestCategories.Navigation)]
		public void Issue2837Test()
		{
			RunningApp.WaitForElement(Success);
		}
	}
}
