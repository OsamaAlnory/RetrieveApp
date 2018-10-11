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
	public partial class CreateUserPage : ContentPage, Loadable
	{
        private bool created;
        private const int MIN_U_LENGTH = 6;
        private const int MAX_U_LENGTH = 20;
        private const int MIN_P_LENGTH = 6;
        private const int MAX_P_LENGTH = 24;

		public CreateUserPage ()
		{
			InitializeComponent ();
            App.Register(this);
            e_name.HeightRequest = App.ScreenHeight / 13;
            e_pass.HeightRequest = App.ScreenHeight / 13;
            fr.Margin = new Thickness(App.ScreenWidth/24,
                App.ScreenHeight/52, App.ScreenWidth/24, 0);
            fr1.Margin = new Thickness(App.ScreenWidth/24,
                0,App.ScreenWidth/24,0);
            crt_acc.Margin = fr.Margin;
            e_name.FontSize = IFont.Calc(e_name.HeightRequest);
            e_pass.FontSize = IFont.Calc(e_pass.HeightRequest);
        }

        public void OnLoadStarted(string type)
        {
            an.IsVisible = true;
            an.Play();
        }

        public void OnLoadFinished(string type)
        {
            an.Loop = false;
            an.Animation = "done_button.json";
            an.HeightRequest = 160;
            an.WidthRequest = 160;
            an.Speed = 0.5f;
            an.Play();
            an.OnFinish += (s, e) => {
                Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                    //await DisplayAlert("Success", "Kontot skapades!", "Ok");
                    an.IsVisible = false;
                    Navigation.PushAsync(new WelcomePage("LoginOnly"));
                    Navigation.RemovePage(this);
                    return false;
                });
            };
        }

        private async void Button_Create_Clicked(object s, EventArgs a)
        {
            if (created)
            {
                return;
            }
            string n = e_name.Text;
            string p = e_pass.Text;
            if(n != null && p != null)
            {
                if (!(n.Length >= MIN_U_LENGTH && n.Length <= MAX_U_LENGTH))
                {
                    DisplayAlert("Fel", "Användarnamn", "Ok");
                    return;
                }
                if (!(p.Length >= MIN_P_LENGTH && p.Length <= MAX_P_LENGTH))
                {
                    DisplayAlert("Fel", "Lösenord", "Ok");
                    return;
                }
                created = true;
                App.StartLoading("Register");
                await DBActions.LoadAccounts();
                bool found = true;
                foreach(Admins admin in DBActions.admins)
                {
                    if(admin.ID == n)
                    {
                        found = false;
                        break;
                    }
                }
                foreach(Guests guest in DBActions.guests)
                {
                    if(guest.Name == n)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    await DBActions.AddUser(new Guests {
                        Name = n, Password = p
                    });
                    await DBActions.LoadAccounts();
                } else
                {
                    created = false;
                    DisplayAlert("Fel", "Kontot finns redan!", "Ok");
                }
                App.FinishLoading("Register");
                return;
            } else
            {
                DisplayAlert("Fel", "Ange namn och lösenord.", "Ok");
            }
            created = false;
        }
	}
}