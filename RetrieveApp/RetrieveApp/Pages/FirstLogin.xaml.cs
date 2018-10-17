using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FirstLogin : ContentPage
	{
        private MediaFile file;

		public FirstLogin ()
		{
			InitializeComponent ();
            icon.Source = App.GetSource("b2.png");
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                await CrossMedia.Current.Initialize();
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }
                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                {
                    try
                    {
                        file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions{});
                        icon.Source = ImageSource.FromStream(() => { return file.GetStream();});
                    }
                    catch {}
                }
                };
            icon.GestureRecognizers.Add(tap);
            loc.FontSize = Device.GetNamedSize(NamedSize.Large, loc);
            loc.HeightRequest = App.ScreenHeight / 20;
            btn.HeightRequest = App.ScreenHeight / 18;
		}
	}
}