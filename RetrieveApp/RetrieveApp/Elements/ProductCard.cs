using RetrieveApp.Database;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements
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
            public I(string src)
            {
                Source = ImageSource.FromResource("RetrieveApp.Images."+src,
                Assembly.GetExecutingAssembly());
                AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0.5, 0.5, 1, 1));
            }
        }
        private class B : StackLayout
        {
            public B()
            {
                AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
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
        public ProductCard(Products product)
        {
            id = new Random().Next(10000, 50540);
            TapGestureRecognizer tp = new TapGestureRecognizer();
            tp.Tapped += OnTapped;
            Button btn = new Button
            {
            Text = "Click Here\n" + product.PName,
            BackgroundColor = Color.BlueViolet
            };
            v = new ScrollView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Children ={
                    new Label {Text = product.Description+"\nTest"},btn}
                },
                GestureRecognizers = {tp}
            };
            bbb = new B
            {
                IsVisible = false,
                Children ={v}
            };
            b = new B
            {
                Opacity = 0,
                BackgroundColor = Color.LightGray
            };
            bb = new B1()
            {
             Children =
              {
               b,
               bbb
              }
            };
            PRODUCT = product;
            Padding = 0;
            CornerRadius = 30;
            HeightRequest = 300;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Content = new AbsoluteLayout {
                Children =
                {
                    new GradientLayout(true) {
                        ColorsList = "#5eff89,#00c106",
                        Mode = GradientMode.ToBottomRight
                    },
                    new I("github.png")
                    {
                        Aspect = Aspect.AspectFill
                    },
                    new L()
                    {
                        Text = DBActions._p(product).SName,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    bb
                }
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
            if (lastClicked != null && lastClicked.id != id) {
                lastClicked.Sel();
            }
            if (!selected)
            {
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
        public bool IsSelected()
        {
            return selected;
        }

    }
}
