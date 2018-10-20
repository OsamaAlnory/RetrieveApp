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
            btn.Text = MapPage._g is Admins ? "VISA" : "RESERVERA";
            lbl.Text = product.PName + "\n" + product.Description;
            btn.Clicked += (s, e) => {
                MapPage.mapPage.OpenProduct(product);
            };
		}
	}
}