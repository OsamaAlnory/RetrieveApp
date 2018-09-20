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
        private static Account _g;
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
                var map = new Map(MapSpan.FromCenterAndRadius(
                    new Position(56.05883, 12.7326381), Distance.FromMiles(3)))
                {
                    IsShowingUser = true,
                };
                map.MapType = MapType.Street;
                st.Children.Add(map);
                fl.Children.Add(new ProductCard(
                    new Product("Hello", 1, 1, 1, DateTime.Now, null)));
                fl.Children.Add(new ProductCard(
                    new Product("Hej", 1, 1, 1, DateTime.Now, null)));
                prd_name.Margin = new Thickness(0, App.ScreenHeight / 24, 0, 0);
                foreach (Admins a in DBActions.admins)
                {
                    map.Pins.Add(new IPin(a));
                }
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