using RetrieveApp.Database;
using RetrieveApp.Design;
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
            StackLayout card = null;
            if(cardType != null && cardType == "admin")
            {
                card = new CardOwner(product);
            } else
            {
                //card = new CardBook(product);
                card = new CardDefault(product);
            }
            StackLayout st = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children ={
                    card
                },
                GestureRecognizers = {tap}
            };
            return st;
        }
    }
}
