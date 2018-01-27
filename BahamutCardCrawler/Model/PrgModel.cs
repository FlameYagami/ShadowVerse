using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahamutCardCrawler.Model
{
    public class PrgModel :BaseModel
    {
        private int _prgValue;
        private string _prgHint;
        private Visibility _prgVlaueVisibility;
        private Visibility _prgHintVisibility;

        public int PrgValue
        {
            get { return _prgValue; }
            set
            {
                _prgValue = value;
                OnPropertyChanged(nameof(PrgValue));
            }
        }

        public string PrgHint
        {
            get { return _prgHint; }
            set
            {
                _prgHint = value;
                OnPropertyChanged(nameof(PrgHint));
            }
        }

        public Visibility PrgValueVisibility
        {
            get { return _prgVlaueVisibility; }
            set
            {
                _prgVlaueVisibility = value;
                OnPropertyChanged(nameof(PrgValueVisibility));
            }
        }

        public Visibility PrgHintVisibility
        {
            get { return _prgHintVisibility; }
            set
            {
                _prgHintVisibility = value;
                OnPropertyChanged(nameof(PrgHintVisibility));
            }
        }

        public PrgModel()
        {
            PrgValue = 0;
            PrgValueVisibility = Visibility.Hidden;
            PrgHint = string.Empty;
            PrgHintVisibility = Visibility.Hidden;
        }

        public void Start()
        {
            PrgHintVisibility = Visibility.Visible;
            PrgValueVisibility = Visibility.Visible;
        }

        public void Finish()
        {
            PrgValue = 100;
            PrgHint = "已完成";
        }

        public void Update(int prgValue, string prgHint)
        {
            PrgValue = prgValue;
            PrgHint = prgHint;
        }
    }
}
