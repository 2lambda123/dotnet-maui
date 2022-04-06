﻿namespace Microsoft.Maui.Controls
{
	public partial class Entry
	{
		public static IPropertyMapper<IEntry, EntryHandler> ControlsEntryMapper =
			new PropertyMapper<Entry, EntryHandler>(EntryHandler.Mapper)
			{
#if WINDOWS
				[nameof(PlatformConfiguration.WindowsSpecific.InputView.DetectReadingOrderFromContentProperty.PropertyName)] = MapDetectReadingOrderFromContent,
#endif
				[nameof(Text)] = MapText,
				[nameof(TextTransform)] = MapText,
			};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.Entry legacy behaviors
			EntryHandler.Mapper = ControlsEntryMapper;
		}
	}
}
