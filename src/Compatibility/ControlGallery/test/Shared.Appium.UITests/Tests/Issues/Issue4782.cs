﻿#if ANDROID
using NUnit.Framework;
using UITest.Appium;

namespace UITests
{
	public class Issue4782 : IssuesUITest
	{
		const string Success = "Success";

		public Issue4782(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "[Android] Null drawable crashes Image Button";

		[Test]
		[Category(UITestCategories.ImageButton)]
		public void ImageButtonNullDrawable()
		{
			RunningApp.WaitForElement(Success);
		}
	}
}
#endif