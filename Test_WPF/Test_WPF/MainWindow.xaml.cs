using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bạn đã click vào");
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            btn.Background = Brushes.Red;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            btn.Background = Brushes.MediumOrchid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txBlock.Text = "2334";
        }
    }
}
