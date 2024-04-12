﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace UITests
{
	public class Issue8766 : IssuesUITest
	{
		public Issue8766(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "[Bug] CollectionView.EmptyView does not inherit parent Visual";

		[Test]
		[Category(UITestCategories.CollectionView)]
		public void VisualPropagatesToEmptyView()
		{
			RunningApp.WaitForElement("TestReady");
			RunningApp.Screenshot("CollectionViewWithEmptyView");
		}
	}
}
