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

namespace Sodoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //StackPanel panel = new StackPanel();
            //panel.Orientation = Orientation.Horizontal;
            //for (int i = 0; i < 10; i++)
            //{
            //    panel.Children.Add(new Button() { Height = 100, Width = 100 });
            //}
            //test.Items.Add(panel);
            ////grid.Children.Add(panel);
        }
    }
}
