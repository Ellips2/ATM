﻿using System;
using ATM.Core;
using System.Collections.ObjectModel;
using ATM.MVVM.Model;
using ATM.Core.Services;
using System.Linq;
using System.Windows.Input;
using ATM.Core.Commands;

namespace ATM.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private readonly CloseApplicationCommand closeApplicationCommand = new CloseApplicationCommand();
        public RelayCommand CloseApplicationCommand { get; }

        
        #region [Данные банкомата]
        private ObservableCollection<MoneyCassetteModel> newMoneyCassettes = new ObservableCollection<MoneyCassetteModel>();
        private ObservableCollection<MoneyCassetteModel> moneyCassettes;
        public ObservableCollection<MoneyCassetteModel> MoneyCassettes { get => moneyCassettes; set {moneyCassettes = value; OnPropertyChanged(nameof(MoneyCassettes)); } }

        private int[] denominations = new int[4] { 100, 500, 1000, 5000};
        public int[] Denominations { get => denominations; }
        
        private int totalSum = 0;

        public RelayCommandWithP<object> OKCommand
        {
            get
            {
                if (_okCommand == null)
                    _okCommand = new RelayCommandWithP<object>(OkCommand_Execute);
                return _okCommand;
            }
            set
            {
                ;
            }
        }
        private RelayCommandWithP<object> _okCommand = null;

        private void OkCommand_Execute(object obj)
        {
            int addedDenomination = Convert.ToInt32(obj);
            for (int i = 0; i < newMoneyCassettes.Count; i++)
            {
                if (newMoneyCassettes[i].Denomination == addedDenomination)
                {
                    newMoneyCassettes[i].CountBill++;
                }
            }
            MoneyCassettes = newMoneyCassettes;
        }

        public int TotalSum { get { return totalSum = GetTotalSum(); } set { totalSum = value; if (totalSum < 0) totalSum = 0; OnPropertyChanged(nameof(TotalSum)); } }

        private string currency = "gold";
        public string Currency { get => currency; set { currency = value; OnPropertyChanged(nameof(Currency)); } }
        #endregion


        #region [Ввод пользователя]
        private int selectedDenomination;
        public int SelectedDenomination { get => selectedDenomination; set { selectedDenomination = value; OnPropertyChanged(nameof(SelectedDenomination)); } }

        private int desireSumm = 0;
        public int DesireSumm { get => desireSumm; set { desireSumm = value; OnPropertyChanged(nameof(DesireSumm)); } }

        private int inputSumm = 0;
        public int InputSumm { get { return inputSumm; } set { inputSumm = value; if (totalSum < 0) totalSum = 0; OnPropertyChanged(nameof(InputSumm)); } }
        #endregion


        #region [Сервис банкомата]
        private ATMservices _ATMservices = new ATMservices();

        private MessageModel message = new MessageModel();
        public MessageModel Message { get => message; set { message = value; OnPropertyChanged(nameof(Message)); } }

        private RelayCommand cashCommand;
        public RelayCommand CashCommand { get { return cashCommand ?? (cashCommand = new RelayCommand(obj => { TryCash(); })); } }

        private RelayCommand depositCommand;
        public RelayCommand DepositCommand { get { return depositCommand ?? (depositCommand = new RelayCommand(obj => { TryDeposite(); })); } }
        #endregion

        public MainViewModel()
        {
            CloseApplicationCommand = new RelayCommand(closeApplicationCommand.Execute, closeApplicationCommand.CanExecute);
            OKCommand = new RelayCommandWithP<object>(OkCommand_Execute);
            //Создаем кассеты с определенным номиналом купюр и случайным количеством купюр
            MoneyCassettes = new ObservableCollection<MoneyCassetteModel>();
            Random rand = new Random();
            for (int i = 0; i < denominations.Length; i++)
            {
                MoneyCassettes.Add(new MoneyCassetteModel() { Denomination = denominations[i], CountBill = rand.Next(0, MoneyCassetteModel.MAX_BILL) });
            }
            selectedDenomination = denominations[0];    //Номинал для размена по умолчанию
        }
        private int GetTotalSum()
        {
            int total = 0;
            for (int i = 0; i < MoneyCassettes.Count; i++)
            {
                total += MoneyCassettes[i].CountBill * MoneyCassettes[i].Denomination;
            }
            return total;
        }

        private void TryCash()
        {
            for (int i = 0; i < moneyCassettes.Count; i++)
            {
                if (desireSumm % moneyCassettes[i].Denomination == 0)
                {
                    Message = _ATMservices.Cash(desireSumm, selectedDenomination, MoneyCassettes);
                    OnPropertyChanged(nameof(TotalSum));
                    return;
                }
            } 
            Message.Text = "Non correct input. Please enter another amount.";
            Message.Color = "#FF0000";
        }
        private void TryDeposite()
        {
            newMoneyCassettes.Clear();
            for (int i = 0; i < denominations.Length; i++)
            {
                newMoneyCassettes.Add(new MoneyCassetteModel() { Denomination = denominations[i], CountBill = 0 });
            }
            //InputSumm = 0;
            //if (desireSumm % selectedDenomination == 0)
            //{
            //    //Message = _ATMservices.Deposite(desireSumm, selectedDenomination, MoneyCassettes);
            //    OnPropertyChanged(nameof(TotalSum));
            //}
            //else
            //{
            //    Message.Text = "Non correct input. Please enter another amount.";
            //    Message.Color = "#FF0000";
            //}
        }

        private void AddBanknote(object denomination)
        {
            //InputSumm += denomination ;
        }
    }
}
