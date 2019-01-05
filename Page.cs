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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Stitchmania
{
    public class Page : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Item> _stitches;

        public ObservableCollection<Item> Stitches
        {
            get { return _stitches; }
            set
            {
                if (_stitches != value)
                {
                    _stitches = value;
                    OnPropertyChanged("Stitches");
                }
            }
        }
    }
}
