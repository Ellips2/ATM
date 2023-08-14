using System;
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
        private static MainViewModel _instance = new MainViewModel();
        public static MainViewModel Instance { get { return _instance; } set{ _instance = value; } } 

        private readonly CloseApplicationCommand closeApplicationCommand = new CloseApplicationCommand();
        public RelayCommand CloseApplicationCommand { get; }

        private MessageModel message = new MessageModel();
        public MessageModel Message { get => message; set { message = value; OnPropertyChanged(nameof(Message)); } }

        #region [Для переключения между View]
        public RelayCommand CashViewCommand { get; set; }
        public RelayCommand DepositeViewCommand { get; set; }

        private object currentView;

        public object CurrentView
        {
            get { return currentView; }
            set { currentView = value;
                OnPropertyChanged();
            }
        }

        public CashViewModel CashVM { get; set; }
        public DepositeViewModel DepositeVM { get; set; }

        #endregion


        #region [Данные банкомата]
        private ObservableCollection<MoneyCassetteModel> moneyCassettes;
        public ObservableCollection<MoneyCassetteModel> MoneyCassettes { get => moneyCassettes; set {moneyCassettes = value; OnPropertyChanged(nameof(MoneyCassettes)); } }

        private int[] denominations = new int[4] { 100, 500, 1000, 5000};
        public int[] Denominations { get => denominations; }
        
        private int totalSum = 0;
        public int TotalSum { get { return totalSum = GetTotalSum(); } set { totalSum = value; if (totalSum < 0) totalSum = 0; OnPropertyChanged(nameof(TotalSum)); } }

        private string currency = "gold";
        public string Currency { get => currency; set { currency = value; OnPropertyChanged(nameof(Currency)); } }
        #endregion

        public MainViewModel()
        {
            Instance = this;

            CashVM = new CashViewModel();
            DepositeVM = new DepositeViewModel();
            CurrentView = CashVM;
            CashViewCommand = new RelayCommand(o => { CurrentView = CashVM; });
            DepositeViewCommand = new RelayCommand(o => { CurrentView = DepositeVM; });

            CloseApplicationCommand = new RelayCommand(closeApplicationCommand.Execute, closeApplicationCommand.CanExecute);

            //Создаем кассеты с определенным номиналом купюр и случайным количеством купюр
            MoneyCassettes = new ObservableCollection<MoneyCassetteModel>();
            Random rand = new Random();
            for (int i = 0; i < denominations.Length; i++)
            {
                MoneyCassettes.Add(new MoneyCassetteModel() { Denomination = denominations[i], CountBill = rand.Next(0, MoneyCassetteModel.MAX_BILL) });
            }
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
    }
}
