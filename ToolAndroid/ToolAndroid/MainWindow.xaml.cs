using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ToolAndroid
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

        private void BtnOpenNox_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            tbPathNox.Text = openFileDialog.FileName;
        }

        List<string> GetLstDevices(string pathNox)
        {
            string command = @"devices";
            List<string> lstDevices = new List<string>();
            if (!string.IsNullOrEmpty(pathNox))
            {
                string temp = ExecuteCommand(pathNox + "nox_adb.exe", command);
                lstDevices = temp.Replace("List of devices attached", "").Trim().Split('\n').ToList();
                return lstDevices;
            }
            else return lstDevices;
        }

        string ExecuteCommand(string filename,string command)
        {
            string result = string.Empty;
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = filename;
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            return result;
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {

        }

        public class LstDevices
        {
            public string device { get; set; }
            public string ip { get; set; }
        }

        private void TbPathNox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPathNox.Text.Contains("Nox.exe"))
            {
                List<LstDevices> dv = new List<LstDevices>();
                if (string.IsNullOrEmpty(tbPathNox.Text)) return;
                else
                {
                    foreach (string devices in GetLstDevices(tbPathNox.Text.Replace("Nox.exe", "")))
                    {
                        string[] temp = devices.Split('\r', '\t');
                        LstDevices lstDevices = new LstDevices();
                        lstDevices.ip = temp[0];
                        lstDevices.device = temp[1];
                        dv.Add(lstDevices);
                    }
                }
                lvInfomation.ItemsSource = dv;
            }
        }
    }
}
