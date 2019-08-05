using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;
using System.Threading;

namespace ToolAndroid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static bool isDrawing;
        Point startPoint, endPoint;

        public MainWindow()
        {
            InitializeComponent();
            Core.CleanPicture();
            //Core.FindImage();
            //cvPicture.Background = new ImageBrush(new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Capture", "image.png"),UriKind.Relative)));
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
                lvInfomation.ItemsSource = Core.GetDevices(tbPathNox.Text.Replace("Nox.exe", ""));
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
            cvPicture.Background = null;
            cvPicture.Children.Clear();
            isLoaded = false;
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
            {
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

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cvPicture.Background = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void CvPicture_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!isDrawing)
            {
                startPoint = e.GetPosition(cvPicture);
                isDrawing = true;
            }
            else return;
        }

        private void DrawingRectangle(Point startPoint, Point endPoint, Canvas canvasPicture)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Stroke = Brushes.Red;
            rectangle.StrokeThickness = 4;
            if ((endPoint.X < endPoint.Y) || (endPoint.Y < startPoint.Y))
            {
                isDrawing = false;
                return;
            }
            else
            {
                rectangle.Width = Math.Abs(endPoint.X - startPoint.X);
                rectangle.Height = Math.Abs(endPoint.Y - startPoint.Y);
                Canvas.SetLeft(rectangle, startPoint.X);
                Canvas.SetTop(rectangle, startPoint.Y);
                canvasPicture.Children.Add(rectangle);
            }
        }

        private void CvPicture_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isDrawing) isDrawing = false;
            else return;
        }

        private void CvPicture_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDrawing)
            {
                
                endPoint = e.GetPosition(cvPicture);
                if (cvPicture.Children.Count > 1) cvPicture.Children.Clear();
                DrawingRectangle(startPoint, endPoint,cvPicture);
            }
            else return;
        }
    }
}
