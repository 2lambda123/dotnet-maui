#nullable enable
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui
{
	public static class ViewExtensions
	{
		public static void UpdateIsEnabled(this FrameworkElement nativeView, IView view) =>
			(nativeView as Control)?.UpdateIsEnabled(view.IsEnabled);

		public static void UpdateBackground(this FrameworkElement nativeView, IView view)
		{
			if (nativeView is Control control)
				control.UpdateBackground(view.Background);
			else if (nativeView is Border border)
				border.UpdateBackground(view.Background);
			else if (nativeView is Panel panel)
				panel.UpdateBackground(view.Background);
		}

		public static void UpdateAutomationId(this FrameworkElement nativeView, IView view) =>
			AutomationProperties.SetAutomationId(nativeView, view.AutomationId);

		public static void UpdateSemantics(this FrameworkElement nativeView, IView view)
		{
			var semantics = view.Semantics;
			if (semantics == null)
				return;

			AutomationProperties.SetName(nativeView, semantics.Description);
			AutomationProperties.SetHelpText(nativeView, semantics.Hint);
			AutomationProperties.SetHeadingLevel(nativeView, (UI.Xaml.Automation.Peers.AutomationHeadingLevel)((int)semantics.HeadingLevel));
		}

		internal static void UpdateProperty(this FrameworkElement nativeControl, DependencyProperty property, Color color)
		{
			if (color.IsDefault())
				nativeControl.ClearValue(property);
			else
				nativeControl.SetValue(property, color.ToNative());
		}

		internal static void UpdateProperty(this FrameworkElement nativeControl, DependencyProperty property, object? value)
		{
			if (value == null)
				nativeControl.ClearValue(property);
			else
				nativeControl.SetValue(property, value);
		}
	}
}
