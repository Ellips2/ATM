using ATM.Core;
using ATM.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
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
        public int DesireSumm { get => desireSumm; set { if (NumberValidationTextBox(value.ToString())) desireSumm = value; else desireSumm = 0; OnPropertyChanged(nameof(DesireSumm)); } }        

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
            if (desireSumm > 0)
            {
                for (int i = 0; i < MainVM.MoneyCassettes.Count; i++)
                {
                    if (desireSumm % MainVM.MoneyCassettes[i].Denomination == 0)
                    {
                        MainVM.Message = Cash(desireSumm, selectedDenomination, MainVM.MoneyCassettes);
                        OnPropertyChanged(nameof(MainVM.TotalSum));
                        return;
                    }
                }
            }
            DesireSumm = 0;
            MainVM.Message.Text = "Non correct input. Please enter another amount.";
            MainVM.Message.Color = "#FF0000";                        
        }

        public MessageModel Cash(int desireSumm, int desireBancnote, ObservableCollection<MoneyCassetteModel> moneyCassette)
        {
            if (moneyCassette.Sum(item => item.CountBill) > 0)
            {
                MoneyCassetteModel cassette = new MoneyCassetteModel();
                for (int i = moneyCassette.Count - 1; i >= 0; i--)
                    if (moneyCassette[i].Denomination == desireBancnote)
                        cassette = moneyCassette[i];

                if (desireSumm >= desireBancnote && cassette.CountBill > 0)
                {
                    cassette.CountBill -= desireSumm / desireBancnote;
                    if (cassette.CountBill < desireSumm / desireBancnote)
                        desireSumm = (desireSumm / desireBancnote - cassette.CountBill) * desireBancnote;
                    else
                        desireSumm -= desireSumm / desireBancnote * desireBancnote; //Находим остаток желаемой суммы с помощью div
                }
                if (desireSumm > 0 || cassette.CountBill == 0)
                {
                    desireBancnote = GetMatchingCassette(desireSumm, cassette, moneyCassette).Denomination;
                    Cash(desireSumm, desireBancnote, moneyCassette);
                }
            }
            else
            {
                return new MessageModel() { Text = "Sorry, no matching banknotes ;(", Color = "#FF0000" };
            }
            return new MessageModel() { Text = "Operation successful!", Color = "#008000" };
        }

        private MoneyCassetteModel GetMatchingCassette(int balance, MoneyCassetteModel cassette, ObservableCollection<MoneyCassetteModel> moneyCassette)
        {
            for (int i = moneyCassette.Count - 1; i >= 0; i--)
            {
                if (moneyCassette[i].CountBill > 0 && balance >= moneyCassette[i].Denomination)
                {
                    cassette = moneyCassette[i];
                    return cassette;
                }
            }
            return cassette;
        }

        public void SetDefault()
        {
            selectedDenomination = MainVM.Denominations[0];
            DesireSumm = 0;
        }
    }    
}
