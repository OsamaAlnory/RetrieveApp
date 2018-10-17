using RetrieveApp.Database;
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
        private class L : Label
        {
            public L()
            {
                AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0.5,0.5,1,1));
                FontFamily = Application.Current.Resources["MFont"] as string;
                FontSize = 30;
            }
        }
        private class I : Image
        {
            public I(byte[] b)
            {
                //if(b != null && b.Length > 0)
                {
                    Source = ImageSource.FromResource("RetrieveApp.Images.b2.png",
                    Assembly.GetExecutingAssembly());
                    //Source = App.ByteToImage(b);
                    AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                    AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0.5, 0.5, 1, 1));
                }
            }
        }
        private class B : StackLayout
        {
            public B()
            {
                AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
            }
            public B(double a, double b, double c, double d)
            {
                AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle(a, b, c, d));
            }
        }
        private class B1 : AbsoluteLayout
        {
            public B1()
            {
                SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
            }
        }
        public Products PRODUCT;
        private static ProductCard lastClicked;
        private B b;
        private B1 bb;
        private B bbb;
        private ScrollView v;
        private bool selected;
        private int id;
        private static Random r = new Random();
        public ProductCard(Products product, string productType)
        {
            id = r.Next(1, 50000);
            TapGestureRecognizer tp = new TapGestureRecognizer();
            tp.Tapped += OnTapped;
            v = new CardScrollContent(product,tp,productType);
            bbb = new B{IsVisible = false,Children ={v}};
            b = new B{Opacity = 0,BackgroundColor = Color.LightGray};
            bb = new B1(){Children ={b,bbb}};
            PRODUCT = product;
            Padding = 0;
            CornerRadius = 30;
            HeightRequest = 300;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Content = new AbsoluteLayout {
              Children={
                    new I(PRODUCT.Image){Aspect = Aspect.AspectFill},
                    new B(0.1,0.1,0.1,0.1){
                        Children =
                        {
                            new Image {Source=App.GetSource("sign.png")}
                        }
                    },
                    new L(){
                        Text = DBActions._p(product).SName,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center},bb}
            };
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);
        }
        private void OnTapped(object s, object a)
        {
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
                bbb.IsVisible = false;
                b.FadeTo(0, 400);
            }
            else
            {
                selected = true;
                bbb.IsVisible = true;
                b.FadeTo(0.7, 400);
            }
        }

    }
}
