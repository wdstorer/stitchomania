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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace Stitchmania
{
    public partial class MainMenu : PhoneApplicationPage
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void NewsLinkButton_Click(object sender, RoutedEventArgs e)
        {
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

        private void btnIndex_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StitchIndex.xaml", UriKind.Relative));
        }

        private void btnGallery_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StitchGallery.xaml", UriKind.Relative));
        }

        private void btnCounter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StitchCounter.xaml", UriKind.Relative));
        }

        private void cntxtGallery_Click(object sender, RoutedEventArgs e)
        {
            //verify that the tile does not already exist before creating a new one.
            var n = from c in ShellTile.ActiveTiles
                    where c.NavigationUri.ToString() == "/StitchGallery.xaml"
                    select c;

            if (n.Count() == 0)
            {
                ShellTile.Create(new Uri("/StitchGallery.xaml", UriKind.Relative), new StandardTileData()
                {
                    BackgroundImage = new Uri("/Media/Tiles/StitchGalleryTile03.jpg", UriKind.Relative),
                    //Title = "Sample"
                });
            }
            else
            {
                MessageBox.Show("This stitch gallery is already pinned to your start menu");
            }
        }

        private void cntxtIndex_Click(object sender, RoutedEventArgs e)
        {
            //verify that the tile does not already exist before creating a new one.
            var n = from c in ShellTile.ActiveTiles
                    where c.NavigationUri.ToString() == "/StitchIndex.xaml"
                    select c;

            if (n.Count() == 0)
            {
                ShellTile.Create(new Uri("/StitchIndex.xaml", UriKind.Relative), new StandardTileData()
                {
                    BackgroundImage = new Uri("/Media/Tiles/StitchIndexTile03.jpg", UriKind.Relative),
                    //Title = "Sample"
                });
            }
            else
            {
                MessageBox.Show("This stitch index is already pinned to your start menu");
            }
        }

        private void cntxtCounter_Click(object sender, RoutedEventArgs e)
        {
            //verify that the tile does not already exist before creating a new one.
            var n = from c in ShellTile.ActiveTiles
                    where c.NavigationUri.ToString() == "/StitchCounter.xaml"
                    select c;

            if (n.Count() == 0)
            {
                ShellTile.Create(new Uri("/StitchCounter.xaml", UriKind.Relative), new StandardTileData()
                {
                    BackgroundImage = new Uri("/Media/Tiles/StitchCounterTile04.jpg", UriKind.Relative),
                    //Title = "Sample"
                });
            }
            else
            {
                MessageBox.Show("This stitch counter is already pinned to your start menu");
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}