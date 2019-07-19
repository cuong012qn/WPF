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

namespace SoanDe_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lsvCauHoi.ItemsSource = DataProvider.Instance.GetLstCauHoi();
            //GridView gridView = lsvCauHoi.View as GridView;
            //foreach (GridViewColumn gridViewColumn in gridView.Columns)
            //{
            //    gridViewColumn.Width = gridViewColumn.ActualWidth;
            //    gridViewColumn.Width = double.NaN;
            //}
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            CauHoi temp = ((Button)sender).DataContext as CauHoi;
            Sua sua = new Sua(temp);
            sua.ShowDialog();
            if (sua.isChange())
                lsvCauHoi.ItemsSource = DataProvider.Instance.GetLstCauHoi();
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (DataProvider.Instance.XoaCauHoi((((Button)sender).DataContext as CauHoi).MaCauHoi))
                lsvCauHoi.ItemsSource = DataProvider.Instance.GetLstCauHoi();
        }
    }
}
