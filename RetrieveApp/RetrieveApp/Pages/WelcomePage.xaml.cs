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
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            tst.Clicked += (e, sender) =>OnButtonClicked();
        }

        private void OnButtonClicked()
        {
            DisplayAlert("Fuck", "Fucking 6zrk", "5raconic");
        }
    }
}