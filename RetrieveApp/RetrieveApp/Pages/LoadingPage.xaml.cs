using RetrieveApp.Database;
using RetrieveApp.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPage : ContentPage, Loadable
	{
		public LoadingPage ()
		{
			InitializeComponent ();
            App.Register(this);
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                if (App.CheckInternetConnection())
                {
                    DBActions.GetAccounts();
                } else
                {
                    DisplayAlert("Fel", "Appen kunde inte startas!\n\nKolla ditt nätverk.", "Avbryt");
                }
                return false;
            });
		}

        public void OnLoadStarted(string type)
        {
            if (type == "Users")
            {
                load.IsVisible = true;
                load.Play();
            }
        }

        public void OnLoadFinished(string type)
        {
            if(type == "Users")
            {
                load.Pause();
                load.IsVisible = false;
                Navigation.PushAsync(new WelcomePage());
            }
        }

    }
}