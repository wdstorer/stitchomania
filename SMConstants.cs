using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Stitchmania
{
    public class SMConstants
    {
        public const bool overwriteXML = false; //Setting this to true will force the xml in isolated storage to allways be replaced by xmlCoreFile
        public const string xmlFile = "Data/StitchMania.xml"; //File name of the isolated storage file
        public const string xmlCoreFile = "Data/StitchManiaCore.xml"; //File name of the core xml resource added to the project
        public const string xmlCounterFile = "/Data/StitchCounter.xml"; //File name of the xml that holds the stitch counter projects
        public const string version = "3"; //app will use this value to determine whether to import the new xml or not
        public const string versionFile = "Data/version.txt"; //text file that holds the version number
    }
}

