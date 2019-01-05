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
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Stitchmania
{
    public partial class TwitterNews : PhoneApplicationPage
    {
 
        public TwitterNews()
        {
            InitializeComponent();
            getFeed();
        }

        void twitter_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                XElement element = XElement.Parse(e.Result);
                lbContent.ItemsSource = from tweet in element.Descendants("status")
                                        select new Tweet
                                        {
                                            UserName = tweet.Element("user").Element("screen_name").Value,
                                            Message = tweet.Element("text").Value,
                                            ImageSource = tweet.Element("user").Element("profile_image_url").Value,
                                            TweetSource = FormatTimestamp(tweet.Element("source").Value, tweet.Element("created_at").Value)
                                        };
                //textBlockPageTitle.Text = tbUsername.Text;
            }
            catch (Exception ex)
            {
                //textBlockPageTitle.Text = "ERROR: " + ex.Message;
            }
            ProgBar.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void getFeed()
        {
            ProgBar.Visibility = System.Windows.Visibility.Visible;
            WebClient twitter = new WebClient();
            twitter.DownloadStringCompleted += new DownloadStringCompletedEventHandler(twitter_DownloadStringCompleted);
            twitter.DownloadStringAsync(new Uri("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=stitchomania"));
        }

        private string StripHtml(string val)
        {
            return Regex.Replace(HttpUtility.HtmlDecode(val), @"<[^>]*>", "");
        }

        private string DateFormat(string val)
        {
            string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
            DateTime dt = DateTime.ParseExact(val, format, CultureInfo.InvariantCulture);
            return dt.ToString("h:mm tt MMM d");
        }

        private string FormatTimestamp(string imageSource, string date)
        {
            return DateFormat(date) + " via " + StripHtml(imageSource);
        }

    }

    public class Tweet
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string ImageSource { get; set; }
        public string TweetSource { get; set; }
    }
}