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
using System.Windows.Media.Imaging;

namespace Stitchmania
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int _stitchindex;

        public int StitchIndex
        {
            get { return _stitchindex; }
            set
            {
                if (_stitchindex != value)
                {
                    _stitchindex = value;
                    OnPropertyChanged("StitchIndex");
                }
            }
        }

        private int _stitchid;

        public int StitchID
        {
            get { return _stitchid; }
            set
            {
                if (_stitchid != value)
                {
                    _stitchid = value;
                    OnPropertyChanged("StitchID");
                }
            }
        }

        private string _stitchname;

        public string StitchName
        {
            get { return _stitchname; }
            set
            {
                if (_stitchname != value)
                {
                    _stitchname = value;
                    OnPropertyChanged("StitchName");
                }
            }
        }

        private string _image;

        public string Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        private string _imageuri;

        public string ImageURI
        {
            get { return _imageuri; }
            set
            {
                if (_imageuri != value)
                {
                    _imageuri = value;
                    OnPropertyChanged("ImageURI");
                }
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _chart;

        public string Chart
        {
            get { return _chart; }
            set
            {
                if (_chart != value)
                {
                    _chart = value;
                    OnPropertyChanged("Chart");
                }
            }
        }

        private string _co;

        public string CO
        {
            get { return _co; }
            set
            {
                if (_co != value)
                {
                    _co = value;
                    OnPropertyChanged("CO");
                }
            }
        }

        private string _notes;

        public string Notes
        {
            get { return _notes; }
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }

        private string _difficulty;

        public string Difficulty
        {
            get { return _difficulty; }
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    OnPropertyChanged("Difficulty");
                }
            }
        }

        private string _difficultytext;

        public string DifficultyText
        {
            get { return _difficulty.Remove(0, 2); }
        }

        private bool _favorite;

        public bool Favorite
        {
            get { return _favorite; }
            set
            {
                if (_favorite != value)
                {
                    _favorite = value;
                    OnPropertyChanged("Favorite");
                    OnPropertyChanged("imgFavorite");
                }
            }
        }

        //private ImageSource _imgfavorite;

        public ImageSource imgFavorite
        {
            get
            {
                if (Favorite)
                {
                    return new BitmapImage(new Uri("Media/FavIconSelected.png", UriKind.Relative));
                }
                else
                {
                    return new BitmapImage(new Uri("Media/FavIconDeSelected.png", UriKind.Relative));
                }
            }
        }
    }
}
