using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Xml.Linq;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
//using Stitchmania;


namespace Stitchmania
{
    public partial class StitchGallery : PhoneApplicationPage, INotifyPropertyChanged
    {
        public string qsParamID = "";

        bool newPageInstance = false;

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Set newPageInstance back to false. It will be set back to true if the constructor is called again.
            newPageInstance = false;

            // Set a key in the State dictionary that will be checked for in OnNavigatedTo
            this.State["PreservingPageState"] = true;

            // Save the Pivot control's SelectedIndex in page state
            State["GalleryIndex"] = pvtGallery.SelectedIndex;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Can't restore pivot state here because of a bug in Silverlight when restoring a pivot that contains more than 3 items...
            if (newPageInstance && this.State.ContainsKey("PreservingPageState"))
            {
                //Anyone want to restore something?
            }
            else if (NavigationContext.QueryString.TryGetValue("ID", out qsParamID)) //This was created for jumping to the correct stitch when being called from the stitch index page.
            {
                int id = Int32.Parse(qsParamID) - 1; //Subtracting 1 because StitchID value starts from 1 and SelectedIndex starts from 0
                pvtGallery.SelectedIndex = id;
            }
            else if (NavigationContext.QueryString.TryGetValue("TID", out qsParamID)) //This was created for jumping to the correct stitch when being called from a secondary tile.
            {
                int id = getStitchIndex(Int32.Parse(qsParamID));
                pvtGallery.SelectedIndex = id - 1;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Page> _pages;
        public ObservableCollection<Page> Pages
        {
            get { return _pages; }
            set
            {
                if (_pages != value)
                {
                    _pages = value;
                    OnPropertyChanged("Pages");
                }
            }
        }

        public void loadBackground(string imagePath)
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            brush.Opacity = 0.3;
            pvtGallery.Background = brush;
            pivotBackground.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            //pivotFadeIn.Begin();
        }

        public void MakeFavorite(int intIndex)
        {
            int intFavValue = 0;

            if (Pages[intIndex].Stitches[0].Favorite)
            {
                Pages[intIndex].Stitches[0].Favorite = false;
                intFavValue = 0;
            }
            else
            {
                Pages[intIndex].Stitches[0].Favorite = true;
                intFavValue = 1;
            }

            //Update XML with new Favorite info.  Will probably have to figure out how to update a single record if this is too slow..
            //Write(Pages, SMConstants.xmlFile);

            //Figured out how to update a single record...
            updateFavorite(pvtGallery.SelectedIndex + 1, intFavValue, SMConstants.xmlFile);

        }

        public int getStitchIndex(int sid)
        {
            int intSID = 0;

            //Generate grouping by favorites
            var stitch = (from page in Pages
                          where page.Stitches[0].StitchID.ToString() == sid.ToString()
                          select page.Stitches[0].StitchIndex).FirstOrDefault();

            intSID = Int32.Parse(stitch.ToString());

            return intSID;
        }

        public void updateFavorite(int intSID, int intFavValue, string fileName)
        {
            //Update record where stitchID = sid
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var xDoc = XDocument.Load(fs);

                XElement refStitch = (from query in xDoc.Descendants("ArrayOfPage").Descendants("Item")
                                       where (string)query.Element("StitchIndex") == intSID.ToString()
                                       select query).FirstOrDefault();

                //XElement refProject = xDoc.Elements("Projects").Where(e => (string)e.Element("ProjectID") == intPID.ToString()).FirstOrDefault();

                if (refStitch != null)
                {
                    refStitch.SetElementValue("Favorite", intFavValue.ToString());

                    //After loading the file we need to close and reopen with truncate so we can save a new copy of it
                    fs.Close();
                    fs = store.OpenFile(fileName, FileMode.Truncate, FileAccess.ReadWrite);

                    //Save updated xml
                    xDoc.Save(fs);
                }

                //Closing filestream
                fs.Close();
            }
        }

        public static T LoadFile<T>(string fileName)
        {
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                if(store.FileExists(fileName))
                {
                    IsolatedStorageFileStream stream = store.OpenFile(fileName, FileMode.Open);
                    T xmlResults = (T)serializer.Deserialize(stream);
                    stream.Close();
                    return xmlResults;
                    //return (T)serializer.Deserialize(stream);
                }
                else
                {
                    XmlReader stream = XmlReader.Create(fileName);         
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception emAll)
            {
                throw emAll;
            }
        }

        //ReadTextfile() was created to find out what the xml looked like after being created by the LoadFile function.
        //To see the serialized xml put a watch on readFile.Close() and look at the value of strMyXML in the XML visualizer.
        private void ReadTextFile()
        {
            //Obtain a virtual store for application
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            //This code will open and read the contents of myFile.txt
            //Add exception in case the user attempts to click “Read button first.

            StreamReader readFile = null;

            try
            {
                readFile = new StreamReader(new IsolatedStorageFileStream("StitchMania.xml", FileMode.Open, myStore));
                string fileText = readFile.ReadToEnd().ToString();
                
                //The control txtRead will display the text entered in the file
                string strMyXML = fileText;
                readFile.Close();
            }

            catch
            {
            }
        }

        private static string getVersion()
        {
            //Obtain a virtual store for application
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            //This code will open and read the contents of myFile.txt
            //Add exception in case the user attempts to click “Read button first.
            string strVersion = "";

            if (!myStore.DirectoryExists("Data"))
            {
                myStore.CreateDirectory("Data");
            }

            try
            {
                if (myStore.FileExists(SMConstants.versionFile))
                {
                    StreamReader readFile = null;
                    readFile = new StreamReader(new IsolatedStorageFileStream(SMConstants.versionFile, FileMode.Open, myStore));
                    string fileText = readFile.ReadToEnd().ToString();

                    //The control txtRead will display the text entered in the file
                    strVersion = fileText;
                    readFile.Close();
                }
                else
                {
                    StreamWriter writeFile = null;
                    //create a new file with the current version
                    writeFile = new StreamWriter(new IsolatedStorageFileStream(SMConstants.versionFile, FileMode.Create, myStore));
                    writeFile.Write("1", 0, 1);
                    writeFile.Close();

                    strVersion = "1";
                }
            }
            catch
            {
            }

            return strVersion;
        }

        public static void Write<T>(T obj, string fileName)
        {
            XmlWriterSettings writerSettings =
                    new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "\t"
                    };

            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();

                //If the data directory doesn't exist we will get an exception when we try to create the stitchmanaia.xml file there...
                if(!store.DirectoryExists("Data"))
                {
                    store.CreateDirectory("Data");
                }

                if (store.FileExists(fileName))
                    store.DeleteFile(fileName);

                IsolatedStorageFileStream stream = store.OpenFile(fileName, FileMode.Create);

                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (XmlWriter xmlWriter = XmlWriter.Create(stream, writerSettings))
                {
                    serializer.Serialize(xmlWriter, obj);
                }

                stream.Close();

                //Update version file
                StreamWriter writeFile = null;
                writeFile = new StreamWriter(new IsolatedStorageFileStream(SMConstants.versionFile, FileMode.Truncate, store));
                writeFile.Write(SMConstants.version, 0, 1);
                writeFile.Close();
            }
            catch (Exception emAll)
            {
                throw emAll;
            }
        }

        //Filter method for CollectionViewSource.  Not using this anymore, but might come in handy some day.
        public void FilterID(object sender, FilterEventArgs e)
        {
            Page filteredPage = e.Item as Page;

            if (qsParamID != "")
            {
                int id = Int32.Parse(qsParamID);

                if (filteredPage.Stitches[0].StitchID == id)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        public void upgradeStitches(string fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var xDoc = XDocument.Load(fs);

                //Store stitchID of favorites into an array
                string[] favStitchIDs = (from query in xDoc.Descendants("ArrayOfPage").Descendants("Item")
                                         where (string)query.Element("Favorite") == "1" || (string)query.Element("Favorite") == "true"
                                      select query.Element("StitchID").Value.ToString()).ToArray();

                //Closing filestream
                fs.Close();

                //Replace existing xml with new core xml
                Pages = LoadFile<ObservableCollection<Page>>(SMConstants.xmlCoreFile);
                Write(Pages, SMConstants.xmlFile);     

                //Setup favorite stitches
                fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                xDoc = XDocument.Load(fs);

                foreach (string sid in favStitchIDs)
                {
                    XElement refStitch = (from query in xDoc.Descendants("ArrayOfPage").Descendants("Item")
                                          where (string)query.Element("StitchID") == sid
                                          select query).FirstOrDefault();

                    if (refStitch != null)
                    {
                        refStitch.SetElementValue("Favorite", "1");
                    }
                }

                //After loading the file we need to close and reopen with truncate so we can save a new copy of it
                fs.Close();
                fs = store.OpenFile(fileName, FileMode.Truncate, FileAccess.ReadWrite);

                //Save updated xml
                xDoc.Save(fs);

                //Closing filestream
                fs.Close();
            }
        }

        // Constructor
        public StitchGallery()
        {
            InitializeComponent();

            newPageInstance = true;

            DataContext = this;

            var store = IsolatedStorageFile.GetUserStoreForApplication();

            //Check if an older version of the stitch gallery xml exists
            if (getVersion() != SMConstants.version && store.FileExists(SMConstants.xmlFile))
            {
                upgradeStitches(SMConstants.xmlFile);
            }
            
            //New install...
            if (!store.FileExists(SMConstants.xmlFile) || SMConstants.overwriteXML == true)
            {
                //Load core xml into ObservableCollection
                Pages = LoadFile<ObservableCollection<Page>>(SMConstants.xmlCoreFile);

                //Write ObservableCollection to IsolatedStorage
                Write(Pages, SMConstants.xmlFile);
            }
            else
            {
                //Load xml into ObservableCollection
                Pages = LoadFile<ObservableCollection<Page>>(SMConstants.xmlFile);

                /*
                //If this page was called with an ID parameter, then filter the collection accordingly **Not using this since I am navigating directly to the selectedindex**
                if (qsParamID != "")
                {
                    CollectionViewSource filterPage = new CollectionViewSource { Source = Pages };
                    filterPage.Filter += new FilterEventHandler(FilterID);
                    pvtGallery.ItemsSource = filterPage.View;
                }
                 * */

                //Pages = new ObservableCollection<Page> { new Page { Stitches = new ObservableCollection<Item> { new Item { StitchName = "Stitch01" }, new Item { Description = "BlahBlahBlah" }, new Item { Image = "Media/stitch01.jpg" }, new Item { ImageURI = "Media/stitch01.jpg" }, new Item { Difficulty = "Easy" }, new Item { Favorite = "true" } } } };
                //Pages.Add(new Page { Stitches = new ObservableCollection<Item> { new Item { StitchName = "Stitch02", Description = "BlahBlahBlah", Image = "Media/stitch02.jpg", ImageURI = "Media/stitch02.jpg", Difficulty = "Easy", Favorite = "true" } } });
            } 
        }



        private void pvtGallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void pvtGallery_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            string strImagePath = Pages[pvtGallery.SelectedIndex].Stitches[0].Image.ToString();
            loadBackground(strImagePath);
        }

        private void pvtGallery_UnloadingPivotItem(object sender, PivotItemEventArgs e)
        {
        }

        private void pvtGallery_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
        }

        private void pvtGallery_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
        }

        private void imgFav_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MakeFavorite(pvtGallery.SelectedIndex);
        }

        private void pvtGallery_Loaded(object sender, RoutedEventArgs e)
        {
            // Restore the Pivot control's SelectedIndex
            if (State.ContainsKey("GalleryIndex"))
                pvtGallery.SelectedIndex = (int)State["GalleryIndex"];
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            int intStitchIndex = pvtGallery.SelectedIndex + 1;
            string strStitchName = "";
            string strDescription = "";
            string strCO = "";
            string strChart = "";
            string strNotes = "";

            //Update record where stitchID = sid
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fs = store.OpenFile(SMConstants.xmlFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var xDoc = XDocument.Load(fs);

                XElement refStitch = (from query in xDoc.Descendants("ArrayOfPage").Descendants("Item")
                                      where (string)query.Element("StitchIndex") == intStitchIndex.ToString()
                                      select query).FirstOrDefault();

                if (refStitch != null)
                {
                    strStitchName = (string)refStitch.Element("StitchName");
                    strDescription = (string)refStitch.Element("Description");
                    strCO = (string)refStitch.Element("CO");
                    strChart = (string)refStitch.Element("Chart");
                    strNotes = (string)refStitch.Element("Notes");
                }

                //Closing filestream
                fs.Close();
            }

            String emailBody = "";

            emailBody = "Stitcho'mania Pattern for " + strStitchName + "\r\n";
            emailBody += "\r\n";
            emailBody += strDescription + "\r\n";
            emailBody += "\r\n";
            emailBody += strCO + "\r\n";
            emailBody += "\r\n";
            emailBody += strChart + "\r\n";
            emailBody += "\r\n";
            emailBody += strNotes + "\r\n";

            //emailAddressChooserTask.Show(); 
            //Create Email Message
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = "";
            //emailComposeTask.To = saveEmailAddressTask.Email; 
            emailComposeTask.Body = emailBody;
            //emailComposeTask.Cc = "testmail2@test.com"; 
            emailComposeTask.Subject = strStitchName + " pattern from Stitcho'mania!";
            emailComposeTask.Show(); 
        }

        private void ApplicationBarMenuItem2_Click(object sender, EventArgs e)
        {
            //verify that the tile does not already exist before creating a new one.
            var n = from c in ShellTile.ActiveTiles
                    where c.NavigationUri.ToString() == "/StitchGallery.xaml?TID=" + Pages[pvtGallery.SelectedIndex].Stitches[0].StitchID
                    select c;

            if (n.Count() == 0)
            {
                ShellTile.Create(new Uri("/StitchGallery.xaml?TID=" + Pages[pvtGallery.SelectedIndex].Stitches[0].StitchID, UriKind.Relative), new StandardTileData()
                {
                    BackgroundImage = new Uri("/Media/Tiles/" + Pages[pvtGallery.SelectedIndex].Stitches[0].Image, UriKind.Relative),
                    //Title = "Sample"
                });
            }
            else
            {
                MessageBox.Show("This stitch is already pinned to your start menu");
            }
        }
    }

}