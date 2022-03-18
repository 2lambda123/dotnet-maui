namespace Microsoft.Maui.Devices.Sensors.Implementations
{
	/// <include file="../../docs/Microsoft.Maui.Essentials/OrientationSensor.xml" path="Type[@FullName='Microsoft.Maui.Essentials.OrientationSensor']/Docs" />
	public partial class OrientationSensorImplementation : IOrientationSensor
	{
		bool PlatformIsSupported =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		void PlatformStart(SensorSpeed sensorSpeed) =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		void PlatformStop() =>
			throw ExceptionUtils.NotSupportedOrImplementedException;
	}
}
