﻿using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Microsoft.Maui.Controls
{
	public partial class DatePicker
	{
		public static void MapUpdateMode(IDatePickerHandler handler, DatePicker datePicker)
		{
			handler.UpdateImmediately = datePicker.OnThisPlatform().UpdateMode() == UpdateMode.Immediately;
		}

		public static void MapUpdateMode(DatePickerHandler handler, DatePicker datePicker) =>
			MapUpdateMode((IDatePickerHandler)handler, datePicker);
	}
}