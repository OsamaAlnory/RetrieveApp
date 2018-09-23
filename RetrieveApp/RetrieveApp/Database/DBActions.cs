using Newtonsoft.Json;
using RetrieveApp.Elements;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms.Maps;

namespace RetrieveApp.Database
{
    internal class DBActions
    {
        public static List<Admins> admins = new List<Admins>();
        public static List<Guests> guests = new List<Guests>();
        public static List<Products> products = new List<Products>();

        public static async void GetAccounts()
        {
            App.StartLoading("Users");
            HttpClient client = new HttpClient();
            var responce1 = await client.GetStringAsync("http://fannylovisa.somee.com/api/Admins");
            admins = JsonConvert.DeserializeObject<List<Admins>>(responce1);
            var responce2 = await client.GetStringAsync("http://fannylovisa.somee.com/api/Users");
            guests = JsonConvert.DeserializeObject<List<Guests>>(responce2);
            App.FinishLoading("Users");
            App.StartLoading("Products");
            var responce3 = await client.GetStringAsync("http://fannylovisa.somee.com/api/Products");
            products = JsonConvert.DeserializeObject<List<Products>>(responce3);
            App.FinishLoading("Products");
            App.StartLoading("Pins");
            Geocoder g = new Geocoder();
            foreach (Admins place in admins)
            {
                Position pos = new Position();
                IEnumerable<Position> result = await g.GetPositionsForAddressAsync(place.Address);
                foreach (Position p in result)
                {
                    pos = p;
                    break;
                }
                new IPin(place) { Position = pos};
            }
            App.FinishLoading("Pins");
        }
            
        public static Admins _p(Products p) {
            foreach(Admins a in admins)
            {
                if(a.ID == p.AdminID)
                {
                    return a;
                }
            }
            return null;
        }

        public static Guests _g(Products p)
        {
            foreach (Guests a in guests)
            {
                if (a.ID == p.Guest)
                {
                    return a;
                }
            }
            return null;
        }

    }
}
