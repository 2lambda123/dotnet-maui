﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Appium;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using NUnit.Framework;

namespace Microsoft.Maui.AppiumTests.Issues
{
	public class SoftInputExtensionsPageTests : _IssuesUITest
	{
		public SoftInputExtensionsPageTests(TestDevice device) : base(device) { }

		public override string Issue => "Soft Input Extension Methods";

		[Test]
		public void SoftInputExtensionsPageTest()
		{
			try
			{
				App.Tap("ShowKeyboard");

				Assert.IsTrue(App.WaitForTextToBePresentInElement("Result", "True"));

				App.Tap("HideKeyboard");

				Assert.IsTrue(App.WaitForTextToBePresentInElement("Result", "False"));
			}
			finally
			{
				App.NavigateBack();
			}
		}
	}
}
