﻿using System;

namespace Microsoft.Maui.Handlers
{
	public partial class EditorHandler : AbstractViewHandler<IEditor, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();

		public static void MapText(IViewHandler handler, IEditor editor) { }

		public static void MapCharacterSpacing(IViewHandler handler, IEditor editor) { }
		
		public static void MapPredictiveText(EditorHandler handler, IEditor editor) { }
	}
}