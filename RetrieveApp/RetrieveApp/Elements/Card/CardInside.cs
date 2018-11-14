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
            TapGestureRecognizer tap, FilterState state)
        {
            StackLayout card = null;
            if(cardType == "default")
            {
                card = new CardDefault(b);
            } else if(cardType == "admin")
            {
                card = new CardAdmin(b);
            } else if(cardType == "booked")
            {
                card = new CardBooked(b);
            } else if(cardType == "user")
            {
                card = new CardUser(b);
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
