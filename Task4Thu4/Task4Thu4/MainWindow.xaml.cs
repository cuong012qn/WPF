using MaterialDesignThemes.Wpf;
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

namespace Task4Thu4
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

        private async void BtnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateAccountView createaccount = new CreateAccountView();
            await DialogHost.Show(createaccount, "MainWindow");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBlock text = new TextBlock()
            {
                Text = "Hi",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            CreateAccountView createAccount = new CreateAccountView();
            await DialogHost.Show(text, "DS");
        }
    }
}
