using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RetrieveApp.Database;
using RetrieveApp.Elements;
using RetrieveApp.Elements.Card;
using RetrieveApp.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : TabbedPage
	{
        public static Account _g;
        public static MapPage mapPage;
        private MediaFile file;
        private MapPageEvents events;

		public MapPage (Account _d)
		{
                _g = _d;
            if (mapPage == null)
                {
                    mapPage = this;
                }
            TimeSpan t = TimeSpan.FromHours(9);
            for (int x = 0; x < 1000; x++)
            {
                DBActions.products.Add(new Products{
                        ID = 3,
                        AdminID = "Admin",
                        Description = "Crap",
                        PName = new Random().Next(9999)+"Cookies",
                        OldPrice =20, NewPrice=10,
                        Quantity = 10, ExpireTime=t});
            }
            InitializeComponent();
            lbl_lg.Text = (_d is Admins) ? ((Admins)_d).SName
                : ((Guests)_d).Name;
            if(_d is Guests)
            {
                Children.Remove(pg_admin);
                filters.IsVisible = false;
            } else
            {
                pg_b.Title = "Mina produkter";
            }
            image.Source = ImageSource.FromResource(App.PATH+"noimage.png",
                Assembly.GetExecutingAssembly());
                var map = new Map(MapSpan.FromCenterAndRadius(
                    new Position(56.05883, 12.7326381), Distance.FromMiles(3)))
                {
                    IsShowingUser = true,
                };
                map.MapType = MapType.Street;
                st.Children.Add(map);
                prd_name.Margin = new Thickness(0, App.ScreenHeight / 24, 0, 0);
                foreach (IPin pin in IPin.pins)
                {
                    map.Pins.Add(pin);
                }
            btn_img.Clicked += Camera_Clicked;
            logout.Clicked += ProcessLogOut;
            search.TextChanged += TextChanged;
            search1.TextChanged += TextChanged1;
            events = new MapPageEvents(new LayoutButtons(stk_btns, fl, sc, null)
             ,new LayoutButtons(stk_btns2, fl1, sc1, "admin"));
            events.rel("");
            if(_d is Admins)
            {
                events.rel_admin("");
            } else
            {

            }
            events.AddItems();
            Designer.DesignPageButtons(stk_btns);
            Designer.DesignPageButtons(stk_btns2);
            filter_all.FontSize = Device.GetNamedSize(NamedSize.Small, filter_all);
            filter_b.FontSize = Device.GetNamedSize(NamedSize.Small, filter_b);
        }

        private void TextChanged(object s, TextChangedEventArgs a)
        {
            events.rel(a.NewTextValue);
        }

        private void TextChanged1(object s, TextChangedEventArgs a)
        {
            events.rel_admin(a.NewTextValue);
        }

        public string TXT()
        {
            return search1.Text;
        }

        private async void Camera_Clicked(object sender, EventArgs e)
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
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        AllowCropping = true,
                        CompressionQuality = 0,
                        PhotoSize = PhotoSize.Small
                    });
                    image.Source = ImageSource.FromStream(() => { return file.GetStream(); });
                } catch(Exception ex)
                {
                    await DisplayAlert("Fel",ex.Message,"Avbryt");
                }
            }
        }
        private async void ProcessLogOut(object s, object a)
        {
            bool IS = await App.SendSure(this);
            if (IS)
            {
                Application.Current.Properties.Remove("Logged");
                await Navigation.PushAsync(new WelcomePage("LoginOnly"));
                Navigation.RemovePage(this);
            }
        }

        private void Btn_add(object s, EventArgs a)
        {
            icon_r.Speed = 0.6f;
            icon_r.IsVisible = true;
            icon_r.Play();
            DBActions.AddProduct(new Products {
                AdminID = ((Admins)_g).ID,
                PName = prd_name.Text,
                OldPrice = 10,
                NewPrice = 20,
                Quantity = 50,
                ExpireTime = tmr.Time,
                Description = desc.Text,
                Image = App.ImageToByte(file)
            });
        }
        public static Page GetPageByName(string title)
        {
            foreach(Page p in mapPage.Children)
            {
                if(p.Title == title)
                {
                    return p;
                }
            }
            return null;
        }
        public static void PinSearch(string txt)
        {
            mapPage.CurrentPage = GetPageByName("Flöde");
            mapPage.search.Text = txt;
        }
        public void OpenProduct(Products p)
        {
            Navigation.PushAsync(new ProductView(p));
        }
	}
}