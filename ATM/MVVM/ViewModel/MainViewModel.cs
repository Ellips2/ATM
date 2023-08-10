using System;
using ATM.Core;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TextHandler.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private readonly CloseApplicationCommand closeApplicationCommand = new CloseApplicationCommand();
        public RelayCommand CloseApplicationCommand { get; }

        //#region [Работа с файлами]
        //public ObservableCollection<TextFile> TextFiles { get; set; }
        //private readonly TextProcess textProcess = new TextProcess();
        //private TextFile selectedTextFile = new TextFile();
        //public TextFile SelectedTextFile { get => selectedTextFile; set { selectedTextFile = value; OnPropertyChanged(nameof(SelectedTextFile)); } }
        //#endregion

        

        public MainViewModel()
        {
            //TextFiles = new ObservableCollection<TextFile>();            
            CloseApplicationCommand = new RelayCommand(closeApplicationCommand.Execute, closeApplicationCommand.CanExecute);            
        }

    }
}
