using RetrieveApp.Elements;
using RetrieveApp.Elements.PopupContent;
using Rg.Plugins.Popup.Extensions;
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
	public partial class ManagePage : ContentPage
	{
		public ManagePage ()
		{
			InitializeComponent ();
            img.Source = App.GetSource("background.png");
            icon.Source = App.GetSource("logo.png");
		}
        private void OnClicked1(object s, EventArgs args)
        {
            ShowPopup(new Popup(new AddAdmin(this), this));
        }

        private void OnClicked2(object s, EventArgs args)
        {
            ShowPopup(new Popup(new EditAdmin(this), this));
        }

        private async void LogOut(object s, EventArgs args)
        {
            if(await App.SendSure(this))
            {
                await App.LogOut(this);
            }
        }

        private async void ShowPopup(Popup page)
        {
            // Open a PopupPage
            await Navigation.PushPopupAsync(page);
            // Close the last PopupPage int the PopupStack
            //await Navigation.PopPopupAsync();

            // Close all PopupPages in the PopupStack
            //await Navigation.PopAllPopupAsync();

            // Close an one PopupPage in the PopupStack even if the page is not the last
            //await Navigation.RemovePopupPageAsync(page);
        }

    }
}