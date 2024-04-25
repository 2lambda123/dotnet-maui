﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.AppiumTests.Issues
{
    public class Issue21726 : _IssuesUITest
    {
        public Issue21726(TestDevice device) : base(device)
        {
        }

        public override string Issue => "Modal with a bottom sheet should not crash iOS Keyboard Scroll";

        [Test]
        public void PushViewControllerWithNullWindow()
        {
            try
            {
                App.WaitForElement("AddVC");
                App.Click("AddVC");
                App.WaitForElement("TextField1").Click();
                App.WaitForElement("Button1").Click();
                var mainPageElement = App.WaitForElement("AddVC");
                Assert.NotNull(mainPageElement);
            }
            catch
            {
                // Just in case these tests leave the app in an unreliable state
                App.ResetApp();
                FixtureSetup();
                throw;
            }
        }
    }
}
