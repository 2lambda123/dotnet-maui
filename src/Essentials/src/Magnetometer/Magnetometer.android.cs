using Android.Hardware;
using Android.Runtime;

namespace Microsoft.Maui.Essentials.Implementations
{
	public partial class MagnetometerImplementation : IMagnetometer
	{
		public bool IsSupported =>
			   Platform.SensorManager?.GetDefaultSensor(SensorType.MagneticField) != null;

		static MagnetometerListener listener;
		static Sensor magnetometer;

		public void Start(SensorSpeed sensorSpeed)
		{
			var delay = sensorSpeed.ToPlatform();

			listener = new MagnetometerListener();
			magnetometer = Platform.SensorManager.GetDefaultSensor(SensorType.MagneticField);
			Platform.SensorManager.RegisterListener(listener, magnetometer, delay);
		}

		public void Stop()
		{
			if (listener == null || magnetometer == null)
				return;

			Platform.SensorManager.UnregisterListener(listener, magnetometer);
			listener.Dispose();
			listener = null;
		}
	}

	class MagnetometerListener : Java.Lang.Object, ISensorEventListener
	{
		internal MagnetometerListener()
		{
		}

		void ISensorEventListener.OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
		{
		}

		void ISensorEventListener.OnSensorChanged(SensorEvent e)
		{
			if ((e?.Values?.Count ?? 0) < 3)
				return;

			var data = new MagnetometerData(e.Values[0], e.Values[1], e.Values[2]);
			Magnetometer.OnChanged(data);
		}
	}
}
