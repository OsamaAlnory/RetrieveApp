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
	public partial class EditAdmin : StackLayout
	{
        private Page page;
        private Admins admin;
        private bool clicked = false;
		public EditAdmin (Page page)
		{
            this.page = page;
			InitializeComponent ();
            img.Source = App.GetSource("admin.png");
            search.SearchCommand = new Command(OnSearch);
		}

        private void OnSearch()
        {
            var text = search.Text;
            if(text != null)
            {
                var ad = DBActions.GetAdminById(text);
                if(ad != null)
                {
                    if(ad.ID == App.ACCOUNT_NAME)
                    {
                        return;
                    }
                    admin = ad;
                    stack.IsVisible = true;
                } else
                {
                    stack.IsVisible = false;
                    App.Send("Fel", "Kunde inte hitta kontot med ID: "+text+"!", "Avbryt");
                }
            } else
            {
                App.Send("Fel", "Ange admin id!","Avbryt");
            }
        }

        private async void ButtonRemove(object s, EventArgs args)
        {
            if (clicked)
            {
                return;
            }
            if(admin != null)
            {
                clicked = true;
                var b = await App.Send("Meddelande", 
                    "Vill du också radera alla tillhörande produkter?", "Ja", "Nej");
                await DBActions.FullyRemoveAdmin(admin);
                if (b)
                {
                    var a = DBActions._a(admin);
                    foreach(Products p in a)
                    {
                        await DBActions.FullyRemoveProduct(p);
                    }
                }
                search.Text = null;
                App.Send("Info", "Kontot med ID: "+admin.ID+" har raderats!", "Ok");
                stack.IsVisible = false;
                clicked = false;
            }
        }

        private async void ButtonReset(object s, EventArgs args)
        {
            if (clicked)
            {
                return;
            }
            clicked = true;
            if(admin != null)
            {
                admin.Login = false;
                await DBActions.FullyEditAdmin(admin);
                search.Text = null;
                App.Send("Info", "Kontot med ID: "+admin.ID+" har återställts!", "Ok");
                stack.IsVisible = false;
                clicked = false;
            }
        }

	}
}