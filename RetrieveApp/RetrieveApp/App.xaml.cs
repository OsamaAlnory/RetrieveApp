﻿using Plugin.Geolocator;
using Plugin.Media.Abstractions;
using RetrieveApp.Database;
using RetrieveApp.Elements;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RetrieveApp
{
    public partial class App : Application
    {
        public const string VERSION = "1.0.0";
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
        public static Page CURRENT_PAGE;
        public static List<Loadable> loadables = new List<Loadable>();
        public static readonly string PATH = "RetrieveApp.Images.";
        private static double LAT;
        private static double LON;
        private static double ALT;

        public App()
        {
            InitializeComponent();
            
             MainPage = new NavigationPage(new LoadingPage());
            CURRENT_PAGE = MainPage;
        }

        public static byte[] ImageToByte(MediaFile file)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                // file.Dispose();
                byte[] i = memoryStream.ToArray();
                return i;
            }
        }
        public static ImageSource ByteToImage(byte[] b)
        {
            return ImageSource.FromStream(() => new MemoryStream(b));
        }
        public static async Task ReloadPins()
        {
            foreach (Admins place in DBActions.admins)
            {
                new IPin(place)
                {
                    Position = (await App.GetPositions(place.Address))[0]
                };
            }
        }
        public static async Task<List<Position>> GetPositions(string address)
        {
            Geocoder g = new Geocoder();
            return (await g.GetPositionsForAddressAsync(address)).ToList();
        }
        private static async Task getLocation()
        {
            var loc = CrossGeolocator.Current;
            loc.DesiredAccuracy = 20;
            var pos = await loc.GetPositionAsync(TimeSpan.FromMilliseconds(1000));
            LAT = pos.Latitude;
            LON = pos.Longitude;
            ALT = pos.Altitude;
        }

        public static void Register(Loadable page)
        {
            loadables.Add(page);
        }

        public static ImageSource GetSource(string name)
        {
            return ImageSource.FromResource(PATH+name, Assembly.GetExecutingAssembly());
        }

        public static void StartLoading(string type)
        {
            foreach(Loadable loadable in loadables){
                loadable.OnLoadStarted(type);
            }
        }

        public static void FinishLoading(string type)
        {
            foreach (Loadable loadable in loadables)
            {
                loadable.OnLoadFinished(type);
            }
        }

        public static bool CheckInternetConnection()
        {
            string CheckUrl = "http://google.com";
            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                iNetRequest.Timeout = 8000;
                WebResponse iNetResponse = iNetRequest.GetResponse();
                iNetResponse.Close();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
