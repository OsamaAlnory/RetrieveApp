using Newtonsoft.Json;
using RetrieveApp.Elements;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace RetrieveApp.Database
{
    internal class DBActions
    {
        private const string LINK = "http://windows.u7979705.fsdata.se/api/";
        private const string IMAGES = "Images";
        private const string USERS = "Users";
        private const string PRODUCTS = "Products";
        private const string ADMINS = "Admins";
        private const string AIMAGE = "AdminsImage";
        private const string OPTIONS = "Options";
        public static List<Admins> admins = new List<Admins>();
        public static List<Guests> guests = new List<Guests>();
        public static List<Products> products = new List<Products>();
        public static Dictionary<string, object> options = new Dictionary<string, object>();

        public static async void LoadData()
        {
            if(await LoadOptions())
            {
                return;
            }
            await LoadAccounts();
            await LoadProducts();
            //UnrealData.Load();
            App.StartLoading("Pins");
            await App.ReloadPins();
            App.FinishLoading("Pins");
            App.StartLoading("Fixing");
            await CheckProducts();
            //await CheckUsersProducts();
            App.FinishLoading("Fixing");
        }

        public static async Task<int> CheckProducts()
        {
            int total = 0;
            foreach (Products p in products)
            {
                if(GetAdminById(p.AdminID) == null)
                {
                    total++;
                    await FullyRemoveProduct(p);
                }
            }
            return total;
        }

        public static async Task CheckUsersProducts()
        {
            foreach(Guests guest in guests)
            {
                bool found = false;
                var ids = getCart(guest);
                foreach(int id in ids)
                {
                    if(GetProductById(id) == null)
                    {
                        found = true;
                        var cart = guest.Cart.Split(';');
                        var newCart = "";
                        int S = 0;
                        for (int x = 0; x < cart.Length; x++)
                        {
                            var f = cart[x].Split(',');
                            int a = int.Parse(f[0]);
                            if (a != id)
                            {
                                if (S > 0)
                                {
                                    newCart += ";";
                                }
                                newCart += cart[x];
                                S++;
                            }
                        }
                        if (string.IsNullOrEmpty(newCart))
                        {
                            newCart = null;
                        }
                        guest.Cart = newCart;
                    }
                }
                if (found)
                {
                    await FullyEditUser(guest);
                }
            }
        }

        public static async Task LoadAccounts()
        {
            await LoadAdmins();
            await LoadUsers();
        }

        public static async Task<bool> LoadOptions()
        {
            HttpClient client = new HttpClient();
            var responce = await client.GetStringAsync(LINK + OPTIONS);
            List<Options> op = JsonConvert.DeserializeObject<List<Options>>(responce);
            foreach (Options o in op)
            {
                options.Add(o.Name, o.Value);
            }
            if (options.ContainsKey("IsDev") &&
                options["IsDev"] as string == "true")
            {
                LoadingPage.page.Quit();
                return true;
            }
            return false;
        }

        public static async Task<bool> Check(Admins admin, Page page)
        {
            await LoadAdmins();
            if(GetAdminById(admin.ID) == null)
            {
                await App.Send("Info", "Ert konto har raderats!", "Ok");
                await App.LogOut(page);
                return true;
            }
            return false;
        }

        public static async Task LoadAdmins()
        {
            App.StartLoading("Admins");
            admins.Clear();
            HttpClient client = new HttpClient();
            var responce = await client.GetStringAsync(LINK+ADMINS);
            admins = JsonConvert.DeserializeObject<List<Admins>>(responce);
            App.FinishLoading("Admins");
        }

        public static async Task LoadUsers()
        {
            App.StartLoading("Users");
            HttpClient client = new HttpClient();
            var responce = await client.GetStringAsync(LINK + USERS);
            guests = JsonConvert.DeserializeObject<List<Guests>>(responce);
            App.FinishLoading("Users");
        }

        public static async Task LoadProducts()
        {
            App.StartLoading("Products");
            products.Clear();
            HttpClient client = new HttpClient();
            var responce = await client.GetStringAsync(LINK+PRODUCTS);
            products = JsonConvert.DeserializeObject<List<Products>>(responce);
            foreach(Products p in products)
            {
                if (_p(p) == null)
                {
                    //await FullyRemoveProduct(p);
                }
            }
            App.FinishLoading("Products");
        }
        public static async Task<AdminIcon> LoadAdminIcon(Admins ad)
        {
            HttpClient client = new HttpClient();
            try
            {
                var responce = await client.GetStringAsync(LINK+AIMAGE+"/"+ad.ID);
                return JsonConvert.DeserializeObject<AdminIcon>(responce);
            } catch(Exception e)
            {
                return null;
            }
        }
        public static async Task<Images> LoadProductImage(Products product)
        {
            HttpClient client = new HttpClient();
            try
            {
                var responce = await client.GetStringAsync(LINK+IMAGES+"/"+product.ID);
                return JsonConvert.DeserializeObject<Images>(responce);
            }
            catch (Exception e)
            {
                return null;
            }
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

        public static Admins GetAdminById(string id)
        {
            foreach(Admins a in admins)
            {
                if(a.ID == id)
                {
                    return a;
                }
            }
            return null;
        }

        public static Products GetProductById(int id)
        {
            foreach(Products p in products)
            {
                if(p.ID == id)
                {
                    return p;
                }
            }
            return null;
        }

        public static List<int> GetBookers(Products p)
        {
            var list = new List<int>();
            foreach(Guests g in guests)
            {
                if(hasBooked(g, p))
                {
                    list.Add(g.ID);
                }
            }
            return list;
        }

        public static List<Products> GetProducts(Guests g)
        {
            var list = new List<Products>();
            foreach(Products p in products)
            {
                if (GetBookers(p).Contains(g.ID))
                {
                    list.Add(p);
                }
            }
            return list;
        }
        public static int GetQuantity(Guests user, Products product)
        {
            if (hasBooked(user, product))
            {
                var str = user.Cart.Split(';');
                foreach(var a in str)
                {
                    var b = a.Split(',');
                    if(int.Parse(b[0]) == product.ID)
                    {
                        return int.Parse(b[1]);
                    }
                }
            }
            return 0;
        }
        public static List<int> getCart(Guests g)
        {
            List<int> ints = new List<int>();
                if (!string.IsNullOrEmpty(g.Cart))
                {
                    string[] s = g.Cart.Split(';');
                    foreach (string a in s)
                    {
                        string[] d = a.Split(',');
                        ints.Add(int.Parse(d[0]));
                    }
                }
            return ints;
        }
        public static async Task Unbook(Guests user, Products product, bool restore)
        {
            if(hasBooked(user, product))
            {
                var cart = user.Cart.Split(';');
                var newCart = "";
                int quantity = 0;
                int S = 0;
                    for (int x = 0; x < cart.Length; x++)
                    {
                        var f = cart[x].Split(',');
                        int a = int.Parse(f[0]);
                        if (a != product.ID)
                        {
                            if (S > 0)
                            {
                                newCart += ";";
                            }
                            newCart += cart[x];
                        S++;
                        }
                        else
                        {
                            quantity = int.Parse(f[1]);
                        }
                    }
                    if (string.IsNullOrEmpty(newCart))
                    {
                        newCart = null;
                    }
                    user.Cart = newCart;
                    await FullyEditUser(user);
                    if (restore)
                    {
                        product.Quantity += quantity;
                        await FullyEditProduct(product);
                    }
            }
        }
        public static async Task book(Guests user, Products product, int quantity)
        {
            addToCart(user, product, quantity);
            product.Quantity -= quantity;
            await FullyEditProduct(product);
            await FullyEditUser(user);
        }
        public static void addToCart(Guests user, Products product, int quantity)
        {
            if(!string.IsNullOrEmpty(user.Cart))
            {
                user.Cart += ";";
            }
            user.Cart += product.ID + "," + quantity;
        }
        public static bool hasBooked(Guests user, Products product)
        {
            return getCart(user).Contains(product.ID);
        }

        public static async Task RemoveFromCarts(Products product)
        {
            foreach(Guests user in guests)
            {
                await Unbook(user, product, false);
            }
        }

        public static List<int> GetQuantities(Guests user)
        {
            var list = new List<int>();
            if(!string.IsNullOrEmpty(user.Cart))
            {
                var cart = user.Cart.Split(';');
                for (int x = 0; x < cart.Length; x++)
                {
                    list.Add(int.Parse(cart[x].Split(',')[1]));
                }
            }
            return list;
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
        public static async Task<bool> Process(string actionName,params object[] values)
        {
            HttpResponseMessage response = null;
            if(actionName == "adduser")
            {
                response = await AddUser(values[0] as Guests);
            } else if(actionName == "addproduct")
            {

            }
            if(response != null && response.StatusCode != HttpStatusCode.Created)
            {
                 App.CURRENT_PAGE.DisplayAlert("Fel", response.StatusCode.ToString()
                    +"\nKontakta oss.", "Avbryt");
            }
            return response != null && response.StatusCode == HttpStatusCode.Created;
        }
        public static List<Products> _a(Admins a)
        {
            var list = new List<Products>();
            foreach(Products p in products)
            {
                if (p.AdminID == a.ID)
                {
                    list.Add(p);
                }
            }
            return list;
        }

        public static async Task<HttpResponseMessage> AddUser(Guests user)
        {
            var json = JsonConvert.SerializeObject(user);
            var c = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            return await client.PostAsync(LINK+USERS, c);
        }
        public static async Task<HttpResponseMessage> RemoveUser(Guests user)
        {
            var json = JsonConvert.SerializeObject(user);
            var c = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            return await client.DeleteAsync(LINK+USERS+"/"+user.ID);
        }
        public static async Task<HttpResponseMessage> RemoveProduct(int id)
        {
            HttpClient client = new HttpClient();
            return await client.DeleteAsync(LINK+PRODUCTS+"/"+id);
        }
        public static async Task<HttpResponseMessage> RemoveAdminIcon(Admins admin)
        {
            HttpClient client = new HttpClient();
            return await client.DeleteAsync(LINK+AIMAGE+"/"+admin.ID);
        }
        public static async Task<HttpResponseMessage> RemoveProductImage(int id)
        {
            HttpClient client = new HttpClient();
            return await client.DeleteAsync(LINK+IMAGES+"/"+id);
        }
        public static async Task<HttpResponseMessage> RemoveAdmin(Admins admin)
        {
            HttpClient client = new HttpClient();
            return await client.DeleteAsync(LINK+ADMINS+"/"+admin.ID);
        }
        public static async Task FullyRemoveAdmin(Admins admin)
        {
            await RemoveAdmin(admin);
            await RemoveAdminIcon(admin);
            await LoadAdmins();
        }
        public static async Task FullyAddProduct(Products product, byte[] img)
        {
            await AddProduct(product);
            await AddProductImage(new Images { ID = product.ID, Image = img});
            await LoadProducts();
        }
        public static async Task AddProduct(Products product)
        {
            var json = JsonConvert.SerializeObject(product);
            var c = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var r = await client.PostAsync(LINK+PRODUCTS, c);
        }
        public static async Task AddAdmin(Admins admin)
        {
            var json = JsonConvert.SerializeObject(admin);
            var c = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var r = await client.PostAsync(LINK+ADMINS, c);
        }
        public static async Task FullyAddAdmin(Admins admin)
        {
            await AddAdmin(admin);
            await LoadAdmins();
        }
        public static async Task AddAdminIcon(AdminIcon img)
        {
            var json = JsonConvert.SerializeObject(img);
            var c = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var r = await client.PostAsync(LINK+AIMAGE, c);
        }
        public static async Task AddProductImage(Images img)
        {
            var json = JsonConvert.SerializeObject(img);
            var c = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var r = await client.PostAsync(LINK+IMAGES, c);
        }
        public static async Task<HttpResponseMessage> EditAdmin(Admins admin)
        {
            var json = JsonConvert.SerializeObject(admin);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var responce = await client.PutAsync(LINK+ADMINS+"/" + admin.ID, content);
            return responce;
        }
        public static async Task FullyEditAdmin(Admins admin)
        {
            await EditAdmin(admin);
            await LoadAdmins();
        }
        public static async Task<HttpResponseMessage> EditUser(Guests g)
        {
            //if (string.IsNullOrEmpty(g.Cart))
            //{
            //    g.Cart = null;
            //}
            var json = JsonConvert.SerializeObject(g);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var responce = await client.PutAsync(LINK+USERS+"/"+g.ID, content);
            return responce;
        }
        public static async Task<HttpResponseMessage> EditAdminIcon(AdminIcon icon)
        {
            var json = JsonConvert.SerializeObject(icon);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var responce = await client.PutAsync(LINK+AIMAGE+"/"+icon.ID, content);
            return responce;
        }
        public static async Task EditProduct(Products product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var responce = await client.PutAsync(LINK + PRODUCTS + "/" + product.ID, content);
        }
        public static async Task FullyEditProduct(Products product)
        {
            await EditProduct(product);
            await LoadProducts();
        }
        public static async Task FullyRemoveProduct(Products product)
        {
            await RemoveFromCarts(product);
            products.Remove(product);
            await RemoveProduct(product.ID);
            await RemoveProductImage(product.ID);
        }
        public static async Task FullyEditUser(Guests user)
        {
            await EditUser(user);
            await LoadUsers();
        }

    }
}
