using Plugin.Geolocator;
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
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RetrieveApp
{
    public partial class App : Application
    {
        private const bool allowRemoving = true;
        public const string VERSION = "1.0.2";
        public static string ACCOUNT_NAME = "RetrieveIt";
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
            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                // file.Dispose();
                byte[] i = memoryStream.ToArray();
                return i;
            }
        }

        public static void RemovePage(Page page)
        {
            if (allowRemoving)
            {
                page.Navigation.RemovePage(page);
            }
        }

        public static async Task LogOut(Page page)
        {
            if (Current.Properties.ContainsKey("Logged"))
            {
                Current.Properties.Remove("Logged");
            }
            await page.Navigation.PushAsync(new WelcomePage("LoginOnly"));
            page.Navigation.RemovePage(page);
        }

        public static async Task<bool> SendSure(Page page)
        {
            return (await page.DisplayAlert("Meddelande", "Är du säker?", "Ja", "Avbryt"));
        }

        public static async Task<bool> SendSure()
        {
            return await SendSure(MapPage.mapPage);
        }

        public static ImageSource ByteToImage(byte[] b)
        {
            return ImageSource.FromStream(() => new MemoryStream(b));
        }

        public static async Task ReloadPins()
        {
            IPin.pins.Clear();
            foreach (Admins place in DBActions.admins)
            {
                if(place.Address != null)
                {
                    new IPin(place)
                    {
                        Position = (await GetPositions(place.Address))[0]
                    };
                }
            }
        }

        public static async Task<List<Position>> GetPositions(string address)
        {
            Geocoder g = new Geocoder();
            return (await g.GetPositionsForAddressAsync(address)).ToList();
        }

        public static async Task<string> GetAddress(Position pos)
        {
            Geocoder g = new Geocoder();
            return (await g.GetAddressesForPositionAsync(pos)).ToList()[0];
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

        public static async Task<MediaFile> OpenCamera(bool gallery)
        {
            MediaFile file = null;
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
                    if (gallery)
                    {
                        file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { });
                    } else
                    {
                        file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            AllowCropping = true,
                            PhotoSize = PhotoSize.MaxWidthHeight
                        });
                    }
                }
                catch { }
            }
            return file;
        }

        public static async Task Send(string title, string msg, string ok)
        {
             await MapPage.mapPage.DisplayAlert(title, msg, ok);
        }

        public static async Task<bool> Send(string title, string msg, string yes, string no)
        {
            return (await Send(title, msg, yes, no, MapPage.mapPage));
        }

        public static async Task<bool> Send(string title, string msg, string yes, string no,
            Page page)
        {
            return (await page.DisplayAlert(title, msg, yes, no));
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
