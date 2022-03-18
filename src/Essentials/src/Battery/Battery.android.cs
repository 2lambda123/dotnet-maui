using System;
using Android.Content;
using Android.OS;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Essentials;

namespace Microsoft.Maui.Devices.Implementations
{
	public partial class BatteryImplementation : IBattery
	{
		BatteryBroadcastReceiver batteryReceiver;
		EnergySaverBroadcastReceiver powerReceiver;

		public void StartEnergySaverListeners()
		{
			powerReceiver = new EnergySaverBroadcastReceiver(Battery.OnEnergySaverChanged);
			Platform.AppContext.RegisterReceiver(powerReceiver, new IntentFilter(PowerManager.ActionPowerSaveModeChanged));
		}

		public void StopEnergySaverListeners()
		{
			try
			{
				Platform.AppContext.UnregisterReceiver(powerReceiver);
			}
			catch (Java.Lang.IllegalArgumentException)
			{
				System.Diagnostics.Debug.WriteLine("Energy saver receiver already unregistered. Disposing of it.");
			}
			powerReceiver.Dispose();
			powerReceiver = null;
		}

		public EnergySaverStatus EnergySaverStatus
		{
			get
			{
				var status = Platform.PowerManager?.IsPowerSaveMode ?? false;
				return status ? EnergySaverStatus.On : EnergySaverStatus.Off;
			}
		}

		public void StartBatteryListeners()
		{
			Permissions.EnsureDeclared<Permissions.Battery>();

			batteryReceiver = new BatteryBroadcastReceiver(Battery.OnBatteryInfoChanged);
			Platform.AppContext.RegisterReceiver(batteryReceiver, new IntentFilter(Intent.ActionBatteryChanged));
		}

		public void StopBatteryListeners()
		{
			try
			{
				Platform.AppContext.UnregisterReceiver(batteryReceiver);
			}
			catch (Java.Lang.IllegalArgumentException)
			{
				System.Diagnostics.Debug.WriteLine("Battery receiver already unregistered. Disposing of it.");
			}
			batteryReceiver.Dispose();
			batteryReceiver = null;
		}

		public double ChargeLevel
		{
			get
			{
				Permissions.EnsureDeclared<Permissions.Battery>();

				using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
				using (var battery = Platform.AppContext.RegisterReceiver(null, filter))
				{
					var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
					var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

					if (scale <= 0)
						return 1.0;

					return (double)level / (double)scale;
				}
			}
		}

		public BatteryState State
		{
			get
			{
				Permissions.EnsureDeclared<Permissions.Battery>();

				using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
				using (var battery = Platform.AppContext.RegisterReceiver(null, filter))
				{
					var status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
					switch (status)
					{
						case (int)BatteryStatus.Charging:
							return BatteryState.Charging;
						case (int)BatteryStatus.Discharging:
							return BatteryState.Discharging;
						case (int)BatteryStatus.Full:
							return BatteryState.Full;
						case (int)BatteryStatus.NotCharging:
							return BatteryState.NotCharging;
					}
				}

				return BatteryState.Unknown;
			}
		}

		public BatteryPowerSource PowerSource
		{
			get
			{
				Permissions.EnsureDeclared<Permissions.Battery>();

				using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
				using (var battery = Platform.AppContext.RegisterReceiver(null, filter))
				{
					var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);

					if (chargePlug == (int)BatteryPlugged.Usb)
						return BatteryPowerSource.Usb;

					if (chargePlug == (int)BatteryPlugged.Ac)
						return BatteryPowerSource.AC;

					if (chargePlug == (int)BatteryPlugged.Wireless)
						return BatteryPowerSource.Wireless;

					return BatteryPowerSource.Battery;
				}
			}
		}
	}

	[BroadcastReceiver(Enabled = true, Exported = false, Label = "Essentials Battery Broadcast Receiver")]
	class BatteryBroadcastReceiver : BroadcastReceiver
	{
		Action onChanged;

		public BatteryBroadcastReceiver()
		{
		}

		public BatteryBroadcastReceiver(Action onChanged) =>
			this.onChanged = onChanged;

		public override void OnReceive(Context context, Intent intent) =>
			onChanged?.Invoke();
	}

	[BroadcastReceiver(Enabled = true, Exported = false, Label = "Essentials Energy Saver Broadcast Receiver")]
	class EnergySaverBroadcastReceiver : BroadcastReceiver
	{
		Action onChanged;

		public EnergySaverBroadcastReceiver()
		{
		}

		public EnergySaverBroadcastReceiver(Action onChanged) =>
			this.onChanged = onChanged;

		public override void OnReceive(Context context, Intent intent) =>
			onChanged?.Invoke();
	}
}
