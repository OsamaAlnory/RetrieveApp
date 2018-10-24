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
        public Products product;

		public CardAdmin(Products product)
		{
            this.product = product;
			InitializeComponent ();
		}

        private async void RemoveProduct(object s, EventArgs a)
        {
            var A = await App.SendSure(MapPage.mapPage);
            if (A)
            {
                DBActions.RemoveProduct(product);
            }
        }

	}
}