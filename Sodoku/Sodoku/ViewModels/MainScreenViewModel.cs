using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;
using Number = UI.Number;

namespace Sodoku.ViewModels
{
    class MainScreenViewModel : BaseViewModel
    {
        private List<List<int>> _matrix;
        private string _time;

        public ICommand LoadedWindowCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand LoadFromFileCommand { get; set; }
        public ICommand StopWatchCommand { get; set; }
        private Stopwatch Watch;

        public List<List<int>> Matrix { get => _matrix; set => _matrix = value; }
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        public MainScreenViewModel()
        {
            Watch = new Stopwatch();
            Matrix = new List<List<int>>();
            LoadedWindowCommand = new RelayCommand<StackPanel>((p) => true,
                (p) =>
                {
                    //var list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    //UI.Matrix.SetEventButton(p);
                    //var t = new List<List<int>>();
                    //t.Add(new List<int>() { 2, 3, 4, 5, 1, 6, 7, 8, 9 });
                    //t.Add(new List<int>() { 5, 1, 6, 7, 8, 9, 0, 0, 0 });
                    //t.Add(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                    //var s = UI.Algorithm.GetBoxMatrix(t, 6, 1);
                    //for (int k = 0; k < 3; k++)
                    //{
                    //    if (!t[k][6].Equals(0))
                    //    {
                    //        if (list.Contains(t[k][6]))
                    //            list.Remove(t[k][6]);
                    //    }
                    //}
                    //t[1].ForEach(x =>
                    //{
                    //    if (list.Contains(x))
                    //        list.Remove(x);
                    //});
                    //string result = string.Empty;
                    //foreach (List<int> vs in s)
                    //{
                    //    vs.ForEach(x => result += x.ToString() + " ");
                    //    result += "\n";
                    //}
                    //string a = string.Empty;
                    //list.ForEach(x => a += x.ToString() + " ");
                    //MessageBox.Show(a);
                    //MessageBox.Show(result);
                });

            StopWatchCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                Watch.Stop();
                MessageBox.Show(Watch.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            });

            LoadCommand = new RelayCommand<StackPanel>((p) => true, async (p) =>
            {
                //UI.Matrix.GetMatrix(p as StackPanel);
                //var mat = UI.Matrix.GetMatrixFromFile(Path.Combine(Directory.GetCurrentDirectory(), "InputMatrix.txt"));
                //UI.Generate.Matrix = mat;
                //var s = UI.Generate.GetBox(3, 3);
                Task task = Task.Run(() =>
                {
                    UI.Generate.FillMatrix();
                    var matrix = UI.Generate.Matrix;
                    string r = string.Empty;
                    foreach (List<Number> row in matrix)
                    {
                        foreach (Number column in row)
                        {
                            r += column.Value + " ";
                        }
                        r += "\n";
                    }
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "Output.txt"), r);
                    MessageBox.Show(r);
                });
                await task;
            });

            LoadFromFileCommand = new RelayCommand<StackPanel>((p) => { return true; }, (p) =>
            {
                List<List<Number>> getresult = UI.Matrix.GetMatrixFromFile(Path.Combine(Directory.GetCurrentDirectory(), "InputMatrix.txt"));
                UI.Matrix.SetMatrix(p, getresult);
                Watch.Start();
                Task task = Task.Run(() =>
                {
                    while (Watch.IsRunning)
                    {
                        TimeSpan sp = Watch.Elapsed;
                        Time = string.Format("{0:00}:{1:00}", sp.Minutes.ToString(), sp.Seconds.ToString());
                    }
                });
            });
        }
    }
}
