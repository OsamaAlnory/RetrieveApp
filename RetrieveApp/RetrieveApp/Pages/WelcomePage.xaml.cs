using RetrieveApp.Database;
using RetrieveApp.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : CarouselPage
    {
        private Account ad;
        private bool signed;

        public WelcomePage(string state)
        {
            InitializeComponent();
            if(state == "LoginOnly")
            {
                for(int x = 0; x < Children.Count-1; x++)
                {
                    Children.RemoveAt(x);
                }
            } else
            {
                icon.Source = ImageSource.FromResource(App.PATH + "logo.png",
                 Assembly.GetExecutingAssembly());
                btn.Clicked += (e, args) => OnButtonClicked();
            }
            log_btn.Clicked += (e, args) => ProcessLogin();
            crt_btn.Clicked += (e, args) => CreateAccoutClicked();
            animationView.OnFinish += (e, s) => {
                animationView.IsVisible = false;
                Navigation.PushAsync(new MapPage(ad));
                Navigation.RemovePage(this);
            };
            btn.Margin = new Thickness(15, 0, 15, App.ScreenHeight/24);
            e_name.HeightRequest = App.ScreenHeight / 12;
            e_pass.HeightRequest = App.ScreenHeight / 12;
            fr1.Margin = new Thickness(App.ScreenWidth / 24,
                15, App.ScreenWidth / 24, 0);
            fr.Margin = new Thickness(App.ScreenWidth / 24,
              App.ScreenHeight / 64, App.ScreenWidth / 24, 0);
            e_name.FontSize = IFont.Calc(e_name.HeightRequest);
            e_pass.FontSize = IFont.Calc(e_pass.HeightRequest);
        }

        private void OnButtonClicked()
        {
            CurrentPage = Children[1];
        }
        private void ProcessLogin()
        {
            string u = e_name.Text;
            string p = e_pass.Text;
            if(u != null && p != null)
            {
                bool found = false;
                foreach(Admins acc in DBActions.admins)
                {
                    if(u == acc.ID && p == acc.Password)
                    {
                        found = true;
                        ad = acc;
                    }
                }
                foreach(Guests g in DBActions.guests)
                {
                    if(u == g.Name && p == g.Password)
                    {
                        found = true;
                        ad = g;
                    }
                }
                if (found)
                {
                    if (!signed)
                    {
                        signed = true;
                        if(ad is Guests)
                        {
                            Application.Current.Properties["Logged"] = ((Guests)ad).ID;
                            Application.Current.SavePropertiesAsync();
                        }
                        animationView.IsVisible = true;
                        animationView.Play();
                    }
                } else
                {
                    DisplayAlert("Login Error", "Invalid username or password!", "cancel");
                }
            } else
            {
                DisplayAlert("Login Error","Enter your username and password","ok");
            }
        }
        private void CreateAccoutClicked()
        {
            Navigation.PushAsync(new CreateUserPage());
        }
    }
}