using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tesseract;
using System.Security.Cryptography;
using Cookie = OpenQA.Selenium.Cookie;
using ImageFormat = System.Drawing.Imaging.ImageFormat;


namespace Proxy_selenium_WPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string PathOfFolderData = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        public MainWindow()
        {
            //MessageBox.Show(DataProvider.Instance.InsertData("Tz3R", "1f9265123c07f31f25647b6e85f17b12").ToString());
            //MessageBox.Show(DataProvider.Instance.FindValueCaptcha("7d81cdc496a5df3f2cbc47be7ef97fc0").Length.ToString());
            InitializeComponent();
            Core.Instance.CheckDirectory(PathOfFolderData);
            Core.Instance.ClearFile(PathOfFolderData);
            txbUsername.Text = "1800003309";
            pwbPassword.Password = "Cuongqn1@3";
        }

        //async void OpenBrower(string user, string password)
        //{
        //    string url = @"http://phongdaotao2.ntt.edu.vn/Default.aspx#";
        //    IWebDriver webDriver = SetOptionsChromeDriver();
        //    webDriver.Navigate().GoToUrl(url);
        //    IWebElement webElement = webDriver.FindElement(By.Id("imgSecurityCode"));
        //    Thread.Sleep(1500);
        //    DownloadImage(webElement.GetAttribute("src"));
        //    string pathImg = Path.Combine(Directory.GetCurrentDirectory(), "processingImage.png");
        //    Task<string> result = Task.Run(() => SolveCaptcha(pathImg));
        //    webDriver.FindElement(By.Id("ctl00_ucRight1_txtMaSV")).SendKeys(txbUsername.Text);
        //    webDriver.FindElement(By.Id("ctl00_ucRight1_txtMatKhau")).SendKeys(pwbPassword.Password);
        //    webDriver.FindElement(By.Id("txtSercurityCode")).SendKeys(await result);
        //    try
        //    {
        //        if (webDriver.SwitchTo().Alert().Text == "Mã bảo vệ không đúng.")
        //        {
        //            webDriver.Dispose();
        //            OpenBrower(txbUsername.Text, pwbPassword.Password);
        //        }
        //        else
        //        {
        //            if (webDriver.FindElement(By.Id("ctl00_ucRight1_btnChangePassword")).Text.Equals("Đổi mật khẩu"))
        //            {
        //                #region SaveCookie
        //                var cookie = webDriver.Manage().Cookies;//ctl00_ucRight1_btnChangePassword
        //                foreach (var c in cookie.AllCookies)
        //                {
        //                    string[] s = new string[] { c.Name, c.Domain, c.Expiry.ToString(), c.IsHttpOnly.ToString(), c.Secure.ToString(), c.Value, c.Path };
        //                    //File.WriteAllText("Cookie.data", c.Name + "\n");
        //                    //File.WriteAllText("Cookie.data", c.Domain + "\n");
        //                    //File.WriteAllText("Cookie.data", c.Expiry + "\n");
        //                    //File.WriteAllText("Cookie.data", c.IsHttpOnly + "\n");
        //                    //File.WriteAllText("Cookie.data", c.Secure + "\n");
        //                    //File.WriteAllText("Cookie.data", c.Value + "\n");
        //                    //File.WriteAllText("Cookie.data", c.Path + "\n");
        //                    File.WriteAllLines("Cookie.data", s);
        //                }
        //                #endregion
        //            }//Đổi mật khẩu
        //        }
        //    }
        //    catch (NoAlertPresentException)
        //    {

        //    }
        //}

        void UsingCookie(IWebDriver brower)
        {
            string file = File.ReadAllText("Cookie.data");
            string[] s = file.Split('\n');
            Cookie ck = new Cookie(s[0], s[5], s[1], s[6], null);
            brower.Manage().Cookies.AddCookie(ck);
            brower.Navigate().GoToUrl(@"http://phongdaotao3.ntt.edu.vn/");
        }


        /// <summary>
        /// CaptureScreen
        /// </summary>
        /// <param name="brower"></param>
        /// <param name="PathToSave">Đường dẫn lưu ảnh</param>
        /// <param name="NameOfImage">Tên ảnh (sử dụng tìm captcha)</param>
        /// <param name="elementCaptcha">ID Captcha</param>
        private void CaptureWebPage(IWebDriver brower, string PathToSave, string NameOfImage = "CaptureScreen", string elementCaptcha = null, bool isGetInfo = false)
        {
            ITakesScreenshot dscreenshot = brower as ITakesScreenshot;
            Screenshot screenshot = dscreenshot.GetScreenshot();

            Screenshot tempImage = screenshot;
            if (string.IsNullOrEmpty(elementCaptcha))
            {
                tempImage.SaveAsFile(Path.Combine(PathToSave, string.Format("{0}.png", NameOfImage)), ScreenshotImageFormat.Png);
            }
            else
            {
                tempImage.SaveAsFile(Path.Combine(PathOfFolderData, "temp.png"), ScreenshotImageFormat.Png);
                //"imgSecurityCode1"
                IWebElement getCaptcha = brower.FindElement(By.Id(elementCaptcha));

                System.Drawing.Point point = getCaptcha.Location;
                int width = getCaptcha.Size.Width;
                int height = getCaptcha.Size.Height;
                //{X = 766 Y = 411}
                if (isGetInfo)
                {
                    width -= 20;
                    point.X -= width;
                    height += 2;
                }
                Rectangle section = new Rectangle(point, new System.Drawing.Size(width, height));
                using (Bitmap source = new Bitmap(Path.Combine(PathOfFolderData, "temp.png")))
                {
                    Bitmap bitmap = CropImage(source, section);
                    bitmap.Save(Path.Combine(PathOfFolderData, string.Format("{0}.png", NameOfImage)), ImageFormat.Png);
                }
            }
        }

        private Bitmap CropImage(Bitmap source, Rectangle section)
        {
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            return bmp;
        }



        private void ProcessImage()
        {
            Bitmap bm = new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "image.bmp"));
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {
                    Color color = bm.GetPixel(i, j);
                    if (color.R == 238 && color.G == 238 && color.B == 238)
                    {
                        bm.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        int rgb = (color.R + color.G + color.B) / 3;
                        bm.SetPixel(i, j, Color.FromArgb(rgb, rgb, rgb));
                    }
                }
            }
            bm.Save("processingImage.png", ImageFormat.Png);
            bm.Dispose();
        }

        private async void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txbUsername.Text) &&
                 !string.IsNullOrEmpty(pwbPassword.Password.ToString()))
            {
                if (cbChooseFeature.SelectedIndex.Equals(0))
                {
                    //Dialog dialog = new Dialog("Tính năng đang được phát triển");
                    //dialog.ShowDialog();
                    ////OpenBrower(txbUsername.Text, pwbPassword.Password.ToString());
                    if (Core.Instance.LoginAsync(txbUsername.Text, pwbPassword.Password.ToString()))
                    {
                        MessageBox.Show("Login Successful");
                    }
                }
                else if (cbChooseFeature.SelectedIndex.Equals(1))
                {
                    var window = this as Window;
                    bool waitResult = false;
                    string username = txbUsername.Text;
                    Loading_Core loading = new Loading_Core();
                    Action action = new Action(() => loading.Dispatcher.Invoke(new Action(() => loading.ShowDialog())));
                    action.BeginInvoke(ar => loading = null, null);
                    window.Opacity = .4;
                    try
                    {
                        Task<bool> writeLog = Task.Run(() => Core.Instance.GetScoreAsync(username));
                        waitResult = await writeLog;
                    }
                    finally
                    {
                        loading.Close();
                        window.Opacity = 1.0;
                    }
                    string getDatetime = DateTime.Now.ToString();
                    if (waitResult.Equals(false))
                    {
                        //rtxbLog.Dispatcher.Invoke(() => textRange.Text +=
                        //(string.Format("{0}: Có lỗi khi đăng nhập, kiểm tra lại MSSV\n",
                        //getDatetime)));
                    }
                    else
                    {
                        var main = new Main(txbUsername.Text);
                        main.ShowDialog();
                    }
                }
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hi");
        }
    }
}
