using RetrieveApp.Database;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements.Card
{
    public class ICardAdmin
    {
        private Products product;
        public StackLayout getContent()
        {
            Button btn = new Button
            {
                Text = "VISA",
                BackgroundColor = Color.BlueViolet,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.White
            };
            Button btn_del = new Button
            {
                Text = "Ta bort",
                BackgroundColor = Color.Red,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.White
            };
            btn.Clicked += (s, e) => {
                MapPage.mapPage.OpenProduct(product);
            };
            btn_del.Clicked += async (s, e) => {
                var allert = await MapPage.mapPage.DisplayAlert("Warning", "Are you sure?",
                    "Delete", "Cancel");
                if(allert == true)
                {
                    DBActions.RemoveProduct(product);
                }
            };
            return new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    new Label {Text = product.PName+"\n"+product.Description,
                    HorizontalTextAlignment=TextAlignment.Center,
                    TextColor=Color.Black},btn,btn_del}
            };
        }
    }
}
