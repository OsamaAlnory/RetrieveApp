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
            state.Text = "Loading";
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                if (App.CheckInternetConnection())
                {
                    DBActions.GetAccounts();
                } else
                {
                    DisplayAlert("Fel", "Appen kunde inte startas!\n\nKolla ditt nätverk.", "Avbryt");
                    var closer = DependencyService.Get<ICloseApp>();
                    closer?.close();
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
            } else if (type == "Products") {
                state.Text = "Loading products";
            } else if (type == "Pins")
            {
                state.Text = "Loading pins";
            }
        }

        public void OnLoadFinished(string type)
        {
            if(type == "Pins")
            {
                load.Pause();
                load.IsVisible = false;
                //Navigation.PushAsync(new WelcomePage());
                Navigation.PushAsync(new MapPage(DBActions.admins[0]));
            }
        }

    }
}