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
        private static Random random = new Random();

		public About ()
		{
			InitializeComponent ();
            background.Source = App.GetSource("background1.png");
            logo.Source = App.GetSource("logo.png");
            word_icon.Source = App.GetSource("logo_word.png");


            label1.Text = "Skapades Av\n" +

          "Retrieve it Uf\n";


            label_main.Text = "App Tillverkare:\n";
            if (random.Next(1, 3) == 1)
            {
                label_main.Text += "Mohanad Oweidat och Osama Alnori";
            }
            else
            {
                label_main.Text += "Osama Alnori och Mohanad Oweidat";
            }

            
            label.Text = "Kontakta oss\n"+
              "E-post:\n"+
              "retrieveituf@gmail.com\n"+
              "Copyrights 2018 © all rights reserved";
        }

	}
}