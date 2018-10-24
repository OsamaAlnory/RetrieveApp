using RetrieveApp.Database;
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
	public partial class CardUser : StackLayout
	{
        public Products product;

		public CardUser (Products product)
		{
            this.product = product;
			InitializeComponent ();
		}

        private void ButtonRetrieved(object s, EventArgs a)
        {
            // Remove product from user
            //DBActions.RemoveProduct(product);
        }

        private void ButtonUnRetrieved(object s, EventArgs a)
        {
            
        }

	}
}