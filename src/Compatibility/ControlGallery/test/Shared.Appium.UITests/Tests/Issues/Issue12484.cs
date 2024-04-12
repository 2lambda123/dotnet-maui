﻿#if ANDROID
using NUnit.Framework;
using UITest.Appium;

namespace UITests
{
	public class Issue12484 : IssuesUITest
	{
		public Issue12484(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "Unable to set ControlTemplate for TemplatedView in Xamarin.Forms version 5.0";

		[Test]
		[Category(UITestCategories.ViewBaseTests)]
		[FailsOnAndroid]
		public void Issue12484ControlTemplateRendererTest()
		{ 
			RunningApp.WaitForNoElement("Success");
		}
	}
}
#endif