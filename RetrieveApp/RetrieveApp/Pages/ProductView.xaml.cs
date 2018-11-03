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

		public ProductView (Products product, string type)
		{
            App.Register(this);
            this.product = product;
			InitializeComponent ();
            img_main.Source = App.GetSource("background.png");
            Load();
            if(MapPage._g is Admins)
            {
                btn.IsVisible = false;
                amount.IsVisible = false;
                amount_admin.IsVisible = true;
            }
            for (int x = 0; x < product.Quantity; x++)
            {
                qu.Items.Add(""+(x+1));
            }
            if(type == "b")
            {
                amount.IsVisible = false;
                amount_admin.IsVisible = true;
            }
            qu.SelectedIndex = 0;
            var admin = DBActions._p(product);
            seller.Text = admin.SName;
            address.Text = admin.Address;
            email.Text = admin.Email;
            mobile.Text = admin.Phone;
            p_o.Text = product.OldPrice + " kr";
            p_n.Text = product.NewPrice + " kr";
            lbl_name.Text = product.PName;
            lbl_desc.Text = product.Description;
            kvar.Text = product.Quantity+"";
            var v1 = product.ExpireTime.Hours+"";
            var v2 = product.ExpireTime.Minutes + "";
            if(product.ExpireTime.Hours < 10)
            {
                v1 = "0" + v1;
            }
            if(product.ExpireTime.Minutes < 10)
            {
                v2 = "0" + v2;
            }
            tid.Text = v1 + ":" + v2;
            Account acc = MapPage._g;
            if(acc is Guests)
            {
                if(DBActions.hasBooked(acc as Guests, product))
                {
                    btn.BackgroundColor = Color.Crimson;
                    btn.Text = "Avboka";
                }
            } else
            {
                btn.IsVisible = false;
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
                        await DBActions.book(g, product, Q);
                        anim.IsVisible = false;
                        anim.Pause();
                        MapPage.mapPage.ReloadAll();
                        await DisplayAlert("Success","You've booked "+Q+" of "
                            +product.PName+"!\n"+"You've saved "+
                            ((product.OldPrice-product.NewPrice)*Q)+" kr!","Ok");
                        App.RemovePage(this);
                    } else
                    {
                        if (await App.SendSure())
                        {
                            //await DBActions.Unbook(binary.OWNER, binary.PRODUCT, true);
                            MapPage.mapPage.ReloadAll();
                        }
                    }
                    //App.FinishLoading("Confirm");
                }
            };
		}

        private async void Load()
        {
            Images images = await DBActions.LoadProductImage(product);
            if (images != null)
            {
                img.Source = App.ByteToImage(images.Image);
            }
            else
            {
                img.Source = App.GetSource("noimage.png");
            }
            image_loading.Pause();
            image_an_layout.IsVisible = false;
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