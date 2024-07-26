using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

public class BluetoothService
{
    private readonly IAdapter _adapter;
    private readonly IBluetoothLE _bluetoothLE;
    public ObservableCollection<IDevice> Devices { get; private set; }

    public BluetoothService()
    {
        _bluetoothLE = CrossBluetoothLE.Current;
        _adapter = CrossBluetoothLE.Current.Adapter;
        Devices = new ObservableCollection<IDevice>();

        _adapter.DeviceDiscovered += OnDeviceDiscovered;
    }

    private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
    {
        if (!Devices.Contains(e.Device))
        {
            Devices.Add(e.Device);
        }
    }

    public async Task StartScanningAsync()
    {
        if (!_bluetoothLE.IsOn)
        {
            await Application.Current.MainPage.DisplayAlert("Bluetooth Desligado", "Por favor, ative o Bluetooth para continuar.", "OK");
            return;
        }

        Devices.Clear();
        await _adapter.StartScanningForDevicesAsync();
    }

    public async Task ConnectToDeviceAsync(IDevice device)
    {
        await _adapter.ConnectToDeviceAsync(device);
    }

    public async Task DisconnectDeviceAsync(IDevice device)
    {
        await _adapter.DisconnectDeviceAsync(device);
    }

    public async Task PrintTestPageAsync(IDevice device)
    {
        var service = await device.GetServiceAsync(Guid.Parse("YOUR_SERVICE_UUID"));
        var characteristic = await service.GetCharacteristicAsync(Guid.Parse("YOUR_CHARACTERISTIC_UUID"));

        if (characteristic != null)
        {
            var data = Encoding.UTF8.GetBytes("This is a test print page.");
            await characteristic.WriteAsync(data);
        }
        else
        {
            throw new Exception("A característica de impressão não foi encontrada.");
        }
    }
}
