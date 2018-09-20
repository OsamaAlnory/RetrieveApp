using Newtonsoft.Json;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RetrieveApp.Database
{
    internal class DBActions
    {
        public static List<Admins> admins = new List<Admins>();
        public static List<Guests> guests = new List<Guests>();

        public static async void GetAccounts()
        {
            App.StartLoading("Users");
            HttpClient client = new HttpClient();
            var responce1 = await client.GetStringAsync("http://fannylovisa.somee.com/api/Admins");
            admins = JsonConvert.DeserializeObject<List<Admins>>(responce1);
            var responce2 = await client.GetStringAsync("http://fannylovisa.somee.com/api/Users");
            guests = JsonConvert.DeserializeObject<List<Guests>>(responce2);
            App.FinishLoading("Users");
        }
            

    }
}
