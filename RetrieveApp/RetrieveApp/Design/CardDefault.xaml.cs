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
	public partial class CardDefault : StackLayout
	{
        public Products product;
		public CardDefault (Products product)
		{
            this.product = product;
			InitializeComponent ();
            Account acc = MapPage._g;
            if(acc is Guests)
            {
                Guests g = acc as Guests;
                if(!DBActions.hasBooked(g, product))
                {
                    btn.Text = "RESERVERA";
                }
            }
            lbl.Text = product.PName + "\n" + product.Description;
            btn.Clicked += (s, e) => {
                MapPage.mapPage.OpenProduct(product);
            };
		}
	}
}