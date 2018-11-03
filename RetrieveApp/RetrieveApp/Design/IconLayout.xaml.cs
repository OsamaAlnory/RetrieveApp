using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Design
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IconLayout : StackLayout
    {
        public IconLayout(double i1, double i2, double i3, double i4)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(i1,i2,i3,i4));
        }
        public void SetSRC(ImageSource src)
        {
            icon.Source = src;
        }
    }
}