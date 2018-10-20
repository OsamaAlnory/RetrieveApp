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
	public partial class CardOwner : StackLayout
	{
        public Products product;

		public CardOwner (Products product)
		{
            this.product = product;
			InitializeComponent ();
		}
	}
}