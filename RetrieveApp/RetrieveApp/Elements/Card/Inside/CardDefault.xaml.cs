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
        public Binary b;
        private bool loading = false;

		public CardDefault (Binary b)
		{
            this.b = b;
			InitializeComponent ();
            Account acc = MapPage._g;
            if(acc is Guests)
            {
                Guests g = acc as Guests;
                if(!DBActions.hasBooked(g, b.PRODUCT))
                {
                    btn.Text = "RESERVERA";
                }
            }
            lbl.Text = b.PRODUCT.Description;
            btn.Clicked += async (s, e) => {
                if (loading)
                {
                    return;
                }
                if(MapPage._g is Admins)
                {
                    if (await DBActions.Check(MapPage._g as Admins, MapPage.mapPage))
                    {
                        return;
                    }
                }
                loading = true;
                await DBActions.LoadProducts();
                await DBActions.LoadUsers();
                var prds = DBActions.GetProductById(b.PRODUCT.ID);
                if(prds != null && prds.Quantity > 0 && 
                DBActions.GetAdminById(prds.AdminID) != null)
                {
                    MapPage.mapPage.OpenProduct(b);
                } else
                {
                    App.Send("Info", "Produkten finns inte längre!", "Ok");
                    if(DBActions.GetAdminById(prds.AdminID) == null)
                    {
                        await DBActions.CheckProducts();
                    }
                    MapPage.mapPage.ReloadAll();
                }
                loading = false;
            };
		}
	}
}