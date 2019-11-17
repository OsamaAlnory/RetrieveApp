using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using RetrieveApp.Elements;
using RetrieveApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(IEntry), typeof(IEntryRenderer))]

namespace RetrieveApp.iOS
{
    public class IEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                //var PADDING = (e.NewElement as IEntry).PADDING;
                //if (PADDING != null)
                //{
                //Control.LeftView = new UIView(new CGRect(50, 0,
                //    PADDING[0], 0));
                //Control.LeftViewMode = UITextFieldViewMode.Always;
                //Control.RightView = new UIView(new CGRect(0, 0,
                //    PADDING[0], 0));
                //Control.RightViewMode = UITextFieldViewMode.Always;

                //}
                //Control.Layer.CornerRadius = 10;
                //Control.TextColor = UIColor.White;
            }
        }
    }
}