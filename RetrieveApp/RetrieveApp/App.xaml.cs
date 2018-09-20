using RetrieveApp.Database;
using RetrieveApp.Elements;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RetrieveApp
{
    public partial class App : Application
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
        public static Page CURRENT_PAGE;
        public static List<Loadable> loadables = new List<Loadable>();
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoadingPage());
            CURRENT_PAGE = MainPage;
        }

        public static void Register(Loadable page)
        {
            loadables.Add(page);
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
