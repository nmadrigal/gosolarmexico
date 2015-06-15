using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;

namespace AndroidApp
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/FluIcon", Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity
    {
        WebView web_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            web_view = FindViewById<WebView>(Resource.Id.webView);
            web_view.Settings.JavaScriptEnabled = true;
            web_view.LoadUrl("http://www.facebook.com");        

            web_view.SetWebViewClient(new MyWebViewClient());
        }

        public class MyWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.LoadUrl(url);
                return true;
            }
        }
    }
}

