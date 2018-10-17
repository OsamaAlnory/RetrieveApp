using RetrieveApp.Database;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms.Maps;

namespace RetrieveApp.Elements
{
    public class IPin : Pin
    {
        public static List<IPin> pins = new List<IPin>();
        private Admins place;


        public IPin(Admins place)
        {
            this.place = place;
            Type = PinType.SavedPin;
            Address = place.Address;
            Label = place.SName;
            Clicked += (s, e) => OnClicked();
            pins.Add(this);
        }
        public Admins getPlace()
        {
            return place;
        }
        private void OnClicked()
        {
            MapPage.PinSearch(place.SName);
        }

    }
}
