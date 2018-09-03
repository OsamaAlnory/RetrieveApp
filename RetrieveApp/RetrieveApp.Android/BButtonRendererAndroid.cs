using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RetrieveApp.Droid;
using RetrieveApp.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BButton), typeof(BButtonRendererAndroid))]
namespace RetrieveApp.Droid
{
    public class BButtonRendererAndroid : ButtonRenderer
    {
        public BButtonRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if(e.OldElement == null)
            {
                var gr = new GradientDrawable();
                gr.SetCornerRadius(60f);
                gr.SetStroke(5, Android.Graphics.Color.DeepPink);
                Control.SetBackground(gr);
            }
        }

    }
}