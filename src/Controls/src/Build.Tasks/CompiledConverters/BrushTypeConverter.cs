﻿using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Build.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Mono.Cecil.Cil;

namespace Microsoft.Maui.Controls.XamlC;

class BrushTypeConverter : ColorTypeConverter
{
	public override IEnumerable<Instruction> ConvertFromString(string value, ILContext context, BaseNode node)
	{
		var module = context.Body.Method.Module;

		if (!string.IsNullOrEmpty (value))
		{
			value = value.Trim();

			if (value.StartsWith("#", StringComparison.Ordinal))
			{
				foreach (var instruction in base.ConvertFromString(value, context, node))
					yield return instruction;

				yield return Instruction.Create(OpCodes.Newobj, module.ImportCtorReference(("Microsoft.Maui.Controls", "Microsoft.Maui.Controls", "SolidColorBrush"), parameterTypes: new[] {
						("Microsoft.Maui.Graphics", "Microsoft.Maui.Graphics", "Color")}));

				yield break;
			}

			var parts = value.Split('.');
			if (parts.Length == 1 || (parts.Length == 2 && parts[0] == "Brush"))
			{
				var brush = parts[parts.Length - 1];
				var propertyGetterReference = module.ImportPropertyGetterReference(("Microsoft.Maui.Controls", "Microsoft.Maui.Controls", "Brush"),
																   brush,
																   isStatic: true,
																   caseSensitive: false);

				if (propertyGetterReference != null)
				{
					yield return Instruction.Create(OpCodes.Call, propertyGetterReference);
					yield break;
				}
			}
		}
		throw new BuildException(BuildExceptionCode.Conversion, node, null, value, typeof(Brush));
	}
}
