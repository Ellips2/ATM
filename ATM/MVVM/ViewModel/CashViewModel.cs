using ATM.Core;
using System;

namespace ATM.MVVM.ViewModel
{
    internal class CashViewModel : ObservableObject
    {
        public MainViewModel MainVM { get; set; }

        private RelayCommand cashCommand;
        public RelayCommand CashCommand { get { return cashCommand ?? (cashCommand = new RelayCommand(obj => { TryCash(); })); } }

        private int selectedDenomination;
        public int SelectedDenomination { get => selectedDenomination; set { selectedDenomination = value; OnPropertyChanged(nameof(SelectedDenomination)); } }

        private int desireSumm = 0;
        public int DesireSumm { get => desireSumm; set { if (NumberValidationTextBox(value.ToString()) && value > 0) desireSumm = value; else desireSumm = 0; OnPropertyChanged(nameof(DesireSumm)); } }        

        public CashViewModel()
        {
            MainVM = MainViewModel.Instance;
            selectedDenomination = MainVM.Denominations[0];    //Номинал для размена по умолчанию
        }
        private bool NumberValidationTextBox(string obj)
        {
            Char[] ch = obj.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (!Char.IsNumber(ch[i]))
                {
                    return false;
                }
            }            
            return true;
        }

        private void TryCash()
        {
            for (int i = 0; i < MainVM.MoneyCassettes.Count; i++)
            {
                if (desireSumm % MainVM.MoneyCassettes[i].Denomination == 0)
                {
                    MainVM.Message = CashService.Cash(desireSumm, selectedDenomination, MainVM.MoneyCassettes);
                    OnPropertyChanged(nameof(MainVM.TotalSum));
                    return;
                }
            }
            DesireSumm = 0;
            MainVM.Message.Text = "Non correct input. Please enter another amount.";
            MainVM.Message.Color = "#FF0000";                        
        }        

        public void SetDefault()
        {
            selectedDenomination = MainVM.Denominations[0];
            DesireSumm = 0;
        }
    }    
}
