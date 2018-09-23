using Plugin.Geolocator;
using RetrieveApp.Database;
using RetrieveApp.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private double LAT;
        private double LON;
        private double ALT;

		public MapPage (Account _d)
		{
                _g = _d;
            if (mapPage == null)
                {
                    mapPage = this;
                }
                InitializeComponent();
            if(_d is Guests)
            {
                Children.Remove(pg_admin);
            }
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
                foreach(Products pr in DBActions.products)
                {
                fl.Children.Add(new ProductCard(pr));
                }
            btn_img.Clicked += Camera_Clicked;
        }
        private async void Camera_Clicked(object sender, EventArgs e)
        {

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null) { }
                //PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
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
	}
}