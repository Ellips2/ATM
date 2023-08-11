using ATM.Core;
using System.Drawing;

namespace ATM.MVVM.Model
{
    public class MessageModel : ObservableObject
    {
        private string text = "";
        public string Text { get => text; set { text = value; OnPropertyChanged(nameof(Text)); } }

        private string  color = "#FFFFFF";
        public string Color { get => color; set { color = value; OnPropertyChanged(nameof(Color)); } }

    }
}
