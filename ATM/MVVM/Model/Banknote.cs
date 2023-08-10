using System;
using System.Collections.Generic;
using System.Text;
using ATM.Core;

namespace ATM.MVVM.Model
{
    class Banknote : ObservableObject
    {
        private int denomination;
        public int Denomination { get; set; }
    }
}
