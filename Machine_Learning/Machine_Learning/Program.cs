using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.Structure;
using System.Diagnostics;
using Emgu.CV.OCR;
using System.Drawing.Imaging;
using Emgu.CV.Util;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;

namespace Machine_Learning
{
    class Program
    {
        static readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        static void Main(string[] args)
        {
            if (!Directory.Exists("./Data"))
            {
                Directory.CreateDirectory("./Data");
            }
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string[] getFiles = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
            Console.WriteLine(getFiles[0]);
            temp();
            Console.ReadKey();
        }

        private static void GetCaptcha(int start, int end)
        {

            ChromeDriverService chromeDriver = ChromeDriverService.CreateDefaultService();
            chromeDriver.HideCommandPromptWindow = true;
            ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--headless");
            IWebDriver webDriver = new ChromeDriver(chromeDriver, chromeOptions);
            webDriver.Navigate().GoToUrl(@"http://dkhp.ntt.edu.vn/Account/Login");
            for (int i = start; i < end; i++)
            {
                Screenshot ss = (webDriver as ITakesScreenshot).GetScreenshot();
                ss.SaveAsFile(string.Format("./Data/ss_{0}.png", i), ScreenshotImageFormat.Png);
                IWebElement element = webDriver.FindElement(By.Id("CaptchaImage"));
                Point point = element.Location;
                int heigt = element.Size.Height;
                int width = element.Size.Width;
                Rectangle rectangle = new Rectangle(point, new Size(width, heigt));
                using (Bitmap bm = new Bitmap(string.Format("./Data/ss_{0}.png", i)))
                {
                    Bitmap result = CropImage(bm, rectangle);
                    result.Save(string.Format("./Data/crop_{0}.png", i));
                }
                File.Delete(MergePath(string.Format("ss_{0}.png", i)));
                webDriver.Navigate().Refresh();
            }
            webDriver.Dispose();
        }

        private static string MergePath(string pathOfImage)
        {
            return Path.Combine(path, pathOfImage);
        }

        private static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            return bmp;
        }

        static void temp()
        {

            //Image image = Image.FromFile(Path.Combine(path, "Image_1.bmp"));
            Bitmap bm = new Bitmap(Path.Combine(path, "crop_0.png"));
            Image<Bgr, byte> image = new Image<Bgr, byte>(bm);
            CvInvoke.FastNlMeansDenoisingColored(image, image, 30, 30, 7, 21);
            CvInvoke.Threshold(image, image, 210, 255, Emgu.CV.CvEnum.ThresholdType.ToZero);
            Image<Gray, byte> result = image.Convert<Gray, byte>();
            //result.Save(Path.Combine(Directory.GetCurrentDirectory(), "temp1.png"));
            //image.Save(Path.Combine(Directory.GetCurrentDirectory(), "temp.png"));
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    int G = (int)image.Data[j, i, 1];
                    if (G.Equals(0))
                    {
                        image.Data[j, i, 0] = 0;
                        image.Data[j, i, 1] = 0;
                        image.Data[j, i, 2] = 0;
                    }
                    else
                    {
                        image.Data[j, i, 0] = 255;
                        image.Data[j, i, 1] = 255;
                        image.Data[j, i, 2] = 255;
                    }
                }
            }
            image.Save(Path.Combine(Directory.GetCurrentDirectory(), "temp.png"));
            //Bitmap bitmap = new Bitmap(result);
            //for (int i = 0; i < bitmap.Width; i++)
            //{
            //    for (int j = 0; j < bitmap.Height; j++)
            //    {
            //        Color color = bitmap.GetPixel(i, j);
            //        if (color.G == 0)
            //        {
            //            bitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0));
            //        }
            //        else bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
            //    }
            //}
            //bitmap.Save(Path.Combine(Directory.GetCurrentDirectory(), "newImage.png"), ImageFormat.Png);
            //bitmap.Dispose();
        }
    }
}
