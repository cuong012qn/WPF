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

namespace Form
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<GiaLap> gl = new List<GiaLap>();
            gl.Add(new GiaLap { isChecked = false, TrinhGiaLap = "Nox", TrangThai = "OFFlINE", An = string.Empty });
            lvGiaLap.ItemsSource = gl;
        }

        class GiaLap
        {
            public bool isChecked { get; set; }
            public string TrinhGiaLap { get; set; }
            public string TrangThai { get; set; }
            public string An { get; set; }
        }
    }
}
