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
using RetrieveApp.Droid;
using RetrieveApp.Elements;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseAppAndroid))]
namespace RetrieveApp.Droid
{
    public class CloseAppAndroid : ICloseApp
    {
        public void close()
        {
            Process.KillProcess(Process.MyPid());
            //var activity = (Activity)Forms.Context;
            //activity.FinishAffinity();
        }
    }
}