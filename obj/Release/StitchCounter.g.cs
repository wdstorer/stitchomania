﻿#pragma checksum "D:\Projects\MicrosoftDev\Stitchmania\StitchCounter.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9A06B1796AFFB02807153868E0B8228B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Advertising.Mobile.UI;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Stitchmania {
    
    
    public partial class StitchCounter : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel AdPanel;
        
        internal Microsoft.Advertising.Mobile.UI.AdControl adControl1;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock tbContent;
        
        internal Microsoft.Phone.Controls.ListPicker lstProjects;
        
        internal System.Windows.Controls.Button btnEdit;
        
        internal System.Windows.Controls.Grid ContentPanel2;
        
        internal System.Windows.Controls.TextBlock tbCounter;
        
        internal System.Windows.Controls.TextBlock tbTotalRows;
        
        internal System.Windows.Controls.TextBlock tbLastUpdate;
        
        internal System.Windows.Controls.TextBlock tbProjectID;
        
        internal System.Windows.Controls.TextBlock tbProjectName;
        
        internal System.Windows.Controls.Grid ContentPanel3;
        
        internal System.Windows.Controls.TextBlock tbEditProject;
        
        internal System.Windows.Controls.TextBox tbxEditProjectName;
        
        internal System.Windows.Controls.TextBox tbxEditStartingRow;
        
        internal System.Windows.Controls.TextBox tbxEditTotalRows;
        
        internal System.Windows.Controls.Grid ContentPanel4;
        
        internal System.Windows.Controls.TextBlock tbFirstProjectMessage;
        
        internal System.Windows.Controls.TextBlock tbAddProject;
        
        internal System.Windows.Controls.TextBox tbxAddProjectName;
        
        internal System.Windows.Controls.TextBox tbxAddStartingRow;
        
        internal System.Windows.Controls.TextBox tbxAddTotalRows;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Stitchmania;component/StitchCounter.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.AdPanel = ((System.Windows.Controls.StackPanel)(this.FindName("AdPanel")));
            this.adControl1 = ((Microsoft.Advertising.Mobile.UI.AdControl)(this.FindName("adControl1")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.tbContent = ((System.Windows.Controls.TextBlock)(this.FindName("tbContent")));
            this.lstProjects = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("lstProjects")));
            this.btnEdit = ((System.Windows.Controls.Button)(this.FindName("btnEdit")));
            this.ContentPanel2 = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel2")));
            this.tbCounter = ((System.Windows.Controls.TextBlock)(this.FindName("tbCounter")));
            this.tbTotalRows = ((System.Windows.Controls.TextBlock)(this.FindName("tbTotalRows")));
            this.tbLastUpdate = ((System.Windows.Controls.TextBlock)(this.FindName("tbLastUpdate")));
            this.tbProjectID = ((System.Windows.Controls.TextBlock)(this.FindName("tbProjectID")));
            this.tbProjectName = ((System.Windows.Controls.TextBlock)(this.FindName("tbProjectName")));
            this.ContentPanel3 = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel3")));
            this.tbEditProject = ((System.Windows.Controls.TextBlock)(this.FindName("tbEditProject")));
            this.tbxEditProjectName = ((System.Windows.Controls.TextBox)(this.FindName("tbxEditProjectName")));
            this.tbxEditStartingRow = ((System.Windows.Controls.TextBox)(this.FindName("tbxEditStartingRow")));
            this.tbxEditTotalRows = ((System.Windows.Controls.TextBox)(this.FindName("tbxEditTotalRows")));
            this.ContentPanel4 = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel4")));
            this.tbFirstProjectMessage = ((System.Windows.Controls.TextBlock)(this.FindName("tbFirstProjectMessage")));
            this.tbAddProject = ((System.Windows.Controls.TextBlock)(this.FindName("tbAddProject")));
            this.tbxAddProjectName = ((System.Windows.Controls.TextBox)(this.FindName("tbxAddProjectName")));
            this.tbxAddStartingRow = ((System.Windows.Controls.TextBox)(this.FindName("tbxAddStartingRow")));
            this.tbxAddTotalRows = ((System.Windows.Controls.TextBox)(this.FindName("tbxAddTotalRows")));
        }
    }
}
