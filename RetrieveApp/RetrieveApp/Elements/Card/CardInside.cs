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
        public static StackLayout GEN(string cardType, Binary b,
            TapGestureRecognizer tap)
        {
            StackLayout card = null;
            if(cardType == "default")
            {
                card = new CardDefault(b.PRODUCT);
            } else if(cardType == "admin")
            {
                card = new CardAdmin(b.PRODUCT);
            } else if(cardType == "admin")
            {
                
            } else if(cardType == "book")
            {

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
