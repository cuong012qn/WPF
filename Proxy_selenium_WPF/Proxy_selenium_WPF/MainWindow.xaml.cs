using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Tesseract;
using Cookie = OpenQA.Selenium.Cookie;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Proxy_selenium_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txbUsername.Text = "1800003309";
            pwbPassword.Password = "Cuongqn1@3";
            //GetResult();
        }


        private IWebDriver GetheadlessMode()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            var chromedriverServices = ChromeDriverService.CreateDefaultService();
            chromedriverServices.HideCommandPromptWindow = true;
            return new ChromeDriver(chromedriverServices, chromeOptions);
        }

        async void OpenBrower(string user, string password)
        {
            string url = @"http://phongdaotao2.ntt.edu.vn/Default.aspx#";
            IWebDriver webDriver = GetheadlessMode();
            webDriver.Navigate().GoToUrl(url);
            IWebElement webElement = webDriver.FindElement(By.Id("imgSecurityCode"));
            Thread.Sleep(1500);
            DownloadImage(webElement.GetAttribute("src"));
            string pathImg = Path.Combine(Directory.GetCurrentDirectory(), "processingImage.png");
            Task<string> result = Task.Run(() => SolveCaptcha(pathImg));
            webDriver.FindElement(By.Id("ctl00_ucRight1_txtMaSV")).SendKeys(txbUsername.Text);
            webDriver.FindElement(By.Id("ctl00_ucRight1_txtMatKhau")).SendKeys(pwbPassword.Password);
            webDriver.FindElement(By.Id("txtSercurityCode")).SendKeys(await result);
            try
            {
                if (webDriver.SwitchTo().Alert().Text == "Mã bảo vệ không đúng.")
                {
                    webDriver.Dispose();
                    OpenBrower(txbUsername.Text, pwbPassword.Password);
                }
                else
                {
                    if (webDriver.FindElement(By.Id("ctl00_ucRight1_btnChangePassword")).Text.Equals("Đổi mật khẩu"))
                    {
                        #region SaveCookie
                        var cookie = webDriver.Manage().Cookies;//ctl00_ucRight1_btnChangePassword
                        foreach (var c in cookie.AllCookies)
                        {
                            string[] s = new string[] { c.Name, c.Domain, c.Expiry.ToString(), c.IsHttpOnly.ToString(), c.Secure.ToString(), c.Value, c.Path };
                            //File.WriteAllText("Cookie.data", c.Name + "\n");
                            //File.WriteAllText("Cookie.data", c.Domain + "\n");
                            //File.WriteAllText("Cookie.data", c.Expiry + "\n");
                            //File.WriteAllText("Cookie.data", c.IsHttpOnly + "\n");
                            //File.WriteAllText("Cookie.data", c.Secure + "\n");
                            //File.WriteAllText("Cookie.data", c.Value + "\n");
                            //File.WriteAllText("Cookie.data", c.Path + "\n");
                            File.WriteAllLines("Cookie.data", s);
                        }
                        #endregion
                    }//Đổi mật khẩu
                }
            }
            catch (NoAlertPresentException)
            {

            }
        }

        void UsingCookie(IWebDriver brower)
        {
            string file = File.ReadAllText("Cookie.data");
            string[] s = file.Split('\n');
            Cookie ck = new Cookie(s[0], s[5], s[1], s[6], null);
            brower.Manage().Cookies.AddCookie(ck);
            brower.Navigate().GoToUrl(@"http://phongdaotao2.ntt.edu.vn/");
        }

        private string GetResult(string username)
        {
            //id textbox "MSSV"
            //id captcha "imgSecurityCode1"
            //id textboxCap "txtSercurityCode1"
            if (File.Exists("processingImage.png"))
            {
                File.Delete("processingImage.png");
            }

            #region test
            //string url = @"http://phongdaotao2.ntt.edu.vn/Default.aspx#";
            //IWebDriver webDriver = GetheadlessMode();
            //webDriver.Navigate().GoToUrl(url);
            //IWebElement webElement = webDriver.FindElement(By.Id("imgSecurityCode"));
            //Thread.Sleep(1500);
            //DownloadImage(webElement.GetAttribute("src"));
            //string pathImg = Path.Combine(Directory.GetCurrentDirectory(), "processingImage.png");
            //Task<string> result = Task.Run(() => SolveCaptcha(pathImg));
            //webDriver.FindElement(By.Id("ctl00_ucRight1_txtMaSV")).SendKeys(txbUsername.Text);
            //webDriver.FindElement(By.Id("ctl00_ucRight1_txtMatKhau")).SendKeys(pwbPassword.Password);
            //webDriver.FindElement(By.Id("txtSercurityCode")).SendKeys(await result);
            //try
            //{
            //    if (webDriver.SwitchTo().Alert().Text == "Mã bảo vệ không đúng.")
            //    {
            //        webDriver.Dispose();
            //        OpenBrower(txbUsername.Text, pwbPassword.Password);
            //    }
            //    else
            //    {
            //        if (webDriver.FindElement(By.Id("ctl00_ucRight1_btnChangePassword")).Text.Equals("Đổi mật khẩu"))
            //        {
            //            #region SaveCookie
            //            var cookie = webDriver.Manage().Cookies;//ctl00_ucRight1_btnChangePassword
            //            foreach (var c in cookie.AllCookies)
            //            {
            //                string[] s = new string[] { c.Name, c.Domain, c.Expiry.ToString(), c.IsHttpOnly.ToString(), c.Secure.ToString(), c.Value, c.Path };
            //                //File.WriteAllText("Cookie.data", c.Name + "\n");
            //                //File.WriteAllText("Cookie.data", c.Domain + "\n");
            //                //File.WriteAllText("Cookie.data", c.Expiry + "\n");
            //                //File.WriteAllText("Cookie.data", c.IsHttpOnly + "\n");
            //                //File.WriteAllText("Cookie.data", c.Secure + "\n");
            //                //File.WriteAllText("Cookie.data", c.Value + "\n");
            //                //File.WriteAllText("Cookie.data", c.Path + "\n");
            //                File.WriteAllLines("Cookie.data", s);
            //            }
            //            #endregion
            //        }//Đổi mật khẩu
            //    }
            //}
            //catch (NoAlertPresentException)
            //{

            //}
            #endregion
            string url = @"http://phongdaotao3.ntt.edu.vn/XemKetQuaHocTap.aspx?MenuID=354";
            IWebDriver webDriver = GetheadlessMode();
            webDriver.Navigate().GoToUrl(url);
            Thread.Sleep(250);
            IWebElement webElement = webDriver.FindElement(By.Id("imgSecurityCode"));
            //webDriver.FindElement(By.Id("MSSV")).SendKeys(txbUsername.Text);
            //Thread.Sleep(1500);
            //DownloadImage(webElement.GetAttribute("src"));
            CaptureWebPage(webDriver, "imgSecurityCode1");
            Task<string> waitResult = Task.Run(() => SolveCaptcha("captcha.png"));
            webDriver.FindElement(By.Id("MSSV")).SendKeys(username);
            webDriver.FindElement(By.Id("txtSercurityCode1")).SendKeys(waitResult.Result.Trim().Replace("\n", ""));
            webDriver.FindElement(By.XPath(@"/html/body/form[@id='aspnetForm']/div[@id='shadow']/div[@id='main_container']/div[@class='col-full clearfix']/div[@class='col-left']/div[@class='mod news-list']/div[@id='login']/table/tbody/tr/td/div[@id='loginForm']/table/tbody/tr[4]/td[3]/input[@class='button']")).Click();
            try
            {
                string errorCap = webDriver.SwitchTo().Alert().Text;

                if (webDriver.SwitchTo().Alert().Text.Equals("Mã bảo vệ không trùng khớp."))
                {
                    string temp = webDriver.SwitchTo().Alert().Text;
                    webDriver.Dispose();
                    return temp;
                }
                //temp(webDriver);
                //IList<IWebElement> lstElement = webDriver.FindElements(By.ClassName("grid.grid-color2.tblKetQuaHocTap"));
                //string res = string.Empty;
                //foreach (IWebElement element in lstElement)
                //{
                //    res += element.Text + "\n";
                //}
                //MessageBox.Show(res);

            }
            catch (NoAlertPresentException)
            {
                CaptureWebPage(webDriver);
                webDriver.Dispose();
                return "Đăng nhập thành công";
            }
            return null;
        }


        private void CaptureWebPage(IWebDriver brower, string elementCaptcha = null)
        {
            ITakesScreenshot dscreenshot = brower as ITakesScreenshot;
            Screenshot screenshot = dscreenshot.GetScreenshot();

            Screenshot tempImage = screenshot;
            if (string.IsNullOrEmpty(elementCaptcha))
            {
                tempImage.SaveAsFile("capture.png", ScreenshotImageFormat.Png);
            }
            else
            {
                tempImage.SaveAsFile(@"temp.png", ScreenshotImageFormat.Png);
                //"imgSecurityCode1"
                IWebElement getCaptcha = brower.FindElement(By.Id(elementCaptcha));

                System.Drawing.Point point = getCaptcha.Location;
                int width = getCaptcha.Size.Width;
                int height = getCaptcha.Size.Height;

                Rectangle section = new Rectangle(point, new System.Drawing.Size(width, height));
                using (Bitmap source = new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "temp.png")))
                {
                    Bitmap bitmap = CropImage(source, section);
                    bitmap.Save(@"captcha.png");
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

        void temp(IWebDriver webDriver)
        {
            ITakesScreenshot takesScreenshot = webDriver as ITakesScreenshot;
            Screenshot screenshot = takesScreenshot.GetScreenshot();
            screenshot.SaveAsFile("screenshoot.png", ScreenshotImageFormat.Png);
            Process.Start("screenshot.png");
        }

        void CleanConnection(IWebDriver webDriver)
        {
            webDriver.Close();
            webDriver.Quit();
            webDriver.Dispose();
        }

        void ProcessImage()
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

        private string SolveCaptcha(string image)
        {
            Bitmap bm = new Bitmap(image);
            string result = string.Empty;
            //string datapath = Path.Combine(Directory.GetCurrentDirectory(), "eng.traineddata");
            using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractOnly))
            {
                using (var img = Tesseract.PixConverter.ToPix(bm))
                {
                    using (var page = engine.Process(img))
                    {
                        result = page.GetText();
                    }
                }
            }
            bm.Dispose();
            return result;
        }

        bool DownloadImage(string url)
        {
            string currentFolder = Directory.GetCurrentDirectory();
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    using (Stream stream = webClient.OpenRead(url))
                    {
                        using (Bitmap bitmap = new Bitmap(stream))
                        {
                            bitmap.Save(currentFolder + @"\image.bmp", ImageFormat.Bmp);
                        }
                    }
                }
                ProcessImage();
                File.Delete(Path.Combine(currentFolder, "image.bmp"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AddtextRTB(string text, TextRange textRange)
        {
            //TextRange textRange = new TextRange(rtxbLog.Document.ContentStart, rtxbLog.Document.ContentEnd);
            Dispatcher.Invoke(() => textRange.Text += text);
            //Task task = Task.Run(() => textRange.Text += text);
            //await task;
        }

        static int count = 1;

        private async void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txbUsername.Text) &&
                 !string.IsNullOrEmpty(pwbPassword.Password.ToString()))
            {
                if (cbChooseFeature.SelectedIndex.Equals(0))
                {
                    OpenBrower(txbUsername.Text, pwbPassword.Password.ToString());
                }
                else if (cbChooseFeature.SelectedIndex.Equals(1))
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    try
                    {
                        string username = txbUsername.Text;
                        Task<string> writeLog = Task.Run(() => GetResult(username));
                        TextRange textRange = new TextRange(rtxbLog.Document.ContentStart, rtxbLog.Document.ContentEnd);
                        if (writeLog.Result.Equals("Mã bảo vệ không trùng khớp."))
                        {
                            //AddtextRTB(string.Format("Có lỗi khi đăng nhập, đăng nhập lại lần {0}",count),textRange);
                            // textRange.Text += ("Có lỗi khi đăng nhập, đăng nhập lại lần " + count);
                            this.Dispatcher.Invoke(() => textRange.Text += (string.Format("Có lỗi khi đăng nhập, đăng nhập lại lần {0}", count)));
                            count++;
                            BtnCheck_Click(sender, e);
                        }
                        else
                        {
                            //textRange.Text += await writeLog;
                            string result = await writeLog;
                            this.Dispatcher.Invoke(() => (textRange.Text += result));
                            //AddtextRTB(await writeLog,textRange);
                        }
                    }
                    finally
                    {
                        Mouse.OverrideCursor = null;
                    }
                    //GetResult();
                }
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
