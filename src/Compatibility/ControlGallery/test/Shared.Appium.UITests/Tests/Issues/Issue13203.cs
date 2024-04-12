﻿using NUnit.Framework;
using UITest.Appium;

namespace UITests
{
	public class Issue13203 : IssuesUITest
	{
		const string Success = "Success";

		public Issue13203(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "[Bug] [iOS] CollectionView does not bind to items if `IsVisible=False`";

		[Test]
		[Category(UITestCategories.CollectionView)]
		[FailsOnIOS]
		public void CollectionShouldInvalidateOnVisibilityChange()
		{
			RunningApp.WaitForElement(Success);
		}
	}
}