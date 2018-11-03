using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using RetrieveApp.Droid;
using RetrieveApp.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(IEntry), typeof(IEntryRenderer))]
namespace RetrieveApp.Droid
{
    public class IEntryRenderer : EntryRenderer
    {
        public IEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
                if (Control != null)
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetColor(global::Android.Graphics.Color.Transparent);
                    Control.SetBackgroundDrawable(gd);
                    Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                var PADDING = (e.NewElement as IEntry).PADDING;
                if(PADDING != null)
                {
                    Control.SetPadding(PADDING[0],PADDING[1],PADDING[2],PADDING[3]);
                }
                    //Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
                }
        }

    }
}