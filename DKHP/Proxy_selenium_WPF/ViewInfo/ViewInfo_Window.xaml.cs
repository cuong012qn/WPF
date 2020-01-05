using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;
using Path = System.IO.Path;

namespace Proxy_selenium_WPF
{
    /// <summary>
    /// Interaction logic for ViewInfo_Window.xaml
    /// </summary>
    public partial class ViewInfo : Window
    {
        private readonly string currentFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        public ViewInfo()
        {
            InitializeComponent();
        }

        public ViewInfo(string MSSV)
        {
            InitializeComponent();
            LoadImage(MSSV);
            LoadData();
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

        private void LoadData()
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
    }
}
