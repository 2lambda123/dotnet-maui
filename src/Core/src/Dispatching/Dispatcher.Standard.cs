﻿using System;

namespace Microsoft.Maui.Dispatching
{
	public partial class Dispatcher : IDispatcher
	{
		static IDispatcher? GetForCurrentThreadImplementation() =>
			throw new NotImplementedException();
		Dispatcher()
		{
		}

		bool IsInvokeRequiredImplementation() =>
			throw new NotImplementedException();

		void BeginInvokeOnMainThreadImplementation(Action action) =>
			throw new NotImplementedException();
	}
}