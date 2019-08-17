using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
//using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace ToolAndroid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region GlobalVariable
        static bool isDrawing;
        Point startPoint, endPoint;
        private bool isLoaded = false;
        static int count = 0;
        bool isChooseLocation = false;
        #endregion

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
            string currentPath = Path.Combine(Directory.GetCurrentDirectory(), "path.txt");
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
                //MessageBox.Show(String.Format("width {0}\n height {1}", cvPicture.Width, cvPicture.Height));
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void CvPicture_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isChooseLocation)
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    Point p = e.GetPosition(cvPicture);
                    MessageBox.Show(String.Format("Tọa độ\nX {0}\nY {1}", p.X, p.Y));
                }
                else return;
            }
            else
            {
                isDrawing = true;
                startPoint = e.GetPosition(cvPicture);
            }
        }

        private Rectangle DrawingRectangle(Point startPoint, Point endPoint, Canvas canvasPicture)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Stroke = Brushes.Red;
            rectangle.StrokeThickness = 3;
            if ((endPoint.X < endPoint.Y) || (endPoint.Y < startPoint.Y))
            {
                isDrawing = false;
                return null;
            }
            else
            {
                rectangle.Width = Math.Abs(endPoint.X - startPoint.X);
                rectangle.Height = Math.Abs(endPoint.Y - startPoint.Y);
                Canvas.SetLeft(rectangle, startPoint.X);
                Canvas.SetTop(rectangle, startPoint.Y);
                return rectangle;
            }
        }

        private void CvPicture_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDrawing = false;
        }

        private void BtnCut_Click(object sender, RoutedEventArgs e)
        {

            //if (cvPicture.Children.Count != 0)
            //{
            //    DrawingImage drawingImage = new DrawingImage();
            //    drawingImage.
            //}
            //else return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isChooseLocation) isChooseLocation = false;
            else isChooseLocation = true;
        }

        private void BtnCut_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteSelection_Click(object sender, RoutedEventArgs e)
        {
            cvPicture.Children.Clear();
            isDrawing = false;
        }

        private void CvPicture_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isChooseLocation)
                this.Cursor = Cursors.Pen;
        }

        private void CvPicture_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDrawing)
            {
                if (e.LeftButton != System.Windows.Input.MouseButtonState.Pressed)
                {
                    isDrawing = false;
                    return;
                }
                endPoint = e.GetPosition(cvPicture);
                if (endPoint.X < startPoint.X) return;
                //if (cvPicture.Children.Count > 1) cvPicture.Children.Clear();
                //if (DrawingRectangle(startPoint, endPoint, cvPicture) != null)
                //    cvPicture.Children.Add(DrawingRectangle(startPoint, endPoint, cvPicture));
                DrawingGroup dg = new DrawingGroup();
                Image img = new Image();
                using (DrawingContext dc = dg.Open())
                {
                    img.Source = null;
                    double width = Math.Abs(endPoint.X - startPoint.X);
                    double height = Math.Abs(endPoint.Y - startPoint.Y);
                    Rect rect = new Rect(startPoint.X, startPoint.Y, width, height);
                    dc.DrawRectangle(null, new Pen(Brushes.CornflowerBlue, 2), rect);
                }
                DrawingImage dimg = new DrawingImage(dg);
                img.Source = dimg;
                Canvas.SetLeft(img, startPoint.X);
                Canvas.SetTop(img, startPoint.Y);
                if (cvPicture.Children.Count > 0) cvPicture.Children.Clear();
                cvPicture.Children.Add(img);

            }
            else if (isChooseLocation)
            {

            }
            else return;
        }
    }
}
