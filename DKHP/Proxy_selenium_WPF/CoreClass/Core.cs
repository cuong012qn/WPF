using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using System.Collections.Generic;
using System.Threading;

namespace Proxy_selenium_WPF
{
    public class Core
    {
        private readonly string PathOfFolderData = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private static Core _instance;
        private Core()
        {

        }

        public static Core Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Core();
                return _instance;
            }
            private set => _instance = value;
        }

        public void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void ClearFile(string Folder)
        {
            string[] getFileinFolder = Directory.GetFiles(Folder);
            foreach (string s in getFileinFolder)
            {
                File.Delete(s);
            }
        }

        public string SolveCaptcha(string pathOfImage)
        {
            Bitmap bm = new Bitmap(pathOfImage);
            string result = string.Empty;
            //string datapath = Path.Combine(Directory.GetCurrentDirectory(), "eng.traineddata");
            using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractOnly))
            {
                engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
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

        public string GetMD5(string inputString)
        {
            using (MD5 md5 = MD5.Create())
            {
                return GetMd5Hash(md5, inputString);
            }
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public bool VerifyMd5Hash(string input, string hash)
        {
            using (MD5 md5 = MD5.Create())
            {

                string hashOfInput = GetMd5Hash(md5, input);
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                if (0 == comparer.Compare(hashOfInput, hash))
                    return true;
                else return false;
            }
        }

        public bool DownloadImage(string url, string PathToSave, string NameToSave = "ImageDownload")
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    using (Stream stream = webClient.OpenRead(url))
                    {
                        using (Bitmap bitmap = new Bitmap(stream))
                        {
                            bitmap.Save(Path.Combine(PathToSave, string.Format("{0}.png", NameToSave)), ImageFormat.Png);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private IWebDriver GetOptionsChromeDriver(List<string> arguments = null)
        {
            var Coptions = new ChromeOptions();
            var CDServices = ChromeDriverService.CreateDefaultService();
            if (arguments == null)
            {
                //chromeOptions.AddArgument("--headless");
                CDServices.HideCommandPromptWindow = true;
            }
            else
            {
                foreach (string argument in arguments)
                {
                    Coptions.AddArgument(argument);
                }
                CDServices.HideCommandPromptWindow = true;
            }
            return new ChromeDriver(CDServices, Coptions);
        }

        public async Task<bool> GetScoreAsync(string username)
        {
            //id textbox "MSSV"
            //id captcha "imgSecurityCode1"
            //id textboxCap "txtSercurityCode1"
            string url = @"http://phongdaotao3.ntt.edu.vn/XemKetQuaHocTap.aspx?MenuID=354";
            //IWebDriver webDriver = GetOptionsChromeDriver(new List<string>() { "--headless" });
            IWebDriver webDriver = GetOptionsChromeDriver();
            webDriver.Navigate().GoToUrl(url);
            //visibilityOfElementLocated
            Thread.Sleep(250);
            try
            {
                webDriver.FindElement(By.Id("MSSV")).SendKeys(username);
            }
            catch (NoSuchElementException)
            {
                webDriver.Dispose();
                return false;
            }
            do
            {
                string ResultCaptcha = string.Empty;
                string md5GetFromWeb = webDriver.FindElement(By.Id("txtSecurityCodeValue1")).GetAttribute("Value");
                string KeyFromMd5 = DataProvider.Instance.FindValueCaptcha(md5GetFromWeb);
                if (!Convert.ToInt32(KeyFromMd5).Equals(-1))
                {
                    webDriver.FindElement(By.Id("txtSercurityCode1")).SendKeys(KeyFromMd5 + Environment.NewLine);
                    File.WriteAllText(Path.Combine(PathOfFolderData, "Score.txt"), GetTableScore(webDriver));
                    File.WriteAllText(Path.Combine(PathOfFolderData, "Info.txt"), GetTableInfo(webDriver));
                    webDriver.Dispose();
                    return true;
                }
                else
                {
                    Task<bool> downloadImage = Task.Run(() => Core.Instance.DownloadImage(webDriver.FindElement(By.Id("imgSecurityCode1")).GetAttribute("src"), PathOfFolderData, "Captcha"));
                    await downloadImage;
                    if (downloadImage.IsCompleted)
                    {
                        if (downloadImage.Result.Equals(true))
                        {
                            Task<string> waitResult = Task.Run(() => Core.Instance.SolveCaptcha(Path.Combine(PathOfFolderData, "Captcha.png")));
                            ResultCaptcha = await waitResult;
                            Thread.Sleep(500);
                            webDriver.FindElement(By.Id("txtSercurityCode1")).SendKeys(ResultCaptcha.Trim() + Environment.NewLine);
                            try
                            {
                                if (webDriver.SwitchTo().Alert().Text.Equals("Mã số sinh viên không tồn tại."))
                                {
                                    webDriver.SwitchTo().Alert().Accept();
                                    webDriver.Dispose();
                                    return false;
                                }
                                else webDriver.SwitchTo().Alert().Accept();
                            }
                            catch (NoAlertPresentException)
                            {
                                DataProvider.Instance.InsertData(ResultCaptcha.Split('\n')[0], md5GetFromWeb);
                                //DataProvider.Instance.InsertData()
                                File.WriteAllText(Path.Combine(PathOfFolderData, "Score.txt"), GetTableScore(webDriver));
                                File.WriteAllText(Path.Combine(PathOfFolderData, "Info.txt"), GetTableInfo(webDriver));
                                webDriver.Dispose();
                                return true;
                            }
                        }
                    }
                    //CaptureWebPage(webDriver, PathOfFolderData, "Captcha", "imgSecurityCode1");
                }
            } while (true);
        }

        public bool LoginAsync(string username, string password, string mode = null)
        {
            string url = @"http://phongdaotao3.ntt.edu.vn/Default.aspx#";
            if (string.IsNullOrEmpty(mode))
            {
                //IWebDriver brower = GetOptionsChromeDriver(new List<string>() { "--headless" });
                IWebDriver brower = GetOptionsChromeDriver();
                brower.Navigate().GoToUrl(url);
                brower.FindElement(By.Id("ctl00_ucRight1_txtMaSV")).SendKeys(username);
                brower.FindElement(By.Id("ctl00_ucRight1_txtMatKhau")).SendKeys(password);
                bool isDone = false;
                do
                {
                    Core.Instance.DownloadImage(brower.FindElement(By.Id("imgSecurityCode")).GetAttribute("src"), PathOfFolderData,
                         "Captcha");
                    //CaptureWebPage(brower, PathOfFolderData, "Captcha", "txtSercurityCode", true);
                    string result = Core.Instance.SolveCaptcha(Path.Combine(PathOfFolderData, "Captcha.png")).Split('\n')[0];
                    IWebElement txtElement = brower.FindElement(By.Id("txtSercurityCode"));
                    txtElement.Clear();
                    txtElement.SendKeys(result);
                    brower.FindElement(By.Id("ctl00_ucRight1_btnLogin")).Click();
                    try
                    {
                        if (brower.SwitchTo().Alert().Text.Equals("Mã số sinh viên không tồn tại."))
                        {
                            brower.SwitchTo().Alert().Accept();
                            brower.Dispose();
                            return false;
                        }
                        else brower.SwitchTo().Alert().Accept();
                    }
                    catch (NoAlertPresentException)
                    {
                        //brower.Dispose();
                        isDone = true;
                    }
                } while (isDone != true);
                return true;
            }
            return false;
        }

        private string GetTableScore(IWebDriver brower)
        {
            string rowsData = "";
            IWebElement element = brower.FindElement(By.XPath(@"//*[@id='main_container']/div[4]/div[1]/div[2]/table[1]"));
            List<IWebElement> listTrElem = new List<IWebElement>(element.FindElements(By.TagName("tr")));

            foreach (var elemTr in listTrElem)
            {
                List<IWebElement> lstTdElem = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
                if (lstTdElem.Count > 1)
                {
                    foreach (var elemTd in lstTdElem)
                    {
                        rowsData = rowsData + elemTd.Text + ",";
                    }
                }
                rowsData += "\n";
            }
            return rowsData.Trim();
        }

        private string GetTableInfo(IWebDriver brower)
        {
            string result = string.Empty;
            //xPath //*[@class='group-right']/table[@class='none-grid']
            IWebElement element = brower.FindElement(By.XPath("//*[@class='group-right']/table[@class='none-grid']"));
            List<IWebElement> listTrElem = new List<IWebElement>(element.FindElements(By.TagName("tr")));
            foreach (var elemTr in listTrElem)
            {
                List<IWebElement> lstTdElem = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
                if (lstTdElem.Count > 1)
                {
                    foreach (var elemTd in lstTdElem)
                    {
                        result += (elemTd.Text + ",");
                    }
                }
            }

            //GetName student
            IWebElement getname = brower.FindElement(By.XPath("//*[@class='main-content']/div[@class='title-group']"));
            return getname.Text.Split('\n')[1] + Environment.NewLine + result;
        }
    }
}
