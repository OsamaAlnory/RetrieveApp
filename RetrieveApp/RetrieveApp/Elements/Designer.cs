using RetrieveApp.Elements.Card;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements
{
    public class Designer
    {
        public static readonly Color[] PAGE_BUTTON_COLORS = { Color.FromHex("#CC51F1") };

        public static void DesignPageButtons(StackLayout layout)
        {
            foreach(View view in layout.Children)
            {
                if(view is Button)
                {
                    Button b = view as Button;
                    b.BackgroundColor = LayoutButtons.BTN_C[0];
                    b.TextColor = Color.White;
                    b.WidthRequest = MapPage.BUTTON_WIDTH;
                    b.FontSize = Device.GetNamedSize(NamedSize.Small, b);
                    b.CornerRadius = 10;
                }
            }
        }

    }
}
