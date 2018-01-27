using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahamutCardCrawler.Model
{
    public class AppbarModel : BaseModel
    {
        private Visibility _maximizeVisibility;
        private Visibility _restoreVisibility;

        public Visibility RestoreVisibility
        {
            get { return _restoreVisibility; }
            set
            {
                _restoreVisibility = value;
                OnPropertyChanged(nameof(RestoreVisibility));
            }
        }

        public Visibility MaximizeVisibility
        {
            get { return _maximizeVisibility; }
            set
            {
                _maximizeVisibility = value;
                OnPropertyChanged(nameof(MaximizeVisibility));
            }
        }

        public AppbarModel()
        {
            RestoreVisibility = Visibility.Collapsed;
            MaximizeVisibility = Visibility.Visible;
        }

        public void Maximize()
        {
            RestoreVisibility = Visibility.Visible;
            MaximizeVisibility = Visibility.Collapsed;
        }

        public void Restore()
        {
            RestoreVisibility = Visibility.Collapsed;
            MaximizeVisibility = Visibility.Visible;
        }
    }
}
