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

namespace Proxy_selenium_WPF
{
    /// <summary>
    /// Interaction logic for UCToolBar.xaml
    /// </summary>
    public partial class UCToolBar : UserControl
    {
        public UCToolBar()
        {
            InitializeComponent();
        }

        private Window GetWindow()
        {
            return this.GetWindow();
        }

        FrameworkElement GetwindowParent(UserControl p)
        {
            FrameworkElement parent = p;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = GetwindowParent(this);
            var temp = element as Window;
            temp.Close();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            //var window = GetWindow();
            //if (window.WindowState == WindowState.Maximized) window.WindowState = WindowState.Minimized;
            //else window.WindowState = WindowState.Maximized;
            FrameworkElement element = GetwindowParent(this);
            var temp = element as Window;
            if (temp.WindowState == WindowState.Maximized) temp.WindowState = WindowState.Normal;
            else temp.WindowState = WindowState.Maximized;
        }

        private void BtnMinize_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = GetwindowParent(this);
            var temp = element as Window;
            temp.WindowState = WindowState.Minimized;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement element = GetwindowParent(this);
            var temp = element as Window;
            temp.DragMove();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = GetwindowParent(this);
            var temp = element as Window;
            temp.DragMove();
        }

        private void BtnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClose.Background = Brushes.Red;
        }

        private void BtnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnClose.Background = (Brush)bc.ConvertFrom("#FF512DA8");
        }
    }
}
