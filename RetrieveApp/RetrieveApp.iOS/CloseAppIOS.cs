using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
using RetrieveApp.Elements;
using RetrieveApp.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseAppIOS))]
namespace RetrieveApp.iOS
{
    public class CloseAppIOS : ICloseApp
    {
        public void close()
        {
            Thread.CurrentThread.Abort();
        }
    }
}