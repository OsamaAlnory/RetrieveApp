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
        private Binary b;
        private bool loading;

		public ProductView (Binary b, FilterState state)
		{
            App.Register(this);
            this.b = b;
			InitializeComponent ();
            Load();
            var logger = MapPage._g;
            if(logger is Admins)
            {
                btn.IsVisible = false;
                amount.IsVisible = false;
                amount_admin.IsVisible = true;
            }
            for (int x = 0; x < b.PRODUCT.Quantity; x++)
            {
                qu.Items.Add(""+(x+1));
            }
            if((logger is Admins && state != FilterState.BOOKERS) ||
                (state == FilterState.ALL && logger is Guests && 
                DBActions.hasBooked(logger as Guests, b.PRODUCT)))
            {
                amount.IsVisible = false;
                amount_admin.IsVisible = true;
            }
            qu.SelectedIndex = 0;
            var admin = DBActions._p(b.PRODUCT);
            seller.Text = admin.SName;
            address.Text = admin.Address;
            email.Text = admin.Email;
            mobile.Text = admin.Phone;
            p_o.Text = b.PRODUCT.OldPrice + " kr";
            p_n.Text = b.PRODUCT.NewPrice + " kr";
            lbl_name.Text = b.PRODUCT.PName;
            lbl_desc.Text = b.PRODUCT.Description;
            kvar.Text = b.PRODUCT.Quantity+"";
            var v1 = b.PRODUCT.ExpireTime.Hours+"";
            var v2 = b.PRODUCT.ExpireTime.Minutes + "";
            if (state == FilterState.BOOKERS || (state == FilterState.B &&
                logger is Guests))
            {
                amount.IsVisible = false;
                antal.Text = "Antal:";
                kvar.Text = b.QUANTITY+"";
                amount_admin.IsVisible = true;
            }
            if (b.PRODUCT.ExpireTime.Hours < 10)
            {
                v1 = "0" + v1;
            }
            if(b.PRODUCT.ExpireTime.Minutes < 10)
            {
                v2 = "0" + v2;
            }
            tid.Text = v1 + ":" + v2;
            Account acc = MapPage._g;
            if(acc is Guests)
            {
                if(DBActions.hasBooked(acc as Guests, b.PRODUCT))
                {
                    btn.BackgroundColor = Color.Crimson;
                    btn.Text = "Avboka";
                }
            } else
            {
                btn.IsVisible = false;
            }
            btn.Clicked += async (s, e) => {
                if (loading)
                {
                    return;
                }
                    loading = true;
                    anim.IsVisible = true;
                    anim.Play();
                    App.StartLoading("Confirm");
                    Guests g = acc as Guests;
                    if(!DBActions.hasBooked(g, b.PRODUCT))
                    {
                        int Q = int.Parse(qu.SelectedItem.ToString());
                        await DBActions.book(g, b.PRODUCT, Q);
                        anim.IsVisible = false;
                        anim.Pause();
                        MapPage.mapPage.ReloadAll();
                        await DisplayAlert("Info","Du reserverade "+Q+" stycken av "
                            + b.PRODUCT.PName+"!\n"+"Du sparade "+
                            ((b.PRODUCT.OldPrice- b.PRODUCT.NewPrice)*Q)+" kr!","Ok");
                        App.RemovePage(this);
                    } else
                    {
                        if (await App.SendSure())
                        {
                            loading = true;
                            await DBActions.Unbook(g, b.PRODUCT, true);
                            MapPage.mapPage.ReloadAll();
                            App.RemovePage(this);
                        }
                    }
                    //App.FinishLoading("Confirm");
            };
		}

        private async void Load()
        {
            Images images = await DBActions.LoadProductImage(b.PRODUCT);
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