using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;

namespace Proxy_selenium_WPF
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private readonly string currentFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        public Main()
        {
            InitializeComponent();
            dtViewScore.ItemsSource = LoadScore();
        }

        public Main(string MSSV)
        {
            InitializeComponent();
            dtViewScore.ItemsSource = LoadScore();
            LoadImage(MSSV);
            LoadInfo();
            txbMSSV.Text += MSSV;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }


        private async void LoadImage(string MSSV)
        {
            if (string.IsNullOrEmpty(MSSV)) return;
            Task<bool> image = Task.Run(() => DownloadImage(MSSV));
            bool result = await image;
            if (result)
            {
                BitmapImage bmImage =
              new BitmapImage(new Uri(Path.Combine(currentFolder, "image.png")));
                ImageBrush imgBrush = new ImageBrush(bmImage);
                cvImage.Background = imgBrush;
            }
            else cvImage.Background = Brushes.WhiteSmoke;
        }

        private bool DownloadImage(string MSSV)
        {
            string url = string.Format("http://phongdaotao2.ntt.edu.vn/GetImage.aspx?MSSV={0}", MSSV);
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    using (Stream stream = webClient.OpenRead(url))
                    {
                        using (Bitmap bitmap = new Bitmap(stream))
                        {
                            bitmap.Save(Path.Combine(currentFolder, "image.png"), ImageFormat.Png);
                        }
                    }
                }
                //Process.Start(Path.Combine(currentFolder, "image.png"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void LoadInfo()
        {
            string[] ReadFromFile = File.ReadAllText(Path.Combine(currentFolder, "Info.txt")).Split('\n');
            txbHoTen.Text = ReadFromFile[0];
            string[] info = ReadFromFile[1].Split(',');
            txbTrangThai.Text += GetSubStr(info[0]);
            txbGioiTinh.Text += GetSubStr(info[1]);
            txbNgayVaoTruong.Text += GetSubStr(info[2]);
            txbMaHoSo.Text += GetSubStr(info[3]);
            txbKhoa_year.Text += GetSubStr(info[4]);
            txbCoSo.Text += GetSubStr(info[5]);
            txbBacDaoTao.Text += GetSubStr(info[6]);
            txbLoaiDaotao.Text += GetSubStr(info[7]);
            txbNganh.Text += GetSubStr(info[8]);
            txbChuyenNganh.Text += GetSubStr(info[9]);
            txbKhoa.Text += GetSubStr(info[10]);
            txbLop.Text += GetSubStr(info[11]);
            txbChucVu.Text += GetSubStr(info[12]);
            txbCongTacDoan.Text += GetSubStr(info[13]);
        }

        private string GetSubStr(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;
            return inputString.Substring(inputString.IndexOf(':') + 1);
        }

        private List<ObjectScore> LoadScore()
        {
            string workingPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Data", "Score.txt");
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

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //var darkwindow = new Window()
        //    //{
        //    //    Background = Brushes.Black,
        //    //    Opacity = 0.4,
        //    //    AllowsTransparency = true,
        //    //    WindowStyle = WindowStyle.None,
        //    //    WindowState = WindowState.Maximized,
        //    //    Topmost = true
        //    //};
        //    //darkwindow.Show();
        //    FrameworkElement parent = this;
        //    while (parent.Parent != null)
        //    {
        //        parent = parent.Parent as FrameworkElement;
        //    }
        //    Window w = parent as Window;
        //    w.Opacity = 0.4;
        //    var loading = new Loading();
        //    loading.ShowDialog();
        //}

        //FrameworkElement GetwindowParent(UserControl p)
        //{
        //    FrameworkElement parent = p;
        //    while (parent.Parent != null)
        //    {
        //        parent = parent.Parent as FrameworkElement;
        //    }
        //    return parent;
        //}
    }
}
