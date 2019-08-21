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
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;

namespace HelperTool
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

        private static void Core(string duongdan)
        {
            var chromeSV = ChromeDriverService.CreateDefaultService();
            chromeSV.HideCommandPromptWindow = true;
            IWebDriver brower = new ChromeDriver(chromeSV);
            brower.Navigate().GoToUrl(duongdan);
            IList<IWebElement> questions = brower.FindElements(By.ClassName("freebirdFormviewerViewNumberedItemContainer"));
            // MessageBox.Show(questions.Count.ToString());
            foreach (IWebElement question in questions)
            {
                IList<IWebElement> RadioBoxs = question.FindElements(By.ClassName("quantumWizTogglePaperradioRadioContainer"));
                if (RadioBoxs.Count > 0)
                {
                    Thread.Sleep(100);
                    Random random = new Random();
                    int randomNumber = random.Next(0, RadioBoxs.Count);
                    RadioBoxs[randomNumber].Click();
                }
                else
                {
                    IList<IWebElement> checkboxs = question.FindElements(By.ClassName("freebirdFormviewerViewItemsCheckboxOptionContainer"));
                    if (checkboxs.Count > 0)
                    {
                        Random rn = new Random();
                        foreach (IWebElement checkbox in checkboxs)
                        {
                            if (rn.Next(2).Equals(1)) checkbox.Click();
                        }
                    }
                }
            }
            brower.FindElement(By.ClassName("freebirdFormviewerViewNavigationButtons")).Click();
            brower.Dispose();
        }

        private void BtnBatDau_Click(object sender, RoutedEventArgs e)
        {
            if (txbDuongDan.Text.Contains("docs.google.com/forms/") ||
                txbDuongDan.Text.Contains("forms.gle"))
            {
                int soluong = int.MinValue, luong = int.MinValue;
                string duongdan = txbDuongDan.Text;
                if (string.IsNullOrEmpty(duongdan)) return;
                try
                {
                    soluong = Convert.ToInt32(txbSoLuong.Text);
                    luong = Convert.ToInt32(txbSoluong.Text);
                }
                catch
                {
                    return;
                }
                if (soluong < 0 || luong < 0) return;
                try
                {
                    for (int j = 0; j < luong; j++)
                    {
                        for (int i = 0; i < soluong; i++)
                        {
                            new Thread(() => Core(duongdan)).Start();
                        }
                    }
                }
                catch(Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                //for (int i = 0; i < soluong; i++)
                //{
                //    Core(duongdan);
                //}
                //MessageBox.Show("Đã xong", "Thông báo");
            }
            else MessageBox.Show("Không đúng đường dẫn", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
