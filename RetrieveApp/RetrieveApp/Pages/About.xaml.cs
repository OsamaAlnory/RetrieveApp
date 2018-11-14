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
            label_main.Text = "Skapades av:\n";
            if (random.Next(1, 3) == 1)
            {
                label_main.Text += "Osama Alnori och Mohanad Oweidat";
            } else
            {
                label_main.Text += "Mohanad Oweidat och Osama Alnori";
            }
            label.Text = "Kontakta oss\n"+
                "E-post:\n"+
                "osama-alnori@outlook.com\n"+
                "mohanad.oweidat@elev.ga.ntig.se\n"+
                "0768307878\n"+
                "Copyrights 2018 © all rights reserved.";
        }

	}
}