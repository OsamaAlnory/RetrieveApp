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
        public static LoadingPage page;

		public LoadingPage ()
		{
            page = this;
			InitializeComponent ();
            App.Register(this);
            state.Text = "Loading";
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                if (App.CheckInternetConnection())
                {
                    DBActions.GetAccounts();
                } else
                {
                    Crash("Appen kunde inte startas!\n\nKolla ditt nätverk.");
                }
                return false;
            });
		}

        private async void Crash(string reason)
        {
            await DisplayAlert("Fel", reason, "Avbryt");
            var closer = DependencyService.Get<ICloseApp>();
            closer?.close();
        }

        public void OnLoadStarted(string type)
        {
            if (type == "Users")
            {
                load.IsVisible = true;
                load.Play();
            } else if (type == "Products") {
                state.Text = "Laddar produkter";
            } else if (type == "Pins")
            {
                state.Text = "Laddar pins";
            }
        }

        public void OnLoadFinished(string type)
        {
            if(type == "Pins")
            {
                load.Pause();
                load.IsVisible = false;
                Navigation.PushAsync(new WelcomePage("Default"));
                //Navigation.PushAsync(new MapPage(DBActions.admins[0]));
                //Navigation.PushAsync(new MapPage(DBActions.guests[0]));
                //Navigation.PushAsync(new ProductView(DBActions.products[0]));
                /*
                if(Application.Current.Properties.ContainsKey("Logged"))
                {
                    int Id = (int)Application.Current.Properties["Logged"];
                    Navigation.PushAsync(new MapPage(DBActions.GetUserById(Id)));
                }
                else
                {
                    Navigation.PushAsync(new WelcomePage("Default"));
                }*/
                Navigation.RemovePage(this);
            }
        }

        public void Quit()
        {
            Crash("App is in dev mode!");
        }

    }
}