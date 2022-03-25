#nullable enable
using System.Threading.Tasks;

namespace Microsoft.Maui.Devices
{
	public interface IFlashlight
	{
		Task TurnOnAsync();

		Task TurnOffAsync();
	}

	/// <include file="../../docs/Microsoft.Maui.Essentials/Flashlight.xml" path="Type[@FullName='Microsoft.Maui.Essentials.Flashlight']/Docs" />
	public static partial class Flashlight
	{
		/// <include file="../../docs/Microsoft.Maui.Essentials/Flashlight.xml" path="//Member[@MemberName='TurnOnAsync']/Docs" />
		public static Task TurnOnAsync() =>
			Current.TurnOnAsync();

		/// <include file="../../docs/Microsoft.Maui.Essentials/Flashlight.xml" path="//Member[@MemberName='TurnOffAsync']/Docs" />
		public static Task TurnOffAsync() =>
			Current.TurnOffAsync();

		static IFlashlight Current => Devices.Flashlight.Current;

		static IFlashlight? currentImplementation;

		public static IFlashlight Current =>
			currentImplementation ??= new FlashlightImplementation();

		internal static void SetCurrent(IFlashlight? implementation) =>
			currentImplementation = implementation;
	}
}
