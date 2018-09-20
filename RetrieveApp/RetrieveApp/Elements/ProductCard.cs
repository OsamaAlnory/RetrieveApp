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
        public Product PRODUCT;
        public ProductCard(Product product)
        {
            PRODUCT = product;
            Padding = 0;
            CornerRadius = 30;
            HeightRequest = 300;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            //BackgroundColor = Color.GreenYellow;
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
                        Text = product.getName(),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                    }
                }
            };
        }

    }
}
