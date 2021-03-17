﻿using System;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Graphics.Drawable;
using static Android.Views.View;

namespace Microsoft.Maui
{
	public class MauiDatePicker : EditText, IOnClickListener
	{
		public MauiDatePicker(Context? context) : base(context)
		{
			Initialize();
		}

		public MauiDatePicker(Context? context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();
		}

		public MauiDatePicker(Context? context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Initialize();
		}

		protected MauiDatePicker(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		private void Initialize()
		{
			DrawableCompat.Wrap(Background);

			Focusable = true;
			Clickable = true;
			InputType = InputTypes.Null;

			SetOnClickListener(this);
		}

		public Action? ShowPicker { get; set; }
		public Action? HidePicker { get; set; }

		public void OnClick(View? v)
		{
			ShowPicker?.Invoke();
		}
	}
}