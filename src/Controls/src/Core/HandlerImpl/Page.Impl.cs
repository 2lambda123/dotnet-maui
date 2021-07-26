﻿using System;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public partial class Page : IPage
	{
		IView IPage.Content => null;

		// TODO ezhart super sus
		public Thickness Margin => Thickness.Zero;

		internal void SendNavigatedTo(NavigatedToEventArgs args)
		{
			NavigatedTo?.Invoke(this, args);
			OnNavigatedTo();
		}

		internal void SendNavigatingFrom(NavigatingFromEventArgs args)
		{
			NavigatingFrom?.Invoke(this, args);
			OnNavigatingFrom();
		}

		internal void SendNavigatedFrom(NavigatedFromEventArgs args)
		{
			NavigatedFrom?.Invoke(this, args);
			OnNavigatedFrom();
		}

		public event EventHandler<NavigatedToEventArgs> NavigatedTo;
		public event EventHandler<NavigatingFromEventArgs> NavigatingFrom;
		public event EventHandler<NavigatedFromEventArgs> NavigatedFrom;

		protected virtual void OnNavigatedTo() { }
		protected virtual void OnNavigatingFrom() { }
		protected virtual void OnNavigatedFrom() { }
	}

	public sealed class NavigatingFromEventArgs : EventArgs
	{

	}

	public sealed class NavigatedToEventArgs : EventArgs
	{
		internal NavigatedToEventArgs(Page previousPage)
		{
			PreviousPage = previousPage;
		}

		public Page PreviousPage { get; }
	}

	public sealed class NavigatedFromEventArgs : EventArgs
	{
		internal NavigatedFromEventArgs(Page destinationPage)
		{
			DestinationPage = destinationPage;
		}

		public Page DestinationPage { get; }
	}
}
