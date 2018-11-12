using RetrieveApp.Database;
using RetrieveApp.Pages;
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
            admin.Text = binary.PRODUCT.PName;
            var state = MapPage.mapPage.current_state;
            if(state == FilterState.B && MapPage._g is Admins
                && binary.PRODUCT.Quantity < 1)
            {
                zero.IsVisible = true;
                shade.IsVisible = true;
            }
            if(state == FilterState.BOOKERS)
            {
                user.Text = binary.OWNER.Name;
                frame.IsVisible = true;
            }
        }
	}
}