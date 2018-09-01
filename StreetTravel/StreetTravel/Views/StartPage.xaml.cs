using RTAppAndWebTalk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StreetTravel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        private SharedObject communication = new SharedObject();
        public StartPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }


        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                string src = "ms-appx-web:///Html/index.html";
                this.MyWebView.Navigate(new Uri(src));
            }
            catch (Exception ex)
            {

            }         
        }


        private void MyWebView_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }

        private void MyWebView_Loading(FrameworkElement sender, object args)
        {

        }

        private void MyWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            this.MyWebView.AddWebAllowedObject("sharedObj", communication);
        }

        private void MyWebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            (this.DataContext as ViewModels.StartPageViewModel).WebViewScriptNotify(sender, e);
        }

        private async void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            var size = new Size(bounds.Width * scaleFactor, bounds.Height * scaleFactor);

            if (size.Width < 1000 || size.Height < 600)
            {
                string[] script = new string[] { @"
                document.getElementById('pac-input').style.marginLeft='175px';
                document.getElementById('pac-input').style.width='280px';
                " };
                try
                {
                    await MyWebView.InvokeScriptAsync("eval", script);
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                string[] script = new string[] { @"
                document.getElementById('pac-input').style.marginLeft='250px';
                document.getElementById('pac-input').style.width='400px';
                " };

                try
                {
                    await MyWebView.InvokeScriptAsync("eval", script);
                }
                catch (Exception ex)
                {

                }
            }

            (this.DataContext as ViewModels.StartPageViewModel).PageSizeChanged(sender, e);
        }
    }
}
