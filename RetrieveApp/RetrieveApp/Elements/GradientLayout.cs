using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements
{
    public class GradientLayout : StackLayout
    {
        public GradientLayout() { }
        public GradientLayout(bool abs) {
            if (abs)
            {
                AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
            }
        }
        public string ColorsList { get; set; }

        public Color[] Colors
        {
            get
            {
                string[] hex = ColorsList.Split(',');
                Color[] colors = new Color[hex.Length];

                for (int i = 0; i < hex.Length; i++)
                {
                    colors[i] = Color.FromHex(hex[i].Trim());
                }

                return colors;
            }
        }

        public GradientMode Mode { get; set; }
    }
}
