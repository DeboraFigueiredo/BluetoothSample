using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace BluetoothSample
{
    [Activity(Label = "BluetoothSample", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, Theme = "@style/Maui.SplashTheme")]
    public class MainActivity : MauiAppCompatActivity
    {
        const int RequestLocationId = 0;

        readonly string[] Permissions =
        {
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.Bluetooth,
            Manifest.Permission.BluetoothAdmin,
            Manifest.Permission.BluetoothConnect,
            Manifest.Permission.BluetoothAdvertise,
            Manifest.Permission.BluetoothScan
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if ((int)Build.VERSION.SdkInt >= 31)
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothScan) != (int)Permission.Granted ||
                    ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothConnect) != (int)Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, Permissions, RequestLocationId);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults.Length > 0 && grantResults[0] == (int)Permission.Granted)
                        {
                            // Permissão concedida 
                            Console.WriteLine("Permissão de Bluetooth concedida.");
                        }
                        else
                        {
                            // Permissão negada 
                            Console.WriteLine("Permissão de Bluetooth negada.");
                        }
                    }
                    break;
            }
        }
    }
}