using RetrieveApp.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Elements.Card.Front
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DefaultFrontCard : StackLayout
	{
        private Binary binary;

        public DefaultFrontCard(Binary binary)
        {
            this.binary = binary;
            InitializeComponent();
            //admin.Text = DBActions._p(binary.PRODUCT).SName;
            admin.Text = binary.PRODUCT.PName;
            //user.Text = "Hello there! I like this thing. Hey there";
            frame.IsVisible = false;
        }
	}
}