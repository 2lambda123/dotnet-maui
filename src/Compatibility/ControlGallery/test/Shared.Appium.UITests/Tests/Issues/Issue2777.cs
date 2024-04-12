﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace UITests
{
	public class Issue2777 : IssuesUITest
	{
		public Issue2777(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "When add GroupHeaderTemplate in XAML the group header does not show up";

		[Test]
		[Category(UITestCategories.CollectionView)]
		[FailsOnIOS]
		public void Issue2777Test()
		{
			RunningApp.Screenshot("I am at Issue 2965");
			RunningApp.WaitForNoElement("The letter A");
		}
	}
}