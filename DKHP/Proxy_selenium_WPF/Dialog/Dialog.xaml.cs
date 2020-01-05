using System.Windows;

namespace Proxy_selenium_WPF
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        private bool accept;
        private bool cancel;
        public bool Accept { get => accept; private set => accept = value; }
        public bool Cancel { get => cancel; private set => cancel = value; }

        //public Dialog()
        //{
        //    InitializeComponent();
        //}

        private Dialog()
        {

        }

        public Dialog(string Message)
        {
            InitializeComponent();
            rtbMessage.AppendText(Message);
        }

        public Result GetResult()
        {
            if (accept) return Result.Accept;
            else return Result.Cancel;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.accept = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.cancel = true;
            this.Close();
        }
    }
}
