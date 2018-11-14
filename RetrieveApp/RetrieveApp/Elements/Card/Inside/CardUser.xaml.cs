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
	public partial class CardUser : StackLayout
	{
        private Binary binary;
        private bool loading = false;

		public CardUser (Binary binary)
		{
            this.binary = binary;
			InitializeComponent ();
            btn_u.Clicked += ButtonUnRetrieved;
		}

        private async void ShowProduct(object s, EventArgs args)
        {
            if(await DBActions.Check(MapPage._g as Admins, MapPage.mapPage))
            {
                return;
            }
            MapPage.mapPage.OpenProduct(binary);
        }

        private async void ButtonRetrieved(object s, EventArgs a)
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
            await DBActions.Check(MapPage._g as Admins, MapPage.mapPage);
            await DBActions.Unbook(binary.OWNER, binary.PRODUCT, false);
            MapPage.mapPage.ReloadAll();
            loading = false;
        }

        private async void ButtonUnRetrieved(object s, EventArgs args)
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
            await DBActions.Unbook(binary.OWNER, binary.PRODUCT, true);
            MapPage.mapPage.ReloadAll();
            loading = false;
        }

	}
}