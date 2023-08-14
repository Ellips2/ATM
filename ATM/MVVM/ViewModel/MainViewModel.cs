using System;
using ATM.Core;
using System.Collections.ObjectModel;
using ATM.MVVM.Model;

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
        public RelayCommand EmptyViewCommand { get; set; }

        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set { currentView = value;
                if (currentView == CashVM) DepositeVM.CreateNewMoneyCassets();
                if (currentView == DepositeVM) CashVM.SetDefault();
                if (currentView == EmptyVM) { CashVM.SetDefault(); DepositeVM.CreateNewMoneyCassets(); }
                OnPropertyChanged();
            }
        }

        public CashViewModel CashVM { get; set; }
        public DepositeViewModel DepositeVM { get; set; }
        public EmptyViewModel EmptyVM { get; set; }
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
            CloseApplicationCommand = new RelayCommand(closeApplicationCommand.Execute, closeApplicationCommand.CanExecute);

            //Переключение между View
            CashVM = new CashViewModel();
            DepositeVM = new DepositeViewModel();
            CurrentView = EmptyVM;
            CashViewCommand = new RelayCommand(o => { if (CurrentView != CashVM) CurrentView = CashVM; else CurrentView = EmptyVM; });
            DepositeViewCommand = new RelayCommand(o => { if (CurrentView != DepositeVM) CurrentView = DepositeVM; else CurrentView = EmptyVM; });

            //Создаем кассеты с определенным номиналом купюр и их случайным количеством
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
