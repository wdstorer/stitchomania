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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace Stitchmania
{
    public partial class StitchIndex : PhoneApplicationPage, INotifyPropertyChanged
    {
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

        //This function will 
        public static T LoadFile<T>(string fileName)
        {
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                if (store.FileExists(fileName))
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

        //This function will write the ObservableCollection that is passed to an xml file in isolated storage.
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
                if (!store.DirectoryExists("Data"))
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
            }
            catch (Exception emAll)
            {
                throw emAll;
            }
        }

        private static string getVersion()
        {
            //Obtain a virtual store for application
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

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


        public StitchIndex()
        {
            InitializeComponent();

            //create data
            /*
            Pages = new ObservableCollection<Page>();
            Pages.Add(new Page { Stitches = new ObservableCollection<Item> { new Item { StitchName = "AStitch01", Description = "BlahBlahBlah", Difficulty = "Easy", Image = "Media/stitch02.jpg" } } });
            Pages.Add(new Page { Stitches = new ObservableCollection<Item> { new Item { StitchName = "MStitch02", Description = "BlahBlahBlah", Difficulty = "Medium", Image = "Media/stitch02.jpg" } } });
            Pages.Add(new Page { Stitches = new ObservableCollection<Item> { new Item { StitchName = "EStitch03", Description = "BlahBlahBlah", Difficulty = "Easy", Image = "Media/stitch02.jpg" } } });
            Pages.Add(new Page { Stitches = new ObservableCollection<Item> { new Item { StitchName = "AStitch04", Description = "BlahBlahBlah", Difficulty = "Hard", Image = "Media/stitch02.jpg" } } });
            */

            var store = IsolatedStorageFile.GetUserStoreForApplication();

            //Check if an older version of the stitch gallery xml exists
            if (getVersion() != SMConstants.version && store.FileExists(SMConstants.xmlFile))
            {
                upgradeStitches(SMConstants.xmlFile);
            }

            if (!store.FileExists(SMConstants.xmlFile) || SMConstants.overwriteXML == true)
            {
                //Load core xml into ObservableCollection
                Pages = LoadFile<ObservableCollection<Page>>(SMConstants.xmlCoreFile);

                //Write ObservableCollection to IsolatedStorage
                Write(Pages, SMConstants.xmlFile);
            }
            else
            {
                Pages = LoadFile<ObservableCollection<Page>>(SMConstants.xmlFile);
            } 

            //Generate grouping by first letter
            var stitchesByAlpha = from page in Pages
                                 group page by page.Stitches[0].StitchName.Substring(0,1) into s
                                 orderby s.Key
                                 select new Group<Page>(s.Key, s);

            this.stitchesListGroupsAlpha.ItemsSource = stitchesByAlpha;

            //Generate grouping by difficulty
            var stitchesByDifficulty = from page in Pages
                                  group page by page.Stitches[0].Difficulty into s
                                  orderby s.Key
                                  select new Group<Page>(s.Key.Remove(0,2), s);

            this.stitchesListGroupsDifficulty.ItemsSource = stitchesByDifficulty;

            //Generate grouping by favorites
            var stitchesByFavs = from page in Pages
                                 where page.Stitches[0].Favorite == true
                                 group page by page.Stitches[0].StitchName.Substring(0, 1) into s
                                 orderby s.Key
                                 select new Group<Page>(s.Key, s);

            this.stitchesListGroupsFavs.ItemsSource = stitchesByFavs;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Save the Panorama control's SelectedIndex in page state
            State["PanoramaIndex"] = PanoramaControl.SelectedIndex;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Restore the Panorama control's SelectedIndex

            if (State.ContainsKey("PanoramaIndex"))
                PanoramaControl.DefaultItem = PanoramaControl.Items[(int)State["PanoramaIndex"]];
        }

        private void btnStitch_Click(object sender, RoutedEventArgs e)
        {
            Page page = ((Button)sender).DataContext as Page;
            if (page != null)
            {
                NavigationService.Navigate(new Uri("/StitchGallery.xaml?ID=" + page.Stitches[0].StitchIndex, UriKind.Relative));
            }
        }

    }


    public class Group<T> : IEnumerable<T>        
    {            
        public Group(string name, IEnumerable<T> items)            
        {                
            this.Title = name;                
            this.Items = new List<T>(items);            
        }            
        
        public override bool Equals(object obj)            
        {                
            Group<T> that = obj as Group<T>;                
            return (that != null) && (this.Title.Equals(that.Title));            
        }            
        
        public string Title            
        {                
            get;                
            set;            
        }            
        
        public IList<T> Items            
        {                
            get;                
            set;            
        }            
        
        #region IEnumerable<T> Members            
        public IEnumerator<T> GetEnumerator()            
        {                
            return this.Items.GetEnumerator();            
        }            
        #endregion            
        
        #region IEnumerable Members            
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()            
        {                
            return this.Items.GetEnumerator();            
        }            
        #endregion        
    }

}