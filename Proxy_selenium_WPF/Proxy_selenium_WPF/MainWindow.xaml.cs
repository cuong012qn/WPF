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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

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
        }


        static void OpenBrower(string user, string password)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password)) return;
            string url = "https://sso.garena.com/ui/login?app_id=10100&redirect_uri=https%3A%2F%2Faccount.garena.com%2F%3Flocale_name%3DVN&locale=vi-VN";
            IWebDriver web = new ChromeDriver();
            web.Navigate().GoToUrl(url);
            if (string.IsNullOrEmpty(web.Title)) return;
            Thread.Sleep(750);
            web.FindElement(By.Name("username")).SendKeys(user);
            Thread.Sleep(750);
            web.FindElement(By.Name("password")).SendKeys(password);
            //Thread.Sleep(750);
            web.FindElement(By.Name("login")).Click();
            Thread.Sleep(750);
            //xpath = /html/body/div[1]/div[2]/div[1]/form[1]/div[4]/div[2]/span[1]/img/@src
            string urlCaptcha = web.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/form[1]/div[4]/div[2]/span[1]/img")).GetAttribute("src");
            if (string.IsNullOrEmpty(urlCaptcha))
            {

            }
            else
            {
                DownloadImage(urlCaptcha);
                Process.Start("image.bmp");
            }
        }

        static void DownloadImage(string url)
        {
            string currentFolder = Directory.GetCurrentDirectory();
            using (WebClient webClient = new WebClient())
            {
                using (Stream stream = webClient.OpenRead(url))
                {
                    using (Bitmap bitmap = new Bitmap(stream))
                    {
                        bitmap.Save(currentFolder + @"\image.bmp",ImageFormat.Bmp);
                    }
                }
            }
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txbUsername.Text) &&
                 !string.IsNullOrEmpty(pwbPassword.Password.ToString()))
            {
                string username = txbUsername.Text;
                string password = pwbPassword.Password.ToString();
                Thread thread = new Thread(() => OpenBrower(username, password));
                thread.Start();
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
