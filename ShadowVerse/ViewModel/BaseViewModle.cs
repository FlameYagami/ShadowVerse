using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShadowVerse.Annotations;

namespace ShadowVerse.ViewModel
{
    public class BaseViewModle : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}