﻿using Foundation;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Accessibility.Implementations
{
	public partial class SemanticScreenReaderImplementation : ISemanticScreenReader
	{
		public void Announce(string text)
		{
			if (!UIAccessibility.IsVoiceOverRunning)
				return;

			UIAccessibility.PostNotification(UIAccessibilityPostNotification.Announcement, new NSString(text));
		}
	}
}
