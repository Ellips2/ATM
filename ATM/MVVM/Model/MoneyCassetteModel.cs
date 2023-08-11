using ATM.Core;

namespace ATM.MVVM.Model
{
    public class MoneyCassetteModel : ObservableObject
    {
        public const int MAX_BILL = 2500;

        private int denomination;
        public int Denomination { get => denomination; set {denomination = value; OnPropertyChanged(nameof(Denomination)); } }

        private int countBill;
        public int CountBill { get => countBill; set { countBill = value; if (countBill < 0) countBill = 0; OnPropertyChanged(nameof(CountBill)); } }
    }
}
