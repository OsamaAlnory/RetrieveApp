using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RetrieveApp.Elements;
using Xamarin.Forms;

namespace RetrieveApp.Droid
{
    public class CloseAppAndroid : ICloseApp
    {
        public void close()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}