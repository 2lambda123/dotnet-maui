using System;

namespace Microsoft.Maui.Platform
{

	public static partial class WindowExtensions
	{

		public static void UpdateTitle(this Gtk.Window nativeWindow, IWindow window) =>
			nativeWindow.Title = window.Title;

		public static IWindow GetWindow(this Gtk.Window nativeWindow)
		{
			foreach (var window in MauiGtkApplication.Current.Application.Windows)
			{
				if (window?.Handler?.PlatformView is Gtk.Window win && win == nativeWindow)
					return window;
			}

			throw new InvalidOperationException("Window Not Found");
		}

		public static void SetWindow(this Gtk.Window platformWindow, IWindow window, IMauiContext context)
		{
			_ = platformWindow ?? throw new ArgumentNullException(nameof(platformWindow));
			_ = window ?? throw new ArgumentNullException(nameof(window));
			_ = context ?? throw new ArgumentNullException(nameof(context));

			var handler = window.Handler;

			if (handler == null)
				handler = context.Handlers.GetHandler(window.GetType());

			if (handler == null)
				throw new Exception($"Handler not found for window {window}.");

			handler.SetMauiContext(context);

			window.Handler = handler;

			if (handler.VirtualView != window)
				handler.SetVirtualView(window);
		}

	}

}