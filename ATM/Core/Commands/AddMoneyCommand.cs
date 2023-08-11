using System;
using System.Windows.Input;
using ATM.MVVM.ViewModel;

namespace ATM.Core.Commands
{
    internal class AddMoneyCommand : Command
    {
        internal MainViewModel ViewModel { get; set; }

        internal AddMoneyCommand(MainViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }


        //public override bool CanExecute(object parameter) => true;

        //public override void Execute(object parameter) => this.ParametrMethod(parameter as String);


        public override bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                var p = parameter as String;
                if (String.IsNullOrEmpty(p))
                    return false;
                return true;
            }
            return false;
        }

        public override void Execute(object parameter)
        {
            //this.ViewModel.ParametrMethod(parameter as String);
        }

    }
}
