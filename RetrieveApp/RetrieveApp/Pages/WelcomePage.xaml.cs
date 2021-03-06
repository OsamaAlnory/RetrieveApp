﻿using RetrieveApp.Database;
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
        private string state;

        public WelcomePage(string state)
        {
            this.state = state;
            InitializeComponent();
            if(state == "LoginOnly")
            {
                for(int x = 0; x < Children.Count+2; x++)
                {
                    Children.RemoveAt(0);
                }
            } else
            {
                icon.Source = App.GetSource("logo.png");
                btn.Clicked += (e, args) => OnButtonClicked();
                btn1.Clicked += (e, args) => OnButtonClicked2();
                btn2.Clicked += (e, args) => OnButtonClicked3();
                icon1.Source = App.GetSource("logo.png");
                icon2.Source = App.GetSource("map.png");
                icon3.Source = App.GetSource("logo_word.png");
            }
            if(state == "Guide")
            {
                Children.RemoveAt(Children.Count - 1);
                lstbtn.IsVisible = false;
                last_skip.Text = "Stäng";
            }
            log_btn.Clicked += (e, args) => ProcessLogin();
            crt_btn.Clicked += (e, args) => CreateAccoutClicked();
            lstbtn.Clicked += (e, args) => CreateAccoutClicked();
            btn.Margin = new Thickness(15, 0, 15, App.ScreenHeight/24);
            e_name.HeightRequest = App.ScreenHeight / 12;
            e_pass.HeightRequest = App.ScreenHeight / 12;
            fr1.Margin = new Thickness(App.ScreenWidth / 24,
                15, App.ScreenWidth / 24, 0);
            fr.Margin = new Thickness(App.ScreenWidth / 24,
              App.ScreenHeight / 64, App.ScreenWidth / 24, 0);
            e_name.FontSize = IFont.Calc(e_name.HeightRequest);
            e_pass.FontSize = IFont.Calc(e_pass.HeightRequest);
            vnr.Text = "Version: " + App.VERSION;
            laboli.Text = "På kartan kan du se vilka restauranger och cafeer som är anslutna till Retrieve it";
        }

        private void Start()
        {
            animationView.IsVisible = false;
            bool first = false;
            if (ad is Admins)
            {
                if ((ad as Admins).ID == App.ACCOUNT_NAME)
                {
                    Navigation.PushAsync(new ManagePage());
                    return;
                }
                first = !(ad as Admins).Login;
            }
            if (first)
            {
                Navigation.PushAsync(new FirstLogin(ad as Admins));
            }
            else
            {
                Navigation.PushAsync(new MapPage(ad));
            }
            App.RemovePage(this);
        }

        private void OnButtonClicked()
        {
            CurrentPage = Children[1];
        }

        private void OnButtonClicked2()
        {
            CurrentPage = Children[2];
        }
        private void OnButtonClicked3()
        {
            CurrentPage = Children[3];
        }
        private void SkippaClicked(object s, EventArgs args)
        {
            if(state == "Guide")
            {
                Navigation.RemovePage(this);
            } else
            {
                CurrentPage = Children[4];
            }
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
                        Device.StartTimer(TimeSpan.FromSeconds(2), () => {
                            Start();
                            return false;
                        });
                    }
                } else
                {
                    DisplayAlert("Misslyckad Inloggning", "Ogiltigt användernamn eller lösenord!", "Avbryt");
                }
            } else
            {
                DisplayAlert("Misslyckad Inloggning", "Mata in ditt användernamn och lösenord","Okej");
            }
        }
        private void CreateAccoutClicked()
        {
            Navigation.PushAsync(new CreateUserPage());
        }
    }
}