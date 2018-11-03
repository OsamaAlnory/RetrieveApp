using RetrieveApp.Database;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements.Card
{
    public class Classes
    {

    }

    public class I : Image
    {
        public I()
        {
            Aspect = Aspect.AspectFill;
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0.5, 0.5, 1, 1));
        }
        public void SetSRC(ImageSource src)
        {
            Source = src;
        }
    }
    public class B : StackLayout
    {
        public B()
        {
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
        }
        public B(double a, double b, double c, double d)
        {
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(a, b, c, d));
        }
    }
    public class B1 : AbsoluteLayout
    {
        public B1()
        {
            SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
        }
    }
}
