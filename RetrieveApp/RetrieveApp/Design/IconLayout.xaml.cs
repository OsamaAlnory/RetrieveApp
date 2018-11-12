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
        public IconLayout()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0,0,1,1));
        }
        public void SetSRC(ImageSource src)
        {
            icon.Source = src;
        }
    }
}