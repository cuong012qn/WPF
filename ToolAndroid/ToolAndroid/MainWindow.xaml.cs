using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace ToolAndroid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Core.CleanPicture();
            Core.FindImage();
            //cvPicture.Background = new ImageBrush(new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Capture", "image.png"),UriKind.Relative)));
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Capture")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Capture"));
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Capture", "image.png")))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Capture", "image.png"));
            }
            tbPathNox.Text = Core.ReadpathFromFile();
            lvInfomation.ItemsSource = Core.GetDevices(tbPathNox.Text.Replace("Nox.exe", ""));
        }

        private void BtnOpenNox_Click(object sender, RoutedEventArgs e)
        {
            string currentPath = Directory.GetCurrentDirectory() + @"\path.txt";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Contains("Nox.exe"))
            {
                if (Core.WritepathToFile(openFileDialog.FileName))
                    tbPathNox.Text = openFileDialog.FileName;
                else MessageBox.Show("Kiểm tra lại đường dẫn Nox", "Thông báo");
            }
            else tbPathNox.Text = "";
            if (!string.IsNullOrEmpty(tbPathNox.Text))
            {
                lvInfomation.ItemsSource = Core.GetDevices(tbPathNox.Text);
            }
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TbPathNox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void CbSelect_Checked(object sender, RoutedEventArgs e)
        {
            var temp = (((CheckBox)sender).DataContext as Devices);
            if (!temp.isChecked) temp.isChecked = true;
        }

        private void CbSelect_Unchecked(object sender, RoutedEventArgs e)
        {
            var temp = (((CheckBox)sender).DataContext as Devices);
            if (temp.isChecked) temp.isChecked = false;
        }

        private bool isLoaded = false;
        static int count = 0;
        private void BtnCaptureScreen_Click(object sender, RoutedEventArgs e)
        {
            if (cvPicture.Background != null)
                cvPicture.Background = null;
            if (cvPicture.Background == null)
            {
                if (tbPathNox.Text.Contains("Nox.exe"))
                {
                    Devices devices = ((Button)sender).DataContext as Devices;
                    if (devices.isChecked)
                    {
                        Core.ScreenCap(tbPathNox.Text.Replace("Nox.exe", ""), devices, String.Format("image_{0}", count));
                        isLoaded = true;
                    }
                }
                if (isLoaded)
                    LoadtoCanvas(cvPicture);
                count++;
            }
        }

        void LoadtoCanvas(Canvas image)
        {
            if (image.Background == null)
            {
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bmImage = new BitmapImage();
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnDemand;
                bmImage.UriSource = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Capture", String.Format("image_{0}.png", count)), UriKind.Relative);
                bmImage.EndInit();
                cvPicture.Height = bmImage.Height;
                cvPicture.Width = bmImage.Width;
                imageBrush.ImageSource = bmImage;
                image.Background = imageBrush;
            }
            else return;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //MessageBox.Show(Core.CleanPicture().ToString());
            //ExecuteCommand(tbPathNox.Text.Replace("Nox.exe", ""), "disconnect all");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cvPicture.Background = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Core.CleanPicture();
        }

    }
}
