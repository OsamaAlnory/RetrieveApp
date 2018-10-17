using RetrieveApp.Database;
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
    public partial class Vinst : ContentPage
    {
        public Vinst()
        {
            InitializeComponent();

            v.Text = "vinsten är " + Calc(); 
        }


        public double Calc()
        {
            int x = DBActions.admins.Count;
            return x * 500 * ((double)(30.0 / 100.0));

            


        }
    }
}