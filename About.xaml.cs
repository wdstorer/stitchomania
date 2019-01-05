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

namespace Stitchmania
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void lnkEmailSupport_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = "WP7@stitchomania.com";
            emailComposeTask.Body = "";
            emailComposeTask.Subject = "WP7 Stitcho'mania Request";
            emailComposeTask.Show(); 
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            tbVersion.Text = "Stitcho'mania\nVersion 1.5";
            tbFAQ.Text = "Stitcho'mania is a mobile quick reference and tool for knitters. \nKey features of Stitcho'mania:\n-Browse stitches by name or by picture\n-Send patterns via email.\n-Pin menu items and patterns to the Start Menu\n-Stitch counter for tracking multiple projects.\n\nThanks to all who have written reviews and sent us feedback!";
        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            // pop up the link to rate and review the app
            MarketplaceReviewTask review = new MarketplaceReviewTask();
            review.Show();
        }
    }
}