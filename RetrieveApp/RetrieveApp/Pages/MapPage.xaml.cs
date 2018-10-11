using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RetrieveApp.Database;
using RetrieveApp.Elements;
using RetrieveApp.Elements.Card;
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
        public static double BUTTON_WIDTH = 45;
        private double LAT;
        private double LON;
        private double ALT;
        private MediaFile file;
        private LayoutButtons layout_b;
        private LayoutButtons la_2;
        private List<Products> vis1 = new List<Products>();
        private List<Products> vis_admin = new List<Products>();

		public MapPage (Account _d)
		{
                _g = _d;
            if (mapPage == null)
                {
                    mapPage = this;
                }
            TimeSpan t = TimeSpan.FromHours(9);
            for (int x = 0; x < 100; x++)
            {
                DBActions.products.Add(new Products{
                        ID = 3,
                        AdminID = "Admin",
                        Description = "Crap",
                        PName = "Cookies asdsadasdadjkl", OldPrice=20, NewPrice=10,
                        Quantity = 10, ExpireTime=t});
            }
            InitializeComponent();
            lbl_lg.Text = (_d is Admins) ? ((Admins)_d).SName
                : ((Guests)_d).Name;
            if(_d is Guests)
            {
                Children.Remove(pg_admin);
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
            layout_b = new LayoutButtons(stk_btns, fl, sc, null);
            la_2 = new LayoutButtons(stk_btns2, fl1, sc1, "admin");
            rel("");
            if(_d is Admins)
            {
                rel_admin("");
            } else
            {

            }
            layout_b.AddItems();
            la_2.AddItems();
        }

        private void TextChanged(object s, TextChangedEventArgs a)
        {
            string f = a.NewTextValue;
            rel(f);
        }

        private void rel(string f)
        {
            vis1.Clear();
            if (f != null && f != "")
            {
                f = f.ToLower();
                foreach (Products p in DBActions.products)
                {
                    Admins ad = DBActions._p(p);
                    if (ad.SName.ToLower().StartsWith(f))
                    {
                        vis1.Add(p);
                    }
                }
            }
            else
            {
                foreach (Products p in DBActions.products)
                {
                    vis1.Add(p);
                }
            }
            layout_b.visible = vis1;
            layout_b.OpenPage(1);
        }

        private void rel_admin(string f)
        {
            vis_admin.Clear();
            List<Products> list = DBActions._a((Admins)_g);
            if (f != null && f != "")
            {
                f = f.ToLower();
                foreach (Products p in list)
                {
                    Admins ad = DBActions._p(p);
                    if (ad.SName.ToLower().StartsWith(f))
                    {
                        vis_admin.Add(p);
                    }
                }
            }
            else
            {
                foreach (Products p in list)
                {
                    vis_admin.Add(p);
                }
            }
            la_2.visible = vis_admin;
            la_2.OpenPage(1);
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
                        CompressionQuality = 0,
                        PhotoSize = PhotoSize.Small
                    });
                    image.Source = ImageSource.FromStream(() => { return file.GetStream(); });
                } catch(Exception ex)
                {
                    
                    DisplayAlert("daw", "" + ex.Message, "dawd");
                }
                //if (!CrossMedia.Current.IsCameraAvailable ||
                //!CrossMedia.Current.IsTakePhotoSupported)
                //{
                //    DisplayAlert("ERR", "Not supported!", "Cancel");
                //}
            }
        }
        private void ProcessLogOut(object s, object a)
        {
            Application.Current.Properties.Remove("Logged");
            Navigation.PushAsync(new WelcomePage("LoginOnly"));
            Navigation.RemovePage(this);
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
        private async Task getLocation()
        {
            var loc = CrossGeolocator.Current;
            loc.DesiredAccuracy = 20;
            var pos = await loc.GetPositionAsync(TimeSpan.FromMilliseconds(1000));
            LAT = pos.Latitude;
            LON = pos.Longitude;
            ALT = pos.Altitude;
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