using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using ATM.MVVM.ViewModel;

namespace ATM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }    
}
