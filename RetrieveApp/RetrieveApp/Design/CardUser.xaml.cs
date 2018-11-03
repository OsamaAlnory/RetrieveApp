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

		public CardUser (Binary binary)
		{
            this.binary = binary;
			InitializeComponent ();
		}

        private async void ButtonRetrieved(object s, EventArgs a)
        {
            await DBActions.Unbook(binary.OWNER, binary.PRODUCT, false);
            MapPage.mapPage.ReloadAll();
        }

        private async void ButtonUnRetrieved(object s, EventArgs a)
        {
            await DBActions.Unbook(binary.OWNER, binary.PRODUCT, true);
            MapPage.mapPage.ReloadAll();
        }

	}
}