using Lottie.Forms;
using RetrieveApp.Database;
using RetrieveApp.Design;
using RetrieveApp.Elements.Card.Front;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements.Card
{
    public class ProductCard : Frame
    {
        public Binary binary;
        private static ProductCard lastClicked;
        private B fader;
        private B1 container;
        private B content;
        private IconLayout icon = new IconLayout(0.1, 0.1, 0.1, 0.1);
        private I product_image = new I();
        private B content_loading;
        private AnimationView animation;
        private ScrollView v;
        private bool selected;
        private int id;
        private static Random r = new Random();
        private bool LOADING = true;
        public ProductCard(Binary binary, string cardType)
        {
            id = r.Next(9999);
            this.binary = binary;
            animation = new AnimationView {
                Loop = true, AutoPlay = true, Animation="loading.json",
                HeightRequest=50, WidthRequest=50
            };
            TapGestureRecognizer tp = new TapGestureRecognizer();
            tp.Tapped += OnTapped;
            v = new CardScrollContent(binary,tp,cardType);
            content = new B{IsVisible = false,Children ={v}};
            fader = new B{Opacity = 0,BackgroundColor = Color.LightGray};
            container = new B1(){Children ={fader,content}};
            content_loading = new B(0, 0, 1, 1)
            {
                Children = {new StackLayout{Children={
                new Label { Text="Hämtar",FontAttributes=FontAttributes.Bold,
                FontSize=30,
                TextColor =Color.White},animation},
                HorizontalOptions =LayoutOptions.CenterAndExpand,VerticalOptions=
                LayoutOptions.CenterAndExpand},},
                BackgroundColor=Color.LightGray
            };
            Padding = 0;
            CornerRadius = 30;
            HeightRequest = 300;
            IsClippedToBounds = true;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Content = new AbsoluteLayout {
              Children={
                    product_image,
                    icon,new DefaultFrontCard(binary)
                    /*
                    new L(){
                        Text = DBActions._p(binary.PRODUCT).SName,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center}*/,container,
                content_loading}
            };
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);
            LoadContent();
        }
        private async void LoadContent()
        {
            ImageSource src = null;
            ImageSource src1 = null;
            AdminIcon ai = await DBActions.LoadAdminIcon(DBActions._p(binary.PRODUCT));
            Images img = await DBActions.LoadProductImage(binary.PRODUCT);
            if(ai != null)
            {
                src = App.ByteToImage(ai.Image);
            } else
            {
                src = App.GetSource("icon-blank.png");
            }
            if(img != null)
            {
                src1 = App.ByteToImage(img.Image);
            } else
            {
                src1 = App.GetSource("noproduct.jpg");
            }
            icon.SetSRC(src);
            product_image.SetSRC(src1);
            content_loading.IsVisible = false;
            animation.Pause();
            LOADING = false;
        }
        private void OnTapped(object s, object a)
        {
            if (LOADING)
            {
                return;
            }
            if (selected)
            {
                lastClicked = null;
            }
            else
            {
                if (lastClicked != null && lastClicked.id != id)
                {
                    lastClicked.Sel();
                }
                lastClicked = this;
            }
            Sel();
        }
        public void Sel()
        {
            if (selected)
            {
                selected = false;
                content.IsVisible = false;
                fader.FadeTo(0, 400);
            }
            else
            {
                selected = true;
                content.IsVisible = true;
                fader.FadeTo(0.7, 400);
            }
        }

    }
}
