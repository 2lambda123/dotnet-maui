﻿using System;
using Microsoft.UI.Dispatching;

namespace Microsoft.Maui.Dispatching
{
	public partial class Dispatcher : IDispatcher
	{
		readonly DispatcherQueue _dispatcherQueue;

		internal Dispatcher(DispatcherQueue dispatcherQueue)
		{
			_dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));
		}

		bool IsDispatchRequiredImplementation() =>
			!_dispatcherQueue.HasThreadAccess;

		bool DispatchImplementation(Action action) =>
			_dispatcherQueue.TryEnqueue(() => action());

		bool DispatchDelayedImplementation(TimeSpan delay, Action action)
		{
			var timer = _dispatcherQueue.CreateTimer();
			timer.Interval = delay;
			timer.Tick += OnTimerTick;
			timer.Start();
			return true;

			void OnTimerTick(DispatcherQueueTimer sender, object args)
			{
				action();
				timer.Tick -= OnTimerTick;
			}
		}

		IDispatcherTimer CreateTimerImplementation()
		{
			return new DispatcherTimer(_dispatcherQueue);
		}
	}

	partial class DispatcherTimer : IDispatcherTimer
	{
		readonly DispatcherQueue _dispatcherQueue;
		DispatcherQueueTimer _timer;
		bool _started;

		public DispatcherTimer(DispatcherQueue queue)
		{
			_dispatcherQueue = queue;
			_timer = _dispatcherQueue.CreateTimer();
		}

		public TimeSpan Interval
		{
			get => _timer.Interval;
			set => _timer.Interval = value;
		}

		public bool IsRepeating
		{
			get => _timer.IsRepeating;
			set => _timer.IsRepeating = value;
		}

		public bool IsRunning => _timer.IsRunning;

		public event EventHandler? Tick;

		public void Start()
		{
			if (IsRunning)
				return;

			if (_started)
			{
				// if we Start a timer that has already finished, it will immediately fire.
				// to ensure that Start restarts the timer, we need to replace it.
				var newTimer = _dispatcherQueue.CreateTimer();
				newTimer.Interval = Interval;
				newTimer.IsRepeating = IsRepeating;
				_timer = newTimer;
			}
			else
			{
				_started = true;
			}

			_timer.Tick += OnTimerTick;

			_timer.Start();
		}

		public void Stop()
		{
			if (!IsRunning)
				return;

			_timer.Tick -= OnTimerTick;

			_timer.Stop();
		}

		void OnTimerTick(DispatcherQueueTimer sender, object args) =>
			Tick?.Invoke(this, EventArgs.Empty);
	}

	public partial class DispatcherProvider
	{
		static IDispatcher? GetForCurrentThreadImplementation()
		{
			var q = DispatcherQueue.GetForCurrentThread();
			if (q == null)
				return null;

			return new Dispatcher(q);
		}
	}
}