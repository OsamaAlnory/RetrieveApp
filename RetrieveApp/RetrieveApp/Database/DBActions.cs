using Newtonsoft.Json;
using RetrieveApp.Elements;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace RetrieveApp.Database
{
    internal class DBActions
    {
        private const string LINK = "http://windows.u7979705.fsdata.se/api/";
        public static List<Admins> admins = new List<Admins>();
        public static List<Guests> guests = new List<Guests>();
        public static List<Products> products = new List<Products>();
        public static Dictionary<string, object> options = new Dictionary<string, object>();

        public static async void GetAccounts()
        {

            //await LoadAccounts();
            /*
            HttpClient client = new HttpClient();
            var responce4 = await client.GetStringAsync("http://fannylovisa.somee.com/api/options");
            List<Options> op = JsonConvert.DeserializeObject<List<Options>>(responce4);
            foreach (Options o in op)
            {
                options.Add(o.Name, o.Value);
            }
            if (options.ContainsKey("IsDev") &&
                options["IsDev"] as string == "true")
            {
                LoadingPage.page.Quit();
                return;
            }
            */
            await LoadProducts();
            App.StartLoading("Pins");
            admins.Add(new Admins
            {
                ID = "Admin",Address = "Helsingborg Rönnegatan 5e",
                SName = "WOW", Password = "123"
            });
            admins.Add(new Admins { ID="Test", Address="Lund Storgatan",
                SName ="Lidl",Password="123"});
            guests.Add(new Guests {
                ID = 5, Name = "Osama",
                Password = "123"
            });
            products.Add(new Products { ID = 12432, AdminID = "Test", PName="Burger",
            Description="dwad"});
            TimeSpan t = TimeSpan.FromHours(9);
            products.Add(new Products{ID=11,AdminID="Admin",PName="Cookie",
            OldPrice=10, NewPrice=5, Quantity=18, Description="Hello!"});
            addToCart(guests[0], products[0], 10);
            await App.ReloadPins();
            App.FinishLoading("Pins");
        }

        public static async Task LoadAccounts()
        {
            App.StartLoading("Users");
            admins.Clear();
            guests.Clear();
            HttpClient client = new HttpClient();
            //var responce1 = await client.GetStringAsync("http://windows.u7979705.fsdata.se/api/Users");
            //admins = JsonConvert.DeserializeObject<List<Admins>>(responce1);
            var responce2 = await client.GetStringAsync(LINK+"Users");
            guests = JsonConvert.DeserializeObject<List<Guests>>(responce2);
            App.FinishLoading("Users");
        }

        public static async Task LoadProducts()
        {
            App.StartLoading("Products");
            products.Clear();
            HttpClient client = new HttpClient();
            var responce3 = await client.GetStringAsync(LINK+"Products");
            products = JsonConvert.DeserializeObject<List<Products>>(responce3);
            App.FinishLoading("Products");
        }

        public static Guests GetUserById(int id)
        {
            foreach(Guests g in guests)
            {
                if(g.ID == id)
                {
                    return g;
                }
            }
            return null;
        }

        public static List<Guests> GetBookers(Products p)
        {
            var list = new List<Guests>();
            foreach(Guests g in guests)
            {
                if(hasBooked(g, p))
                {
                    list.Add(g);
                }
            }
            return list;
        }

        public static List<Products> GetProducts(Guests g)
        {
            var list = new List<Products>();
            foreach(Products p in products)
            {
                if (GetBookers(p).Contains(g))
                {
                    list.Add(p);
                }
            }
            return list;
        }

        public static List<int> getCart(Guests g)
        {
            List<int> ints = new List<int>();
            if(g.Cart != null)
            {
                string[] s = g.Cart.Split(';');
                foreach(string a in s)
                {
                    string[] d = a.Split(',');
                    ints.Add(int.Parse(d[0]));
                }
            }
            return ints;
        }
        public static void book(Guests user, Products product, int quantity)
        {
            addToCart(user, product, quantity);
            product.Quantity -= quantity;
            // Save to db
        }
        public static void addToCart(Guests user, Products product, int quantity)
        {
            user.Cart += product.ID+","+quantity;
        }
        public static bool hasBooked(Guests user, Products product)
        {
            return getCart(user).Contains(product.ID);
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
        public static List<Products> _a(Admins a)
        {
            List<Products> list = new List<Products>();
            foreach(Products p in products)
            {
                if(p.AdminID == a.ID)
                {
                    list.Add(p);
                }
            }
            return list;
        }

        public static async Task AddUser(Guests user)
        {
            var json = JsonConvert.SerializeObject(user);
            var c = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var r = await client.PostAsync(LINK+"Users", c);
            if(r.StatusCode == HttpStatusCode.Created)
            {
                
            }
        }
        public static async void RemoveProduct(Products product)
        {
            products.Remove(product);
            HttpClient client = new HttpClient();
            var r = await client.DeleteAsync(LINK+"Products/"+product.ID);
        }
        public static async Task AddProduct(Products product)
        {
            var json = JsonConvert.SerializeObject(product);
            var c = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var r = await client.PostAsync(LINK+"Products", c);
            if (r.StatusCode == HttpStatusCode.Created)
            {
                //
            }
            MapPage.mapPage.DisplayAlert("dwad", ""+r.StatusCode,"dwa");
        }

    }
}
