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
        private int STAGES = 5;
        private int CURRENT_STAGE = 0;

		public LoadingPage ()
		{
            page = this;
			InitializeComponent ();
            App.Register(this);
            state.Text = "Hämtar data";
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (App.CheckInternetConnection())
                    {
                        DBActions.LoadData();
                    }
                    else
                    {
                        Crash("Appen kunde inte startas!\n\nSe till att din mobil är" +
                        " ansluten till nätverket.");
                    }
                    return false;
                });
		}

        private async void Crash(string reason)
        {
            Crash(reason, this);
        }

        public static async void Crash(string reason, Page page)
        {
            await page.DisplayAlert("Fel", reason, "Avbryt");
            var closer = DependencyService.Get<ICloseApp>();
            closer?.close();
        }

        public void OnLoadStarted(string type)
        {
            if (type == "Users")
            {
                SetText();
                load.IsVisible = true;
                load.Play();
                //progress.IsVisible = true;
            }
        }

        private void SetText()
        {
            double p = CURRENT_STAGE / ((double)STAGES)*100.0;
            //progress.ProgressTo(p / 100, 250, Easing.Linear);
            state.Text = "Hämtar data "+p+"%";
        }

        public async void OnLoadFinished(string type)
        {
            if(type == "Users")
            {
                CURRENT_STAGE++;
                SetText();
            }
            if(type == "Admins")
            {
                CURRENT_STAGE++;
                SetText();
            }
            if(type == "Products")
            {
                CURRENT_STAGE++;
                SetText();
            }
            if(type == "Pins")
            {
                CURRENT_STAGE++;
                SetText();
            }
            if(type == "Fixing")
            {
                CURRENT_STAGE++;
                SetText();
                load.Pause();
                load.IsVisible = false;
                if(Application.Current.Properties.ContainsKey("Logged"))
                {
                    int Id = (int)Application.Current.Properties["Logged"];
                    Navigation.PushAsync(new MapPage(DBActions.GetUserById(Id)));
                }
                else
                {
                    Navigation.PushAsync(new WelcomePage("Default"));
                }/**/
                App.RemovePage(this);
            }
        }

        public void Quit()
        {
            Crash("App is in dev mode!");
        }

    }
}