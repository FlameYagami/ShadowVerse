using System;
using System.Windows.Input;

namespace ShadowVerse.Command
{
    public class DelegateCommand : ICommand
    {
        public Func<object, bool> CanExecuteCommand = null;
        public Action<object> ExecuteCommand = null;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return (CanExecuteCommand == null) || CanExecuteCommand(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteCommand?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}