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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            btn.Clicked += (e, s) => OnClicked();
            animationView.OnFinish += (e, s) => {
                animationView.IsVisible = false;
            };
            e_user.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);
            //e_user.TextChanged += (e, s) => {
                //e_user.Text = e_user.Text.ToUpper();
                /*
                var a = s.OldTextValue; var b = s.NewTextValue;
                if(a != null && b != null)
                {
                    if (a.Length == 3 && b.Length == 4)
                    {
                        e_user.Text += "-";
                    }
                }
                */
            //};
		}

        private void OnClicked()
        {
            animationView.IsVisible = true;
            animationView.Play();
        }

	}
}