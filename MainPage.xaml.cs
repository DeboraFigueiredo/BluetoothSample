using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.BLE.Abstractions.EventArgs;

namespace BluetoothSample
{
    public partial class MainPage : ContentPage
    {
        private readonly BluetoothService _bluetoothService;

        public MainPage()
        {
            InitializeComponent();
            _bluetoothService = new BluetoothService();
            BindingContext = _bluetoothService;
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Verifique se o Bluetooth está ligado
                if (!CrossBluetoothLE.Current.IsOn)
                {
                    await DisplayAlert("Erro", "Bluetooth está desligado.", "OK");
                    return;
                }

                // Inicie a varredura e atualize a interface
                await _bluetoothService.StartScanningAsync();
                DevicesListView.ItemsSource = null;
                DevicesListView.ItemsSource = _bluetoothService.Devices;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void OnConnectButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var selectedDevice = DevicesListView.SelectedItem as IDevice;
                if (selectedDevice != null)
                {
                    await _bluetoothService.ConnectToDeviceAsync(selectedDevice);
                    await DisplayAlert("Conexão", "Dispositivo conectado com sucesso.", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Nenhum dispositivo selecionado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }

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
            // Adicione o dispositivo se ainda não estiver na lista
            if (!Devices.Contains(e.Device))
            {
                Devices.Add(e.Device);
            }
        }

        public async Task StartScanningAsync()
        {
            if (!_bluetoothLE.IsOn)
            {
                throw new Exception("Bluetooth está desligado.");
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
    }
}
