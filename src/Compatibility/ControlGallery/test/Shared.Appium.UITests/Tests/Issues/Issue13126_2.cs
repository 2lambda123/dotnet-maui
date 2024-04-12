﻿using NUnit.Framework;
using UITest.Appium;

namespace UITests
{
	public class Issue13126_2 : IssuesUITest
	{
		const string Success = "Success";

		public Issue13126_2(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "[Bug] Regression: 5.0.0-pre5 often fails to draw dynamically loaded collection view content";

		[Test]
		[Category(UITestCategories.CollectionView)]
		[FailsOnIOS]
		public void CollectionViewShouldSourceShouldResetWhileInvisible()
		{
			RunningApp.WaitForNoElement(Success);
		}
	}
}