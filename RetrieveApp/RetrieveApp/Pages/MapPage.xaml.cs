using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RetrieveApp.Database;
using RetrieveApp.Elements;
using RetrieveApp.Elements.Card;
using RetrieveApp.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : TabbedPage
	{
        public static Thickness GET_MARGIN;
        private bool clicked = false;
        public static Account _g;
        public static MapPage mapPage;
        private MediaFile file;
        private MapPageEvents events;
        private static Random random = new Random();
        public FilterState current_state = FilterState.ALL;

		public MapPage (Account _d)
		{
            GET_MARGIN = new Thickness(App.ScreenWidth / 20, 0, App.ScreenWidth / 20, 0);
            _g = _d;
            App.CURRENT_PAGE = this;
            mapPage = this;
            InitializeComponent();
            lbl_lg.Text = (_d is Admins) ? ((Admins)_d).SName
             : ((Guests)_d).Name;
                if (_d is Guests)
                {
                    Children.Remove(pg_admin);
                    Children.Remove(pg_b);
                    filter_b.Text = "Reservationer";
                }
            image.Source = App.GetSource("noimage.png");
                var map = new Map(MapSpan.FromCenterAndRadius(
                    new Position(56.05883, 12.7326381), Distance.FromMiles(3)))
                {
                    IsShowingUser = true,
                };
                st.Children.Add(map);
            foreach (IPin pin in IPin.pins)
            {
                map.Pins.Add(pin);
            }
            logout.Clicked += ProcessLogOut;
            events = new MapPageEvents(new LayoutButtons(stk_btns, fl, sc, null),new LayoutButtons(stk_btns2, fl1, sc1, "admin"));
            ReloadAll();
            Designer.DesignPageButtons(stk_btns);
            Designer.DesignPageButtons(stk_btns2);
            filter_all.FontSize = Device.GetNamedSize(NamedSize.Small, filter_all);
            filter_b.FontSize = Device.GetNamedSize(NamedSize.Small, filter_b);
            refr.RefreshCommand = new Command(RCommand);
            refr1.RefreshCommand = new Command(RCommand1);
            filter_all.BackgroundColor = (Color)App.Current.Resources["P_Selected"];
            icon_r.OnFinish += (s, e) => {
                if(icon_r.Animation == "check.json")
                {
                    icon_r.IsVisible = false;
                    icon_r.Pause();
                    icon_r.Loop = true;
                    icon_r.Animation = "trail_loading.json";
                }
            };
            prd_name.HeightRequest = App.ScreenHeight / 16;
            quantity.HeightRequest = App.ScreenHeight / 16;
            price.HeightRequest = App.ScreenHeight / 16;
            oprice.HeightRequest = App.ScreenHeight / 16;
            search.TextChanged += TextChanged;
            search1.TextChanged += TextChanged1;
            search.SearchCommand = new Command(SearchCommand);
            search1.SearchCommand = new Command(SearchCommand1);
        }

        private void OpenGuide(object s, EventArgs args)
        {
            Navigation.PushAsync(new WelcomePage("Guide"));
        }

        private void OpenAbout(object s, EventArgs args)
        {
            Navigation.PushAsync(new About());
        }

        private void PageChanged(object s, EventArgs args)
        {
            var title = CurrentPage.Title;
            if(title == "Reservationer")
            {
                current_state = FilterState.BOOKERS;
                events.rel_admin(search1.Text);
            } else if(title == "Flöde")
            {
                current_state = events.filter;
            }
        }

        private async void RCommand()
        {
            await DBActions.LoadAdmins();
            await DBActions.LoadProducts();
            await DBActions.LoadUsers();
            events.rel(TXT());
            refr.IsRefreshing = false;
        }

        private async void RCommand1()
        {
            await DBActions.LoadProducts();
            await DBActions.LoadUsers();
            events.rel_admin(search1.Text);
            refr1.IsRefreshing = false;
        }

        private void FilterButtonAll(Button s, EventArgs args)
        {
            ChangeFilter(FilterState.ALL);
        }

        private void FilterButtonB(Button s, EventArgs args)
        {
            ChangeFilter(FilterState.B);
        }

        public void ChangeFilter(FilterState state)
        {
            if(events.filter == state)
            {
                return;
            }
            Button b1 = null; Button b2 = null;
            if(state == FilterState.ALL)
            {
                b1 = filter_all;
                b2 = filter_b;
            } else if(state == FilterState.B)
            {
                b2 = filter_all;
                b1 = filter_b;
            }
            b1.BackgroundColor = (Color)App.Current.Resources["P_Selected"];
            b2.BackgroundColor = (Color)App.Current.Resources["P"];
            events.ChangeFilter(state);
            if(state == FilterState.ALL)
            {
                search.Placeholder = "Sök restauranger";
            } else if(state == FilterState.B)
            {
                search.Placeholder = "Sök produkter";
            }
        }

        public void ReloadAll()
        {
            events.rel(TXT());
            if(_g is Admins)
            {
                events.rel_admin(search1.Text);
            }
        }

        public void SearchCommand()
        {
            events.rel(search.Text);
        }

        public void SearchCommand1()
        {
            events.rel_admin(search1.Text);
        }

        private void TextChanged(object s, TextChangedEventArgs a)
        {
            var v = a.NewTextValue;
            if(string.IsNullOrEmpty(v))
            {
                events.rel(v);
            }
        }

        private void TextChanged1(object s, TextChangedEventArgs a)
        {
            var v = a.NewTextValue;
            if (string.IsNullOrEmpty(v))
            {
                events.rel_admin(v);
            }
        }

        public string TXT()
        {
            return search.Text;
        }

        private async void Camera_Clicked(object sender, EventArgs e)
        {
            file = await App.OpenCamera(false);
            SetImage();
        }

        private async void Gallery_Clicked(object s, EventArgs a)
        {
            file = await App.OpenCamera(true);
            SetImage();
        }

        private async void ProcessLogOut(object s, object a)
        {
            bool IS = await App.SendSure(this);
            if (IS)
            {
                Application.Current.Properties.Remove("Logged");
                await Navigation.PushAsync(new WelcomePage("LoginOnly"));
                Navigation.RemovePage(this);
            }
        }

        private async void Btn_add(object s, EventArgs a)
        {
            if(await DBActions.Check(_g as Admins, this))
            {
                return;
            }
            if (clicked)
            {
                return;
            }
            clicked = true;
            var a1 = prd_name.Text; var a2 = oprice.Text; var a3 = price.Text;
            var a4 = quantity.Text; var a5 = tmr.Time; var a6 = desc.Text;
            if(file == null || a1 == null || a2 == null || a3 == null || a4 == null || a5 == null ||
                string.IsNullOrEmpty(a6))
            {
                App.Send("Fel", "Fyll i alla fälten!", "Avbryt");
                clicked = false;
                return;
            }
            try
            {
                double.Parse(a2); double.Parse(a3); int.Parse(a4); 
            } catch(Exception e)
            {
                App.Send("Fel", "Ange ett tal!", "Avbryt");
                clicked = false;
                return;
            }
            icon_r.IsVisible = true;
            icon_r.Play();
            bool f = false;
            int id = 0;
            while (!f)
            {
                id = random.Next(1, 100000000);
                if(DBActions.GetProductById(id) == null)
                {
                    f = true;
                }
            }
            Products product = new Products
            {
                ID = id,
                AdminID = ((Admins)_g).ID,
                PName = a1,
                OldPrice = float.Parse(a2),
                NewPrice = float.Parse(a3),
                Quantity = int.Parse(a4),
                ExpireTime = a5,
                Description = a6
            };
            await DBActions.FullyAddProduct(product, App.ImageToByte(file));
            icon_r.Pause();
            icon_r.Animation = "check.json";
            icon_r.Speed = 0.6f;
            icon_r.Loop = false;
            icon_r.Play();
            ReloadAll();
            ClearInputs();
            clicked = false;
        }

        private void ClearInputs()
        {
            file = null;
            SetImage();
            prd_name.Text = null; oprice.Text = null; price.Text = null; quantity.Text = null;
            tmr.Time = TimeSpan.FromSeconds(0); desc.Text = null;
        }

        private void SetImage()
        {
            if(file != null)
            {
                image.Source = ImageSource.FromStream(() => { return file.GetStream(); });
            } else
            {
                image.Source = App.GetSource("noimage.png");
            }
        }

        public static Page GetPageByName(string title)
        {
            foreach(Page p in mapPage.Children)
            {
                if(p.Title == title)
                {
                    return p;
                }
            }
            return null;
        }
        public static void PinSearch(string txt)
        {
            mapPage.ChangeFilter(FilterState.ALL);
            mapPage.CurrentPage = GetPageByName("Flöde");
            mapPage.search.Text = txt;
            mapPage.events.rel(txt);
        }
        public void OpenProduct(Binary p)
        {
            Navigation.PushAsync(new ProductView(p, current_state));
        }
	}
}