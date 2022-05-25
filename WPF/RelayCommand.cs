using System;
using System.Windows.Input;

namespace WPF
{
    public class RelayCommand : ICommand
    {
        private Action<object> _executeAction;
        private Func<object, bool> _canExecuteAction;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeAction = execute;
            _canExecuteAction = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null || _canExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _executeAction(parameter);
            }
        }
    }
}
