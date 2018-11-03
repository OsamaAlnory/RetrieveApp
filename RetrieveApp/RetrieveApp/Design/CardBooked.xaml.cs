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
		public CardBooked (Binary binary)
		{
            this.binary = binary;
			InitializeComponent ();
		}

        private async void UnbookClicked(object s, EventArgs a)
        {
            if (await App.SendSure())
            {
                await DBActions.Unbook(binary.OWNER, binary.PRODUCT, true);
                MapPage.mapPage.ReloadAll();
            }
        }

        private void ShowProduct(object s, EventArgs a)
        {
            MapPage.mapPage.OpenProduct(binary.PRODUCT);
        }

	}
}