using RetrieveApp.Database;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements.Card
{
    class CardInside
    {
        public static StackLayout GEN(string cardType, Products product,
            TapGestureRecognizer tap)
        {
            ICardDefault card = null;
            if(cardType != null && cardType == "admin")
            {
                card = new ICardAdmin(product);
            } else
            {
                card = new ICardDefault(product);
            }
            StackLayout st = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children ={
                    card.getContent()
                },
                GestureRecognizers = {tap}
            };
            return st;
        }
    }
}
