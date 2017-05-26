using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MaterialDesignPathExtractor
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _filter;
        private Dictionary<string, ImageSource> _images = new Dictionary<string, ImageSource>();
        private Dictionary<string, ImageSource> _storedImages = new Dictionary<string, ImageSource>();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;


            foreach (DictionaryEntry currentResource in Application.Current.Resources.MergedDictionaries.First())
                if (currentResource.Value is ImageSource)
                    StoredImages.Add(currentResource.Key.ToString(), currentResource.Value as ImageSource);

            Images = StoredImages;
        }

        public Dictionary<string, ImageSource> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, ImageSource> StoredImages
        {
            get { return _storedImages; }
            set
            {
                _storedImages = value;
                OnPropertyChanged();
            }
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged();
            }
        }

        public ICommand FilterCommand => new RelayCommand(x =>
        {
            if (string.IsNullOrEmpty(Filter))
                Images = StoredImages;

            Images = StoredImages.Where(image => image.Key.ToLower().Contains(Filter.ToLower())).ToDictionary(p => p.Key, p => p.Value);
        });

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}