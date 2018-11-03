using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RetrieveApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class About : ContentPage
	{

		public About ()
		{
			InitializeComponent ();
            background.Source = App.GetSource("background.png");
            logo.Source = App.GetSource("logo.png");
            word_icon.Source = App.GetSource("logo_word.png");
        }

	}
}