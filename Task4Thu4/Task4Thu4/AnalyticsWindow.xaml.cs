using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace Task4Thu4
{
    /// <summary>
    /// Interaction logic for AnalyticsWindow.xaml
    /// </summary>
    public partial class AnalyticsWindow : Window
    {
        public AnalyticsWindow()
        {
            InitializeComponent();

            //dtAnalysis.ItemsSource = null;
            //dtAnalysis.ItemsSource = new List<SanPham>();
            //dtAnalysis.Columns[0].Header = "STT";
            //dtAnalysis.Columns[1].Header = "Tên";
            //dtAnalysis.Columns[2].Header = "Doanh thu";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (sender as RadioButton);
            if (radioButton.Content != null)
            {
                if (radioButton.Content.ToString().Equals("Sản phẩm"))
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                    List<Product> lstProduct = new List<Product>();
                    lstProduct.Add(new Product() { STT = 1, Name = "Điện thoại", Count = 10, CountPrice = 100000.ToString("#,###", cul.NumberFormat) });
                    dtAnalysis.ItemsSource = null;
                    dtAnalysis.ItemsSource = lstProduct;
                    dtAnalysis.Columns[0].Header = "STT";
                    dtAnalysis.Columns[1].Header = "Tên";
                    dtAnalysis.Columns[2].Header = "Số lượng";
                    dtAnalysis.Columns[3].Header = "Doanh thu";
                }
                else if (radioButton.Content.ToString().Equals("Loại sản phẩm"))
                {

                }
                else if (radioButton.Content.ToString().Equals("Nhân viên"))
                {

                }
                else
                {

                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TextBlock text = new TextBlock()
            //{
            //    Text = "Đang đợi",
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    VerticalAlignment = VerticalAlignment.Center,
            //    FontSize = 20
            //};
            //DialogHost.Show(text, "DataGrid");
            dtAnalysis.ItemsSource = null;
            dtAnalysis.ItemsSource = new List<Product>();
            dtAnalysis.Columns[0].Header = "STT";
            dtAnalysis.Columns[1].Header = "Tên";
            dtAnalysis.Columns[2].Header = "Số lượng";
            dtAnalysis.Columns[3].Header = "Doanh thu";
        }
    }

    class BaseClass
    {
        public int STT { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string CountPrice { get; set; }
    }

    class TypeOfProduct : BaseClass
    {

    }

    class Product : BaseClass
    {

    }
}
