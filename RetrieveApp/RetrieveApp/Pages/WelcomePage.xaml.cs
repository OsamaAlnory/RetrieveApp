using RetrieveApp.Database;
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

        public WelcomePage()
        {
            InitializeComponent();
            icon.Source = ImageSource.FromResource("RetrieveApp.Images.logo.png",
                Assembly.GetExecutingAssembly());
            btn.Clicked += (e, args) => OnButtonClicked();
            log_btn.Clicked += (e, args) => ProcessLogin();
            crt_btn.Clicked += (e, args) => CreateAccoutClicked();
            animationView.OnFinish += (e, s) => {
                animationView.IsVisible = false;
                    Navigation.PushAsync(new MapPage(ad));
                
            };
            btn.Margin = new Thickness(15, 0, 15, App.ScreenHeight/24);
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
                    if (!animationView.IsPlaying)
                    {
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