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
                Console.WriteLine("Botão de varredura clicado.");
                if (!CrossBluetoothLE.Current.IsOn)
                {
                    await DisplayAlert("Erro", "Bluetooth está desligado.", "OK");
                    return;
                }

                await _bluetoothService.StartScanningAsync();
                DevicesListView.ItemsSource = null;
                DevicesListView.ItemsSource = _bluetoothService.Devices;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void OnConnectButtonClicked(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Botão de conexão clicado.");
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
                Console.WriteLine($"Erro: {ex.Message}");
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void OnPrintButtonClicked(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Botão de impressão clicado.");
                var selectedDevice = DevicesListView.SelectedItem as IDevice;
                if (selectedDevice != null)
                {
                    await _bluetoothService.PrintTestPageAsync(selectedDevice);
                    await DisplayAlert("Impressão", "Página de teste enviada para a impressora.", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Nenhum dispositivo selecionado.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}