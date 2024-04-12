﻿using NUnit.Framework;
using UITest.Appium;

namespace UITests
{
	public class Bugzilla23942 : IssuesUITest
	{
		public Bugzilla23942(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "Cannot bind properties in BindableObjects added to static resources in XAML";

		[Test]
		[Category(UITestCategories.LifeCycle)]	
		[FailsOnIOS]
		public void Bugzilla23942Test()
		{
			RunningApp.WaitForNoElement("success");
		}
	}
}