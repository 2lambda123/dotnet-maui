﻿#nullable disable

namespace Microsoft.Maui.Controls
{
	// There are 2 Layout types: Controls and Compatibility
	interface IInputTransparentAffectingElement
	{
		bool CascadeInputTransparent { get; }
	}
}