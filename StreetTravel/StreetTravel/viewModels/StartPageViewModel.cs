using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.System.Profile;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StreetTravel.ViewModels
{
    class StartPageViewModel : ViewModelBase
    {
        private bool IsPcDevice = true;
        public StartPageViewModel()
        {
            //Windows.Desktop , Windows.Mobile
            var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (deviceFamily.CompareTo("Windows.Desktop") == 0)
            {
                IsPcDevice = true;
            }else
            {
                IsPcDevice = false;
            }
        }

        private Visibility _isShowAd = Visibility.Collapsed;
        public Visibility IsShowAd
        {
            get { return _isShowAd; }
            set
            {
                _isShowAd = value;
                RaisePropertyChanged("IsShowAd");
            }
        }


        // load webview or not?
        private bool _isActiveProgressRing = false;
        public bool IsActiveProgressRing
        {
            get
            {
                return _isActiveProgressRing;
            }
            set
            {
                _isActiveProgressRing = value;
                RaisePropertyChanged("IsActiveProgressRing");

            }
        }


        // webview content loading
        private RelayCommand<WebView> _onDisplay_ContentLoading;
        public RelayCommand<WebView> onDisplay_ContentLoading
        {
            get
            {
                return this._onDisplay_ContentLoading
                    ?? (this._onDisplay_ContentLoading = new RelayCommand<WebView>(item =>
                    {
                        IsActiveProgressRing = true;
                        WebViewContentLoading(item);
                    }));
            }
        }

        private async void WebViewContentLoading(WebView item)
        {
            //---------- check network ----------//
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageDialog message = new MessageDialog("You're not connected to Internet!");
                await message.ShowAsync();
            }
            var display = (Windows.UI.Xaml.Controls.WebView)item;
            string html = await display.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
        }



        // webview content loaded
        private RelayCommand<WebView> _onDisplay_ContentLoaded;
        public RelayCommand<WebView> onDisplay_ContentLoaded
        {
            get
            {
                return this._onDisplay_ContentLoaded
                    ?? (this._onDisplay_ContentLoaded = new RelayCommand<WebView>(item =>
                    {
                        WebViewContentLoaded(item);
                    }));
            }
        }

        private async void WebViewContentLoaded(WebView item)
        {
            var display = (Windows.UI.Xaml.Controls.WebView)item;
            string html = await display.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
        }



        public void WebViewScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                var appView = ApplicationView.GetForCurrentView();
                var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
                var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                var size = new Size(bounds.Width * scaleFactor, bounds.Height * scaleFactor);

                if (size.Width < 1000 || size.Height < 600 || !IsPcDevice)
                {
                    IsShowAd = Visibility.Collapsed;
                }
                else
                {
                    IsShowAd = Visibility.Visible;
                }

                if (e.Value.CompareTo("WINDOW_ON_LOAD") == 0)
                {
                    IsActiveProgressRing = false;
                }
            }
            catch(Exception ex)
            {

            }
        }
        public void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window.Current.CoreWindow.SizeChanged += (ss, ee) =>
            {
                try
                {
                    var appView = ApplicationView.GetForCurrentView();
                    var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
                    var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                    var size = new Size(bounds.Width * scaleFactor, bounds.Height * scaleFactor);

                    if (size.Width < 1000 || size.Height < 600 || !IsPcDevice)
                    {
                        IsShowAd = Visibility.Collapsed;
                    }
                    else
                    {
                        IsShowAd = Visibility.Visible;
                    }
                    ee.Handled = true;
                }
                catch(Exception ex)
                {

                }
            };

        }

    }
}
