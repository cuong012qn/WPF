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
            LoadedWindowCommand = new RelayCommand<StackPanel>((p) => { return true; },
                (p) =>
                {
                    UI.Matrix.SetEventButton(p);
                });

            StopWatchCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Watch.Stop();
                MessageBox.Show(Watch.Elapsed.TotalSeconds.ToString());
            });

            LoadCommand = new RelayCommand<StackPanel>((p) => { return true; }, (p) =>
            {
                //UI.Matrix.GetMatrix(p as StackPanel);
                var mat = UI.Matrix.GetMatrixFromFile(Path.Combine(Directory.GetCurrentDirectory(), "InputMatrix.txt"));
                UI.Generate.Matrix = mat;
                var s = UI.Generate.GetBox(3, 3);
                string r = string.Empty;
                foreach (List<Number> row in s)
                {
                    foreach (Number column in row)
                    {
                        r += column.Value + " ";
                    }
                    r += "\n";
                }
                MessageBox.Show(r);
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
