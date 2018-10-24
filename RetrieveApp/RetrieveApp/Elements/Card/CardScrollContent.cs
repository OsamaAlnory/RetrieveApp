using RetrieveApp.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements.Card
{
    class CardScrollContent : ScrollView
    {
        public CardScrollContent(Binary b, TapGestureRecognizer tap, string cardType)
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            Padding = 10;
            Content = CardInside.GEN(cardType, b, tap);
        }
    }
}
