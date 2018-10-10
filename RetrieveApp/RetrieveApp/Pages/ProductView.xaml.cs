using RetrieveApp.Database;
using RetrieveApp.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductView : ContentPage, Loadable
	{
        private Products product;
        private bool loading;

		public ProductView (Products product)
		{
            this.product = product;
			InitializeComponent ();
            if(product.Image != null)
            {
                img.Source = App.ByteToImage(product.Image);
            } else
            {
                img.Source = ImageSource.FromResource(App.PATH + "noimage.png");
            }
            /*
            if(MapPage._g is Admins)
            {
                btn.IsVisible = false;
            }
             */
            for (int x = 0; x < product.Quantity; x++)
            {
                qu.Items.Add(""+(x+1));
            }
            qu.SelectedIndex = 0;
            tid.Text = product.ExpireTime.ToString();
            btn.Clicked += async (s, e) => {
                if (!loading)
                {
                    loading = true;
                    anim.IsVisible = true;
                    anim.Play();
                    App.StartLoading("Confirm");
                    // Reload products
                    Guests g = MapPage._g as Guests;
                    if(!DBActions.hasBooked(g, product))
                    {
                        // Put in cart
                    } else
                    {
                        DisplayAlert("Error", "You've already booked this!", "Cancel");
                    }
                    //App.FinishLoading("Confirm");
                }
            };
		}

        public void OnLoadFinished(string type)
        {
            if(type == "Confirm")
            {
                anim.Pause();
                anim.IsVisible = false;
            }
        }

        public void OnLoadStarted(string type){}
    }
}