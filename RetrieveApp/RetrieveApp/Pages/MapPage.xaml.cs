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
	public partial class MapPage : ContentPage
	{
		public MapPage ()
		{
			InitializeComponent ();
            var map = new Map(MapSpan.FromCenterAndRadius(
                new Position(55.9910816, 13.487605), Distance.FromMiles(39)))
            {
                IsShowingUser = true,
            };
            map.MapType = MapType.Hybrid;
            //map.Pins.Add(new Pin {
            //    Position = new Position(37.1, -122),
            //    Label = "Hello There", Address="No", Type=PinType.SavedPin  
            //});
            st.Children.Add(map);
            t.Clicked += (s, e) => {
                Random r = new Random();
                Device.StartTimer(TimeSpan.FromMilliseconds(30), () => {
                    double x = ((double)r.Next(18000)-9000)/100;
                    double y = ((double)r.Next(36000)-18000)/100;
                    var p = new Pin {
                        Position = new Position(x, y),
                        Label = "XD", Address = "LEL",
                        Type = PinType.SavedPin
                    };
                    map.Pins.Add(p);
                    return true;
                });
            };
        }
        
	}
}