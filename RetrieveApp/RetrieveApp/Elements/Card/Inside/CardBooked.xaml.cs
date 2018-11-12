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
	public partial class CardBooked : StackLayout
	{
        private Binary binary;
        private bool loading = false;
		public CardBooked (Binary binary)
		{
            this.binary = binary;
			InitializeComponent ();
		}

        private async void UnbookClicked(object s, EventArgs a)
        {
            if (loading)
            {
                return;
            }
            if (await App.SendSure())
            {
                loading = true;
                await DBActions.Unbook(binary.OWNER, binary.PRODUCT, true);
                MapPage.mapPage.ReloadAll();
                loading = false;
            }
        }

        private void ShowProduct(object s, EventArgs a)
        {
            MapPage.mapPage.OpenProduct(binary);
        }

	}
}