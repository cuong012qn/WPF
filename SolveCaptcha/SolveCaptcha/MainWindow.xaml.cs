using System.Drawing;
using System.Windows;

namespace SolveCaptcha
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();
            //EEEEEE
            string image = @"C:\Users\cuong\Desktop\test.png";
            Bitmap bm = new Bitmap(image);
            Bitmap bmp = new Bitmap(bm.Width, bm.Height);
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {
                    Color cl = bm.GetPixel(i, j);
                    if (cl.R.Equals(238) && cl.G.Equals(238) && cl.B.Equals(238))
                    {
                        bmp.SetPixel(i, j, Color.White);
                    }
                    else bmp.SetPixel(i, j, cl);
                }
            }
            bmp.Save(@"C:\Users\cuong\Desktop\new.png");
        }
    }
}
