using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp
{
    public class ITasks
    {
        
        public static void Centerize(Layout<View> layout)
        {
            foreach(View v in layout.Children)
            {
                if(v is Entry e)
                {
                    e.HorizontalTextAlignment = TextAlignment.Center;
                }
            }
        }

    }
}
