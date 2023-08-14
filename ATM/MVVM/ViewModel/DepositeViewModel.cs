using System;
using ATM.MVVM.Model;
using ATM.Core;
using System.Collections.ObjectModel;
using System.Linq;
using ATM.Core.Commands;

namespace ATM.MVVM.ViewModel
{
    class DepositeViewModel : ObservableObject
    {
        public MainViewModel MainVM { get; set; }

        private RelayCommand depositCommand;
        public RelayCommand DepositCommand { get { return depositCommand ?? (depositCommand = new RelayCommand(obj => { Deposit(); })); } }

        private ObservableCollection<MoneyCassetteModel> newMoneyCassettes = new ObservableCollection<MoneyCassetteModel>();

        private const int MAX_BANKNOTE_FOR_DEPOSIT = 50;

        public RelayCommandWithP<object> AddBanknoteCommand { get { if (addBanknoteCommand == null) addBanknoteCommand = new RelayCommandWithP<object>(AddBanknoteCommand_Execute); return addBanknoteCommand; } set {;} }
        private RelayCommandWithP<object> addBanknoteCommand = null;

        private int depositSumm = 0;
        public int DepositSumm { get { return depositSumm; } set { depositSumm = value; OnPropertyChanged(nameof(DepositSumm)); } }

        private void AddBanknoteCommand_Execute(object obj)
        {
            int addedDenomination = Convert.ToInt32(obj);
            if (newMoneyCassettes.Sum(item => item.CountBill) <= MAX_BANKNOTE_FOR_DEPOSIT) 
            {
                for (int i = 0; i < newMoneyCassettes.Count; i++)
                {
                    if (newMoneyCassettes[i].Denomination == addedDenomination)
                    {
                        newMoneyCassettes[i].CountBill++;
                    }
                }
            }
            DepositSumm = GetTotalSum();
        }

        public DepositeViewModel()
        {
            MainVM = MainViewModel.Instance;

            AddBanknoteCommand = new RelayCommandWithP<object>(AddBanknoteCommand_Execute);

            CreateNewMoneyCassets();
        }

        private void Deposit()
        {
            for (int i = 0; i < MainVM.MoneyCassettes.Count; i++)
            {
                MainVM.MoneyCassettes[i].CountBill += newMoneyCassettes[i].CountBill;
            }
            CreateNewMoneyCassets();
        }

        private int GetTotalSum()
        {
            int total = 0;
            for (int i = 0; i < newMoneyCassettes.Count; i++)
            {
                total += newMoneyCassettes[i].CountBill * newMoneyCassettes[i].Denomination;
            }
            return total;
        }

        public void CreateNewMoneyCassets()
        {
            newMoneyCassettes.Clear();
            for (int i = 0; i < MainVM.Denominations.Length; i++)
            {
                newMoneyCassettes.Add(new MoneyCassetteModel() { Denomination = MainVM.Denominations[i], CountBill = 0 });
            }
            DepositSumm = 0;
        }
    }
}
