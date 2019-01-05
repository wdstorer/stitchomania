using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Advertising.Mobile.UI;
using System.Windows.Threading;
using Microsoft.Phone.Tasks;

namespace Stitchmania
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();
            //adControl1.AdControlError +=new EventHandler<ErrorEventArgs>(adControl1_AdControlError);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            txbLoading.Visibility = System.Windows.Visibility.Collapsed;
            rect1.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void StitchGalleryButton_Click(object sender, RoutedEventArgs e)
        {
            txbLoading.Visibility = System.Windows.Visibility.Visible;
            rect1.Visibility = System.Windows.Visibility.Visible;
            this.fadeIn.Begin();
            NavigationService.Navigate(new Uri("/StitchGallery.xaml", UriKind.Relative));
        }

        private void StitchIndexLinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StitchIndex.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        /* -- Removed because ErrorEventArgs is no longer found after updating to new vesrion of SDK and I don't think i need this anyway?
        private void adControl1_AdControlError(object sender, ErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                //Updating the property which inturn notify the clients to udpate.   
                this.tbAdvertisement.Text = "";
            });  
             
        }
         * */

 
        private void StitchCounterLinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StitchCounter.xaml", UriKind.Relative));
        }

        private void NewsLinkButton_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/TwitterNews.xaml", UriKind.Relative));
            var wbt = new WebBrowserTask();
            wbt.Uri = new Uri("http://mobile.twitter.com/stitchomania", UriKind.Absolute);
            wbt.Show(); 
        }

        private void FacebookLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var wbt = new WebBrowserTask();
            wbt.Uri = new Uri("http://m.facebook.com/pages/Stitchomania/245251288849176", UriKind.Absolute);
            wbt.Show(); 
        }

    }
}