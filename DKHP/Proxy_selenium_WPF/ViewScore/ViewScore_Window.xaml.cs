using System;
using System.Collections.Generic;
using System.IO;
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

namespace Proxy_selenium_WPF
{
    /// <summary>
    /// Interaction logic for ViewScore.xaml
    /// </summary>
    public partial class ViewScore : Window
    {
        public ViewScore()
        {
            InitializeComponent();
            dtViewScore.ItemsSource = LoadData();
            //MessageBox.Show(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Data.txt"));
        }

        private List<ObjectScore> LoadData()
        {
            string workingPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Data.txt");
            List<ObjectScore> lstObjScore = new List<ObjectScore>();
            string data = File.ReadAllText(workingPath);
            foreach (var s in data.Split('\n'))
            {
                ObjectScore objScore = new ObjectScore();
                var score = s.Split(',');
                //length = 13
                if (!string.IsNullOrEmpty(score[0]) && score.Length.Equals(13))
                {
                    objScore.STT = Convert.ToInt32(score[0]);
                    objScore.TenMonHoc = score[1];
                    objScore.MaLop = score[2];
                    objScore.TinChi = Convert.ToInt32(score[3]);
                    objScore.ThuongKy = (string.IsNullOrWhiteSpace(score[4])) ? "" : score[4].ToString();
                    objScore.GiuaKy = (string.IsNullOrWhiteSpace(score[5])) ? "" : score[5].ToString();
                    objScore.TieuLuan = (string.IsNullOrWhiteSpace(score[6])) ? "" : score[6].ToString();
                    objScore.ThucHanh = (string.IsNullOrWhiteSpace(score[7])) ? "" : score[7].ToString();
                    objScore.KetThuc = (string.IsNullOrWhiteSpace(score[8])) ? "" : score[8].ToString();
                    objScore.TrungBinh = (string.IsNullOrWhiteSpace(score[9])) ? "" : score[9].ToString();
                    objScore.XepLoai = score[10];
                    objScore.GhiChu = score[11];
                    lstObjScore.Add(objScore);
                }
                else if (!string.IsNullOrEmpty(score[0]) && score.Length.Equals(10))
                {
                    objScore.STT = Convert.ToInt32(score[0]);
                    objScore.TenMonHoc = score[1];
                    objScore.MaLop = score[2];
                    objScore.TinChi = Convert.ToInt32(score[3]);
                    objScore.ThuongKy = string.Empty;
                    objScore.GiuaKy = string.Empty;
                    objScore.TieuLuan = string.Empty;
                    objScore.ThucHanh = string.Empty;
                    objScore.KetThuc = (string.IsNullOrWhiteSpace(score[4])) ? "" : score[4].ToString();
                    objScore.TrungBinh = (string.IsNullOrWhiteSpace(score[5])) ? "" : score[5].ToString();
                    objScore.XepLoai = score[6];
                    objScore.GhiChu = score[7];
                    lstObjScore.Add(objScore);
                }
            }
            return lstObjScore;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
