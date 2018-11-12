using RetrieveApp.Database;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Design
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CardAdmin : StackLayout
	{
        private Binary binary;
        private bool loading = false;

		public CardAdmin(Binary binary)
		{
            this.binary = binary;
			InitializeComponent ();
		}

        private async void RemoveProduct(object s, EventArgs a)
        {
            if (loading)
            {
                return;
            }
            if (await DBActions.Check(MapPage._g as Admins, MapPage.mapPage))
            {
                return;
            }
            loading = true;
            var A = await App.SendSure(MapPage.mapPage);
            if (A)
            {
                await DBActions.FullyRemoveProduct(binary.PRODUCT);
                MapPage.mapPage.ReloadAll();
            }
            loading = false;
        }

        private async void ShowProduct(object s, EventArgs a)
        {
            if(await DBActions.Check(MapPage._g as Admins, MapPage.mapPage))
            {
                return;
            }
            MapPage.mapPage.OpenProduct(binary);
        }

    }
}