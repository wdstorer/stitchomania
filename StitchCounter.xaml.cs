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
using System.Xml;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Phone.Shell;

namespace Stitchmania
{
    public partial class StitchCounter : PhoneApplicationPage
    {
        public StitchCounter()
        {
            InitializeComponent();

            //LOAD TEST PROJECTS 
            //saveProject(0, SMConstants.xmlCounterFile, "Sweater", "5", "3", "Add");
            //saveProject(0, SMConstants.xmlCounterFile, "Hat", "6", "4", "Add");
            //saveProject(0, SMConstants.xmlCounterFile, "Scarf", "6", "7", "Add");
            //saveProject(SMConstants.xmlCounterFile, "Socks", "6", "255", "Add");
            

            //Make sure xml file exists

            loadProjectList();

            lstProjects.SelectedIndex = getRecentProjectID();
            loadCounter();


        }

        public int getProjectIndexNum(int intProjectID)
        {

            return 0;
        }

        public int getRecentProjectID()
        {

            //returns the ID of the last updated project
            int intReturnIndex = 0;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(SMConstants.xmlCounterFile))
                {
                    using (var fs = store.OpenFile(SMConstants.xmlCounterFile, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            var xDoc = XDocument.Load(fs);
                            int intRecentID;

                            //find the projectid of the most recent project...
                            string strRecentID = (from p in xDoc.Element("Projects").Elements("Project")
                                                  orderby DateTime.Parse(p.Element("LastUpdate").Value.Substring(14)) descending
                                                  select p.Element("ProjectID").Value).FirstOrDefault();

                            //once we have the ID, we can loop through the project xml to find the index of the matching ID...
                            var projects = xDoc.Descendants("Projects").First();
                            int intProjectIndex = 0;

                            foreach (var project in projects.Elements())
                            {

                                if (project.Element("ProjectID").Value == strRecentID)
                                {
                                    if (Int32.TryParse(strRecentID, out intRecentID))
                                    {
                                        intReturnIndex = intProjectIndex;
                                        return intProjectIndex;    //once we have a match, just leave the function and return the index
                                    }
                                }
                                else
                                {
                                    intProjectIndex++;
                                }
                            }
                        }
                        catch
                        {
                            //string strException = exception.InnerException.ToString();
                        }
                    }
                }
            }

            return intReturnIndex;
        }

        public void loadProjectList()
        {
            string xmlStr = loadXML(SMConstants.xmlCounterFile); //loadXML returns "LoadFail" if xml file is not valid
            
            if (xmlStr != "LoadFail")
            {
                XDocument xmlProjects = XDocument.Parse(xmlStr);
                string newXmlStr = xmlProjects.ToString();
                //tbContent.Text = newXmlStr;

                var data = from query in xmlProjects.Descendants("Project")
                           select new Project
                           {
                               Name = (string)query.Attribute("Name"),
                               Rows = (string)query.Element("Rows"),
                               CurrentRow = (string)query.Element("CurrentRow"),
                               LastUpdate = (string)query.Element("LastUpdate")
                           };

                //Cheating to append an add option to the project list
                string[] data2 = { "test" };
                var data3 = from query in data2
                            select new Project
                            {
                                Name = "Add new project...",
                                Rows = "0",
                                CurrentRow = "0",
                                LastUpdate = "0"
                            };

                data = data.Union(data3);
                lstProjects.ItemsSource = data;
            }
            else
            {
                string[] data2 = { "test" };
                var data3 = from query in data2
                            select new Project
                            {
                                Name = "Add new project...",
                                Rows = "0",
                                CurrentRow = "0",
                                LastUpdate = "0"
                            };
                lstProjects.ItemsSource = data3;
            }
            //lstProjects.Items.Add("Add new project");
        }

        public void loadCounter()
        {
            string xmlStr = loadXML(SMConstants.xmlCounterFile);

            if (xmlStr != "LoadFail")
            {
                XDocument xmlProjects = XDocument.Parse(xmlStr);

                string newXmlStr = xmlProjects.ToString();

                //tbContent.Text = newXmlStr;

                var data = from query in xmlProjects.Descendants("Project")
                           select new Project
                           {
                               Name = (string)query.Attribute("Name"),
                               Rows = (string)query.Element("Rows"),
                               CurrentRow = (string)query.Element("CurrentRow"),
                               LastUpdate = (string)query.Element("LastUpdate"),
                               ProjectID = (string)query.Element("ProjectID")
                           };

                if (xmlProjects.Root.Elements("Project").Count() != 0)
                {
                    tbCounter.DataContext = data.ElementAt(lstProjects.SelectedIndex);
                    tbLastUpdate.DataContext = data.ElementAt(lstProjects.SelectedIndex);
                    tbProjectID.DataContext = data.ElementAt(lstProjects.SelectedIndex);
                    tbProjectName.DataContext = data.ElementAt(lstProjects.SelectedIndex);
                    tbTotalRows.DataContext = data.ElementAt(lstProjects.SelectedIndex);
                }

            }
        }

        public void createFile(string fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                store.CreateDirectory("Data");

                if (store.FileExists(fileName))
                {
                }
                else
                {
                    var fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
            }
        }

        public void saveProject(int pID, string fileName, string strProjectName, string strRows, string strCurrentRow, string strAction)
        {
            if (strAction == "Add")
            {

                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    bool xmlExists = false;
                    int intProjectID = 0;

                    store.CreateDirectory("Data");

                    if (store.FileExists(fileName) && loadXML(SMConstants.xmlCounterFile) != "LoadFail")
                    {
                        xmlExists = true;

                        //Get last projectID assigned 
                        string xmlStr = loadXML(fileName);
                        XDocument xmlProjects = XDocument.Parse(xmlStr);

                        if (xmlProjects.Root.Elements("Project").Count() != 0)
                        {
                            intProjectID = (from query in xmlProjects.Descendants("ProjectID")
                                            select (int)query).Max() + 1;
                        }
                        else
                        {
                            intProjectID = 1;
                        }
                    }
                    else
                    {
                        //store.CreateFile(fileName);
                        intProjectID = 1;
                    }

                    var fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    var root = new XElement("Projects");
                    var child = new XElement("Project");
                    var nameAttribute = new XAttribute("Name", strProjectName);
                    var projectID = new XElement("ProjectID", intProjectID);
                    var rows = new XElement("Rows", strRows);
                    var currentRow = new XElement("CurrentRow", strCurrentRow);
                    var lastUpdate = new XElement("LastUpdate", "Last updated~ " + DateTime.Now.ToString("G"));
                    var xDoc = new XDocument();

                    child.Add(nameAttribute, projectID, rows, currentRow, lastUpdate);
                    root.Add(child);

                    if (xmlExists == true)
                    {
                        //LOAD EXISTING XML AND ADD THE CHILD ELEMENT (PROJECT) TO THE EXISTING ROOT (PROJECTS)
                        xDoc = XDocument.Load(fs);
                        xDoc.Root.Add(child);
                        
                        //ONCE THE EXISTING PROJECT DATA IS LOADED, WE NEED TO CLOSE THEN
                        //REOPEN THE FILESTREAM WITH 'TRUNCATE' SO THAT WE CAN SAVE THE NEW XML. 
                        //OTHERWISE, THE XML DATA WILL BE APPENDED TO WHATEVER WAS ALREADY THERE.
                        fs.Close();
                        fs = store.OpenFile(fileName, FileMode.Truncate, FileAccess.ReadWrite);
                    }
                    else
                    {
                        //STARTING A NEW XML SO WE WILL WRITE THE WHOLE ROOT (PROJECTS) TO THE XML
                        xDoc.Add(root);
                    }

                    //Reorder projects by last modified date:
                    /*
                    var output = new XDocument(new XElement("Projects", from p in xDoc.Element("Projects").Elements("Project")
                                                                            orderby DateTime.Parse(p.Element("LastUpdate").Value.Substring(14)) descending
                                                                            select p));
                     * */

                    xDoc.Save(fs);

                    //FOR DEBUGGING. SHOW VALUE OF XML ON EACH PROJECT SAVE (UNCOMMENT BELOW TO SEE)
                    /*
                    fs.Position = 0;
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        var readToEnd = sr.ReadToEnd();
                        MessageBox.Show(readToEnd);
                    }
                    */

                    //Closing filestream
                    fs.Close();
                }
            }
            else if (strAction == "Update")
            {
                //Update record where ProjectID = pid
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    var xDoc = XDocument.Load(fs);

                    XElement refProject = (from query in xDoc.Descendants("Project")
                                           where (string)query.Element("ProjectID") == pID.ToString()
                                           select query).FirstOrDefault();

                    if (refProject != null)
                    {
                        refProject.SetAttributeValue("Name", strProjectName);
                        refProject.SetElementValue("Rows", strRows);
                        refProject.SetElementValue("CurrentRow", strCurrentRow);
                        refProject.SetElementValue("ProjectID", pID);

                        //FOR DEBUGGING. SHOW VALUE OF XML ON EACH PROJECT SAVE (UNCOMMENT BELOW TO SEE)
                        /*
                        fs.Position = 0;
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            var readToEnd = sr.ReadToEnd();
                            MessageBox.Show(readToEnd);
                        }
                        */

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
            else if (strAction == "Delete")
            {
                //Update record where ProjectID = pid
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    var xDoc = XDocument.Load(fs);

                    XElement refProject = (from query in xDoc.Descendants("Project")
                                           where (string)query.Element("ProjectID") == pID.ToString()
                                           select query).FirstOrDefault();

                    if (refProject != null)
                    {
                        refProject.Remove();

                        //FOR DEBUGGING. SHOW VALUE OF XML ON EACH PROJECT SAVE (UNCOMMENT BELOW TO SEE)
                        /*
                        fs.Position = 0;
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            var readToEnd = sr.ReadToEnd();
                            MessageBox.Show(readToEnd);
                        }
                        */

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
        }

        public void updateCounter(int intPID, int intCounter, int intResets, string fileName)
        {
            //Update record where ProjectID = pid
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var xDoc = XDocument.Load(fs);

                XElement refProject = (from query in xDoc.Descendants("Project")
                                      where (string)query.Element("ProjectID") == intPID.ToString()
                                           select query).FirstOrDefault();

                //XElement refProject = xDoc.Elements("Projects").Where(e => (string)e.Element("ProjectID") == intPID.ToString()).FirstOrDefault();

                if (refProject != null)
                {
                    refProject.SetElementValue("CurrentRow", intCounter.ToString());
                    refProject.SetElementValue("Rows", intResets.ToString());
                    refProject.SetElementValue("LastUpdate", "Last updated~ " + DateTime.Now.ToString("G"));

                    //FOR DEBUGGING. SHOW VALUE OF XML ON EACH PROJECT SAVE (UNCOMMENT BELOW TO SEE)
                    /*
                    fs.Position = 0;
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        var readToEnd = sr.ReadToEnd();
                        MessageBox.Show(readToEnd);
                    }
                    */

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

        public string loadXML(string fileName)
        {
            string strXML;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {

                store.CreateDirectory("Data");

                using (var fs = store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    try
                    {
                        var xDoc = XDocument.Load(fs);
                        strXML = xDoc.ToString();
                    }
                    catch
                    {
                        //string strException = exception.InnerException.ToString();
                        strXML = "LoadFail";
                    }
                }
            }

            return strXML;
        }


        private void lstProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Stitchmania.Project)(lstProjects.SelectedItem)).Name != "Add new project...")
                loadCounter();
            else
            {
                showAddProject();
            }
        }

        private void showAddProject()
        {
            adControl1.Visibility = System.Windows.Visibility.Collapsed;

            if (lstProjects.Items.Count > 1)
            {
                ContentPanel.Visibility = System.Windows.Visibility.Collapsed;
                lstProjects.IsEnabled = false;
                tbFirstProjectMessage.Visibility = System.Windows.Visibility.Collapsed;
                //imgEdit.Visibility = System.Windows.Visibility.Collapsed;
                btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                ContentPanel3.Visibility = System.Windows.Visibility.Collapsed;
                ContentPanel2.Visibility = System.Windows.Visibility.Collapsed;
                ContentPanel4.Visibility = System.Windows.Visibility.Visible;
                SetUpAppBar("add");
                tbxAddProjectName.Focus();
            }
            else
            {
                ContentPanel.Visibility = System.Windows.Visibility.Collapsed;
                lstProjects.IsEnabled = false;
                tbFirstProjectMessage.Visibility = System.Windows.Visibility.Visible;
                //imgEdit.Visibility = System.Windows.Visibility.Collapsed;
                btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                ContentPanel3.Visibility = System.Windows.Visibility.Collapsed;
                ContentPanel2.Visibility = System.Windows.Visibility.Collapsed;
                ContentPanel4.Visibility = System.Windows.Visibility.Visible;
                SetUpAppBar("addfirst");
                tbxAddProjectName.Focus();
            }
        }

        private string projectValidation(string strForm)
        {

            if (strForm == "add")
            {
                switch (tbxAddProjectName.Text)
                {
                    case "":
                        return "Project name cannot be blank.";
                    case "Add new project...":
                        return "Please enter a different name for your project.";
                }

                if (!checkInteger(tbxAddStartingRow.Text))
                    return "Please enter a valid number for starting row.";

                if (!checkInteger(tbxAddTotalRows.Text))
                    return "Please enter a valid number for total rows.";

            }

            if (strForm == "edit")
            {
                switch (tbxEditProjectName.Text)
                {
                    case "":
                        return "Project name cannot be blank.";
                    case "Add new project...":
                        return "Please enter a different name for your project.";
                }


                if (!checkInteger(tbxEditStartingRow.Text))
                    return "Please enter a valid number for starting row.";

                if (!checkInteger(tbxEditTotalRows.Text))
                    return "Please enter a valid number for total rows.";
            }

            return "";
        }

        private bool checkInteger(string strValue)
        {
            try
            {
                if (strValue is string)
                    int.Parse(strValue as string);
                else
                    int.Parse(strValue.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }
 

        private void showEditProject()
        {
            adControl1.Visibility = System.Windows.Visibility.Collapsed;
            ContentPanel.Visibility = System.Windows.Visibility.Collapsed;
            lstProjects.IsEnabled = false;
            tbFirstProjectMessage.Visibility = System.Windows.Visibility.Collapsed;
            //imgEdit.Visibility = System.Windows.Visibility.Collapsed;
            btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            ContentPanel3.Visibility = System.Windows.Visibility.Visible;
            ContentPanel2.Visibility = System.Windows.Visibility.Collapsed;
            ContentPanel4.Visibility = System.Windows.Visibility.Collapsed;
            SetUpAppBar("edit");
        }

        private void AppBarDecrease_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(tbCounter.Text) > 0)
            {
                int intCounter = Int32.Parse(tbCounter.Text) - 1;
                int intResets = Int32.Parse(tbTotalRows.Text);
                updateCounter(int.Parse(tbProjectID.Text), intCounter, intResets, SMConstants.xmlCounterFile);
                loadCounter();
            }
        }


        private void tbCounter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int intCounter = Int32.Parse(tbCounter.Text) + 1;
            int intResets = Int32.Parse(tbTotalRows.Text);
            updateCounter(int.Parse(tbProjectID.Text), intCounter, intResets, SMConstants.xmlCounterFile);
            loadCounter();
        }

        private void AppBarSave_Click(object sender, EventArgs e)
        {
            
            if (((Stitchmania.Project)(lstProjects.SelectedItem)).Name == "Add new project...")
            {
                if (projectValidation("add") != "")
                {
                    MessageBox.Show(projectValidation("add"), "", MessageBoxButton.OK);
                }
                else
                {
                    saveProject(0, SMConstants.xmlCounterFile, tbxAddProjectName.Text, tbxAddTotalRows.Text, tbxAddStartingRow.Text, "Add");
                    loadProjectList();
                    lstProjects.SelectedIndex = lstProjects.Items.Count - 2;
                    showStitchCounter();
                }
                
            }
            else
            {
                if (projectValidation("edit") != "")
                {
                    MessageBox.Show(projectValidation("edit"), "", MessageBoxButton.OK);
                }
                else
                {
                    int selectedProjectIndex = lstProjects.SelectedIndex;

                    saveProject(Int32.Parse(tbProjectID.Text), SMConstants.xmlCounterFile, tbxEditProjectName.Text, tbxEditTotalRows.Text, tbxEditStartingRow.Text, "Update");
                    loadProjectList();
                    loadCounter();
                    lstProjects.SelectedIndex = selectedProjectIndex;
                    //loadProjectList();
                    //lstProjects.SelectedIndex = lstProjects.Items.Count - 2;
                    showStitchCounter();
                }
            }
        }

        private void AppBarDelete_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Press OK to delete this project.", "confirm delete", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                //delete project
                saveProject(Int32.Parse(tbProjectID.Text), SMConstants.xmlCounterFile, tbxEditProjectName.Text, tbxEditTotalRows.Text, tbxEditStartingRow.Text, "Delete");
                showStitchCounter();
                loadProjectList();
                lstProjects.SelectedIndex = 0;
            }
        }

        private void AppBarCancel_Click(object sender, EventArgs e)
        {
            if (((Stitchmania.Project)(lstProjects.SelectedItem)).Name == "Add new project...")
                lstProjects.SelectedIndex = 0;


            //lstProjects.IsEnabled = false;
            showStitchCounter();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (ContentPanel3.Visibility == System.Windows.Visibility.Visible)
            {
                if (lstProjects.Items.Count > 1)
                {
                    e.Cancel = true;
                    showStitchCounter();
                }
            }

            if (ContentPanel4.Visibility == System.Windows.Visibility.Visible)
            {
                if (lstProjects.Items.Count > 1)
                {
                    e.Cancel = true;
                    showStitchCounter();
                }
            }
        }

        private void showStitchCounter()
        {
            //Clear new project textboxes
            tbxAddProjectName.Text = "";
            tbxAddTotalRows.Text = "0";
            tbxAddStartingRow.Text = "0";

            tbxEditProjectName.Text = "";
            tbxEditStartingRow.Text = "0";
            tbxEditTotalRows.Text = "0";

            //Make counter window visible again
            ContentPanel.Visibility = System.Windows.Visibility.Visible;
            ContentPanel3.Visibility = System.Windows.Visibility.Collapsed;
            ContentPanel2.Visibility = System.Windows.Visibility.Visible;
            ContentPanel4.Visibility = System.Windows.Visibility.Collapsed;
            SetUpAppBar("main");
            lstProjects.IsEnabled = true;
            //imgEdit.Visibility = System.Windows.Visibility.Visible;
            btnEdit.Visibility = System.Windows.Visibility.Visible;
            adControl1.Visibility = System.Windows.Visibility.Visible;
        }

        /*
        private void imgEdit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ContentPanel3.Visibility = System.Windows.Visibility.Visible;
            //ContentPanel2.Visibility = System.Windows.Visibility.Collapsed;
            //SetUpAppBar("edit");
            showEditProject();
            tbxEditProjectName.Text = ((Stitchmania.Project)(lstProjects.SelectedItem)).Name;
            tbxEditStartingRow.Text = tbCounter.Text;
            tbxEditTotalRows.Text = tbTotalRows.Text;
        }
         */

        private void SetUpAppBar(string strType)      
        {          
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBar.Opacity = 1;

            int count;

            for (count = 0; count < 4; count++)
            {
                if(ApplicationBar.Buttons.Count > 0)
                    ApplicationBar.Buttons.Remove(ApplicationBar.Buttons[0]);
            }

           ApplicationBarIconButton decrease = new ApplicationBarIconButton(new Uri("Media/appbar.down.rest.png", UriKind.Relative));
            decrease.Text = "decrease";
            decrease.Click += new EventHandler(AppBarDecrease_Click);

            ApplicationBarIconButton reset = new ApplicationBarIconButton(new Uri("Media/appbar.reset.rest.png", UriKind.Relative));
            reset.Text = "reset";
            reset.Click += new EventHandler(AppBarReset_Click);
       
            ApplicationBarIconButton save = new ApplicationBarIconButton(new Uri("Media/appbar.save.rest.png", UriKind.Relative));       
            save.Text = "save";
            save.Click += new EventHandler(AppBarSave_Click);

            ApplicationBarIconButton cancel = new ApplicationBarIconButton(new Uri("Media/appbar.cancel.rest.png", UriKind.Relative));
            cancel.Text = "cancel";
            cancel.Click += new EventHandler(AppBarCancel_Click);

            ApplicationBarIconButton delete = new ApplicationBarIconButton(new Uri("Media/appbar.delete.rest.png", UriKind.Relative));
            delete.Text = "delete";
            delete.Click += new EventHandler(AppBarDelete_Click);

            if (strType == "addfirst")
            {
                ApplicationBar.Buttons.Add(save);
                //ApplicationBar.Buttons.Add(cancel);
            }

            if (strType == "add")
            {
                ApplicationBar.Buttons.Add(save);
                ApplicationBar.Buttons.Add(cancel);
            }

            if (strType == "edit")
            {
                ApplicationBar.Buttons.Add(save);
                ApplicationBar.Buttons.Add(cancel);
                ApplicationBar.Buttons.Add(delete);
            }

            if (strType == "main")
            {
                ApplicationBar.Buttons.Add(decrease);
                ApplicationBar.Buttons.Add(reset);
            }
        }

        private void AppBarReset_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Press OK to reset this counter.", "confirm reset", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                int intCounter = 0;
                int intResets = Int32.Parse(tbTotalRows.Text) + 1;
                updateCounter(int.Parse(tbProjectID.Text), intCounter, intResets, SMConstants.xmlCounterFile);
                loadCounter();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            //ContentPanel3.Visibility = System.Windows.Visibility.Visible;
            //ContentPanel2.Visibility = System.Windows.Visibility.Collapsed;
            //SetUpAppBar("edit");
            showEditProject();
            tbxEditProjectName.Text = ((Stitchmania.Project)(lstProjects.SelectedItem)).Name;
            tbxEditStartingRow.Text = tbCounter.Text;
            tbxEditTotalRows.Text = tbTotalRows.Text;
        }
    }

    public class Project 
    {
        string projectid;
        string name; 
        string rows;
        string currentRow;
        string lastUpdate;

        public string ProjectID
        {
            get
            {
                return projectid;
            }
            set
            {
                projectid = value;
            }
        } 
        public string Name 
        { 
            get 
            { 
                return name; 
            } 
            set 
            { 
                name = value; 
            } 
        } 
        public string Rows
        { 
            get 
            { 
                return rows; 
            } 
            set 
            { 
                rows = value; 
            } 
        }
        public string CurrentRow
        {
            get
            {
                return currentRow;
            }
            set
            {
                currentRow = value;
            }
        }
        public string LastUpdate
        {
            get
            {
                return lastUpdate;
            }
            set
            {
                lastUpdate = value;
            }
        } 
    }
    
}