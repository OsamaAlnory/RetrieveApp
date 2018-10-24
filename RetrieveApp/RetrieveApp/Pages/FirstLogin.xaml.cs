using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RetrieveApp.Database;
using RetrieveApp.Elements;
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
        private bool loading = false;
        private Admins _a;

		public FirstLogin (Admins _a)
		{
            this._a = _a;
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
                        if(file != null)
                        {
                            icon.Source = ImageSource.FromStream(() => {return file.GetStream();});
                        }
                    }catch {
                        DisplayAlert("Fel", "ERR!", "Avbryt");
                    }
                } else
                {
                    DisplayAlert("Fel", "Åtkomst har blockerats!", "Avbryt");
                }
                };
            icon.GestureRecognizers.Add(tap);
            //loc.HeightRequest = App.ScreenHeight / 15;
            //btn.HeightRequest = App.ScreenHeight / 14;
            btn.FontSize = Device.GetNamedSize(NamedSize.Large, btn);
            loc.FontSize = Device.GetNamedSize(NamedSize.Large, loc);
            info.FontSize = Device.GetNamedSize(NamedSize.Medium, info);
        }

        private async void ButtonClicked(object s, EventArgs a)
        {
            var x = loc.Text;
            loading = true;
            if (x != null)
            {
                var list = await App.GetPositions(x);
                if(list.Count == 1)
                {
                    _a.Address = await App.GetAddress(list[0]);
                    await App.ReloadPins();
                    await Navigation.PushAsync(new MapPage(_a));
                    Navigation.RemovePage(this);
                    return;
                }
            }
            loading = false;
            await DisplayAlert("Fel", "Ange giltig adress!", "Avbryt");
        }
	}
}