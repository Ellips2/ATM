using System;
using System.Windows.Input;
using ATM.Core.Commands;

namespace ATM.Core.Commands
{
    internal class RelayCommandWithP<T> : Command
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommandWithP(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _execute(parameter);
    }
}
