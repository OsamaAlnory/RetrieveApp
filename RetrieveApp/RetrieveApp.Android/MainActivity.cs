using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;
using Plugin.Media;
using Plugin.CurrentActivity;

namespace RetrieveApp.Droid
{
    [Activity(Label = "Retrieve It", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnStart()
        {
            base.OnStart();
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation}, 0);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Permission Granted!");
            }
        }
        private static App app;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
           
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                base.OnCreate(savedInstanceState);
                CrossCurrentActivity.Current.Init(this, savedInstanceState);
           
                await CrossMedia.Current.Initialize();
                CrossCurrentActivity.Current.Activity = this;
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
                App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
                App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
                 Xamarin.FormsMaps.Init(this, savedInstanceState);
           
                if (app == null)
                {
                    app = new App();
                }
                LoadApplication(app);
            }
            

             
           
         
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}