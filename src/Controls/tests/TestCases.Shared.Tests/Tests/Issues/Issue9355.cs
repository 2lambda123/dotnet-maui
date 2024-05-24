﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue9355 : _IssuesUITest
	{
		const string TestOk = "Test Ok";

		public Issue9355(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "ScrollViewRenderer renderer dispose crash";

		[Test]
		[Category(UITestCategories.ScrollView)]
		[Category(UITestCategories.Compatibility)]
		public void Issue9355Test()
		{
			this.IgnoreIfPlatforms([TestDevice.iOS, TestDevice.Mac, TestDevice.Windows]);

			App.WaitForNoElement(TestOk);
		}
	}
}
