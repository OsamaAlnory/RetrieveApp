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
        public static FirstLogin firstLogin;
        bool hadAdminIcon = false;

		public FirstLogin (Admins _a)
		{
            firstLogin = this;
            this._a = _a;
			InitializeComponent ();
            SetIcon();
            img.Source = App.GetSource("background.png");
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                file = await App.OpenCamera(true);
                if (file != null)
                {
                    icon.Source = ImageSource.FromStream(() => { return file.GetStream(); });
                }
            };
            icon.GestureRecognizers.Add(tap);
            btn.FontSize = Device.GetNamedSize(NamedSize.Large, btn);
            info.FontSize = Device.GetNamedSize(NamedSize.Medium, info);
            info1.FontSize = Device.GetNamedSize(NamedSize.Medium, info);
            info2.FontSize = Device.GetNamedSize(NamedSize.Medium, info);
            loc.FontSize = Device.GetNamedSize(NamedSize.Large, loc);
            phone.FontSize = Device.GetNamedSize(NamedSize.Large, phone);
            name.FontSize = Device.GetNamedSize(NamedSize.Large, name);
            email.FontSize = Device.GetNamedSize(NamedSize.Large, email);
            Calc(loc); Calc(phone); Calc(name); Calc(email);
        }

        private async void SetIcon()
        {
            var v = await DBActions.LoadAdminIcon(_a);
            if(v != null)
            {
                hadAdminIcon = true;
                icon.Source = App.ByteToImage(v.Image);
            } else
            {
                icon.Source = App.GetSource("icon.png");
            }
        }

        private void Calc(View e)
        {
            e.HeightRequest = App.ScreenHeight / 16;
            e.Margin = new Thickness(App.ScreenWidth/30, 0, App.ScreenWidth/30, 0);
        }

        private async void ButtonClicked(object s, EventArgs a)
        {
            if (loading)
            {
                return;
            }
            var x = loc.Text; var x1 = name.Text; var x2 = email.Text; var x3 = phone.Text;
            loading = true;
            if(file == null)
            {
                DisplayAlert("Fel", "Välj en ikon!", "Avbryt");
                loading = false;
                return;
            }
            if (x != null && x1 != null && x2 != null && x3 != null)
            {
                an.IsVisible = true;
                an.Play();
                var list = await App.GetPositions(x);
                if(list.Count == 1)
                {
                    _a.Address = await App.GetAddress(list[0]);
                    _a.SName = x1; _a.Email = x2; _a.Phone = x3; _a.Login = true;
                    await DBActions.EditAdmin(_a);
                    var aicon = new AdminIcon { ID=_a.ID, Image=App.ImageToByte(file)};
                    if (hadAdminIcon)
                    {
                        await DBActions.EditAdminIcon(aicon);
                    } else
                    {
                        await DBActions.AddAdminIcon(aicon);
                    }
                    await App.ReloadPins();
                    an.IsVisible = false;
                    an.Pause();
                    Navigation.PushAsync(new MapPage(_a));
                    App.RemovePage(this);
                    return;
                }
            }
            an.IsVisible = false;
            an.Pause();
            loading = false;
            await DisplayAlert("Fel", "Ange en giltig adress!", "Avbryt");
        }
	}
}