using RetrieveApp.Database;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements.Card
{
    public class ICardDefault
    {
        protected Products product;

        public ICardDefault(Products product)
        {
            this.product = product;
        }

        public virtual StackLayout getContent()
        {
            Button btn = new Button
            {
                Text = MapPage._g is Admins ? "VISA" : "RESERVERA",
                BackgroundColor = Color.BlueViolet,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.White, CornerRadius=5
            };
            btn.Clicked += (s, e) => {
                MapPage.mapPage.OpenProduct(product);
            };
            return new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    new Label {Text = product.PName+"\n"+product.Description,
                    HorizontalTextAlignment=TextAlignment.Center,
                    TextColor=Color.Black},btn}
            };
        }

    }
}
