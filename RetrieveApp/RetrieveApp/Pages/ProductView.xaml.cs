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
            App.Register(this);
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
            p_o.Text = product.OldPrice + " kr";
            p_n.Text = product.NewPrice + " kr";
            lbl_name.Text = product.PName;
            lbl_desc.Text = product.Description;
            tid.Text = product.ExpireTime.ToString();
            Account acc = MapPage._g;
            if(acc is Guests)
            {
                if(DBActions.hasBooked(acc as Guests, product))
                {
                    btn.BackgroundColor = Color.Crimson;
                    btn.Text = "Avboka";
                }
            }
            btn.Clicked += async (s, e) => {
                if (!loading)
                {
                    loading = true;
                    anim.IsVisible = true;
                    anim.Play();
                    App.StartLoading("Confirm");
                    // Reload products
                    Guests g = acc as Guests;
                    if(!DBActions.hasBooked(g, product))
                    {
                        int Q = int.Parse(qu.SelectedItem.ToString());
                        DBActions.book(g, product, Q);
                        await DisplayAlert("Success","You've booked "+Q+" of "
                            +product.PName+"!\n"+"You've saved "+
                            ((product.OldPrice-product.NewPrice)*Q)+" kr!","Ok");
                        Navigation.RemovePage(this);
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