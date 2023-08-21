﻿using System;
using System.Diagnostics.CodeAnalysis;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Microsoft.Maui.Platform
{

	public class MauiTextField : UITextField, IUIViewLifeCycleEvents
	{

		public MauiTextField(CGRect frame)
			: base(frame)
		{
		}

		public MauiTextField()
		{
		}

		[UnconditionalSuppressMessage(IUIViewLifeCycleEvents.UnconditionalSuppressMessage, "MA0002")]
		EventHandler? _movedToWindow;
		event EventHandler IUIViewLifeCycleEvents.MovedToWindow
		{
			add => _movedToWindow += value;
			remove => _movedToWindow -= value;
		}

		public override void WillMoveToWindow(UIWindow? window)
		{
			base.WillMoveToWindow(window);
			_movedToWindow?.Invoke(this, EventArgs.Empty);
		}

		public override string? Text
		{
			get => base.Text;
			set
			{
				var old = base.Text;

				base.Text = value;

				if (old != value)
					TextPropertySet?.Invoke(this, EventArgs.Empty);
			}
		}

		public override NSAttributedString? AttributedText
		{
			get => base.AttributedText;
			set
			{
				var old = base.AttributedText;

				base.AttributedText = value;

				if (old?.Value != value?.Value)
					TextPropertySet?.Invoke(this, EventArgs.Empty);
			}
		}

		public override UITextRange? SelectedTextRange
		{
			get => base.SelectedTextRange;
			set
			{
				var old = base.SelectedTextRange;

				base.SelectedTextRange = value;

				if (old?.Start != value?.Start || old?.End != value?.End)
					SelectionChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public override void MovedToWindow()
		{
			base.MovedToWindow();
			_movedToWindow?.Invoke(this, EventArgs.Empty);
		}

		[UnconditionalSuppressMessage("Memory", "MA0001", Justification = "Proven safe in test: MemoryTests.HandlerDoesNotLeak")]
		public event EventHandler? TextPropertySet;
		[UnconditionalSuppressMessage("Memory", "MA0001", Justification = "Proven safe in test: MemoryTests.HandlerDoesNotLeak")]
		internal event EventHandler? SelectionChanged;
	}
}