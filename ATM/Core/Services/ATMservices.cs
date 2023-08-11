using ATM.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace ATM.Core.Services
{
    public class ATMservices : ObservableObject
    {
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
                        desireSumm = (desireSumm / desireBancnote - cassette.CountBill)* desireBancnote;
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

        public void Deposite(int desireSumm, int desireBancnote, ObservableCollection<MoneyCassetteModel> moneyCassette)
        {
            //MoneyCassetteModel cassette = moneyCassette.Find(x => x.Denomination == desireBancnote);
            //if (cassette != null)
            //{
            //    cassette.CountBill -= desireSumm;
            //}
        }
    }
}
