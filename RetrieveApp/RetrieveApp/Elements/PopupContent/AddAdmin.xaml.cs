using RetrieveApp.Database;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Elements.PopupContent
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAdmin : StackLayout
	{
        private Page page;
        public AddAdmin(Page page)
        {
            this.page = page;
            InitializeComponent();
            img.Source = App.GetSource("admin.png");
        }

        private async void OnClicked(object s, EventArgs args)
        {
            var a1 = id.Text; var a2 = pass.Text;
            if (a1 != null && a2 != null)
            {
                if (DBActions.GetAdminById(a1) == null)
                {
                    await DBActions.FullyAddAdmin(new Admins { ID = a1, Password = a2, Login = false });
                    await page.DisplayAlert("Test", "Ja", "Ok");
                    Navigation.PopPopupAsync();
                }
                else
                {
                    page.DisplayAlert("Fel", "Kontot " + a1 + " finns redan!", "Avbryt");
                }
            }
            else
            {
                page.DisplayAlert("Fel", "Ange id och lösenord.", "Avbryt");
            }
        }
    }
}