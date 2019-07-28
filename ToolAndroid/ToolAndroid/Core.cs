using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace ToolAndroid
{
    class Core
    {
        static string fileName = "path.txt";
        static string workPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        public static string ReadpathFromFile()
        {
            return File.ReadAllText(workPath);
        }

        public static bool WritepathToFile(string inputvalue)
        {
            if (File.Exists(workPath))
            {
                File.WriteAllText(workPath, inputvalue, Encoding.UTF8);
                return true;
            }
            return false;
        }

        public static void FindImage()
        {
            Image<Bgr, byte> source = new Image<Bgr, byte>(@"C:\Users\cuong\Desktop\ImageSearch\image_0.png");
            Image<Bgr, byte> template = new Image<Bgr, byte>(@"C:\Users\cuong\Desktop\ImageSearch\image_1.png");
            Image<Bgr, byte> imageToShow = source.Copy();
            //Emgu.CV.CvEnum.TM_TYPE.CV_TM_CCOEFF_NORMED
            using (Image<Gray, float> result = source.MatchTemplate(template,TemplateMatchingType.CcoeffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                if (maxValues[0] > 0.9)
                {
                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    imageToShow.Draw(match, new Bgr(Color.Red), 3);
                }
            }
            imageToShow.Save(@"C:\Users\cuong\Desktop\ImageSearch\result.png");
        }

        public static void ExecuteCommands(string workingPath, List<string> commands)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(workingPath + "nox_adb.exe");
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            foreach (string command in commands)
            {
                processStartInfo.Arguments = command;
                Process.Start(processStartInfo).WaitForExit();
            }
        }

        public static void ScreenCap(string workingPath, Devices devices, string nametoSave = null)
        {
            if (string.IsNullOrEmpty(workingPath))
                return;
            List<string> commands = new List<string>();
            string command1 = "-s {0} shell screencap /mnt/shell/emulated/0/Images/image.png";
            string command2 = "pull /mnt/shell/emulated/0/Images/image.png {0} ";
            string command3 = "-s {0} shell rm -f /mnt/shell/emulated/0/Images/image.png";
            commands.Add(String.Format(command1, devices.ip));
            commands.Add(String.Format(command2, Path.Combine(Directory.GetCurrentDirectory(), "Capture", nametoSave + ".png")));
            commands.Add(String.Format(command3, devices.ip));
            ExecuteCommands(workingPath, commands);
        }

        public static List<Devices> GetDevices(string workingPath)
        {
            List<Devices> devices = new List<Devices>();
            List<string> splitDevice = ExecuteCommand(workingPath, "devices")
                .Replace("List of devices attached", "").Trim().Split('\n').ToList();
            if (splitDevice != null)
            {
                foreach (string device in splitDevice)
                {
                    string[] dv = device.Split('\r', '\t');
                    Devices devices1 = new Devices();
                    devices1.device = dv[1];
                    devices1.ip = dv[0];
                    devices1.isChecked = false;
                    devices.Add(devices1);
                }
            }
            return devices;
        }

        public static bool CleanPicture()
        {
            List<string> lstPicture = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Capture"), "*.png",SearchOption.AllDirectories).ToList();
            try
            {
                foreach (string picture in lstPicture)
                {
                    File.Delete(picture);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ExecuteCommand(string workingPath, string command)
        {
            string result = string.Empty;
            using (Process process = new Process())
            {
                process.StartInfo.FileName = workingPath + "nox_adb.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.Arguments = command;
                process.StartInfo.CreateNoWindow = false;
                try
                {
                    process.Start();
                    result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return result;
        }
    }
}
