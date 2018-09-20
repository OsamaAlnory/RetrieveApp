using DevsDNA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : CarouselPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            icon.Source = ImageSource.FromResource("RetrieveApp.Images.github.png",
                Assembly.GetExecutingAssembly());
            btn.Clicked += (e, args) => OnButtonClicked();
            login.Clicked += (e, args) => OnClickLogin();
            user.Clicked += (e, args) => ClientLogin();
        }

        private void OnButtonClicked()
        {
            CurrentPage = Children[1];
        }
        private void OnClickLogin()
        {
            Navigation.PushAsync(new LoginPage());
        }

        private void ClientLogin()
        {
            Navigation.PushAsync(new MapPage());
        }
    }
}