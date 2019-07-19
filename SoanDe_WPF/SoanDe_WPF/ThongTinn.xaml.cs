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
using System.Windows.Shapes;

namespace SoanDe_WPF
{
    /// <summary>
    /// Interaction logic for Sua.xaml
    /// </summary>
    public partial class Sua : Window
    {
        CauHoi Cauhoi = new CauHoi();
        bool _isChange = false;
        public Sua()
        {
            InitializeComponent();
        }

        public Sua(CauHoi Cauhoi)
        {
            InitializeComponent();
            this.Cauhoi = Cauhoi;
            cbTheLoai.ItemsSource = DataProvider.Instance.GetTheLoai();
            //cbTheLoai.DisplayMemberPath = "TheLoai";
            //cbTheLoai.SelectedValuePath = "TheLoai";
            cbTheLoai.SelectedValue = Cauhoi.MaTheLoai;
            txbCauHoi.Text = Cauhoi.NoiDung;
            txbDapAnA.Text = Cauhoi.DapAnA;
            txbDapAnB.Text = Cauhoi.DapAnB;
            txbDapAnC.Text = Cauhoi.DapAnC;
            txbDapAnD.Text = Cauhoi.DapAnD;
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (!cbTheLoai.SelectedValue.Equals(Cauhoi.MaTheLoai))
            {
                if (_isChange == false)
                    _isChange = DataProvider.Instance.UpdateTheLoai(Cauhoi.MaCauHoi, cbTheLoai.SelectedValue.ToString());
            }
            //CauHois cauHois = new CauHois();
            //cauHois.CauHoi = txbCauHoi.Text;
            //cauHois.DapAnA = txbDapAnA.Text;
            //cauHois.DapAnB = txbDapAnB.Text;
            //cauHois.DapAnC = txbDapAnC.Text;
            //cauHois.DapAnD = txbDapAnD.Text;
            //foreach(var item in cbTheLoai.ItemsSource)
            //{
            //    MessageBox.Show(item.ToString());
            //}
            this.Close();
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            if (!cbTheLoai.SelectedValue.Equals(Cauhoi.MaTheLoai))
            {
                var result = MessageBox.Show("Bạn có muốn lưu", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (_isChange == false)
                        _isChange = DataProvider.Instance.UpdateTheLoai(Cauhoi.MaCauHoi, cbTheLoai.SelectedValue.ToString());
                }
                else this.Close();
            }
            this.Close();
        }


        public bool isChange()
        {
            return _isChange;
        }
    }
}
