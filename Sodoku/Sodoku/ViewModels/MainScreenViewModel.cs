using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Number = UI.Number;
using SudokuExtension.Models;
using SudokuExtension;
using UI;

namespace Sodoku.ViewModels
{
    class MainScreenViewModel : BaseViewModel
    {
        private string _time;
        private Level _selectedLevel;
        private List<List<Number>> MatrixSudoku = new List<List<Number>>();
        private ObservableCollection<Level> _listLevel;
        public ObservableCollection<Level> ListLevel
        {
            get => _listLevel;
            set
            {
                _listLevel = value;
                OnPropertyChanged();
            }
        }
        public Level SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                _selectedLevel = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadedWindowCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand LoadFromFileCommand { get; set; }
        public ICommand StopWatchCommand { get; set; }
        public ICommand IsSudokuCommand { get; set; }
        public ICommand StartOfflineCommand { get; set; }
        public ICommand StartOnlineCommand { get; set; }
        private readonly Stopwatch Watch;

        //public List<List<int>> Matrix { get => _matrix; set => _matrix = value; }
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
            ListLevel = new ObservableCollection<Level>
            {
                new Level() { Index = 0, Name = "Easy" },
                new Level() { Index = 1, Name = "Medium" },
                new Level() { Index = 2, Name = "Hard" },
                new Level() { Index = 3, Name = "Expert" }
            };
            Watch = new Stopwatch();
            //Matrix = new List<List<int>>();
            LoadedWindowCommand = new RelayCommand<StackPanel>((p) => true,
                (p) =>
                {
                    UI.Matrix.SetEventButton(p);
                });

            StopWatchCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                Watch.Stop();
                //MessageBox.Show(Watch.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            });

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var temp = UI.Matrix.MatrixSudoku;
                string s = string.Empty;
                foreach (var t in temp)
                {
                    foreach (var x in t)
                    {
                        s += x.Value + " ";
                    }

                    s += "\n";
                }

                MessageBox.Show(s);
            });

            LoadFromFileCommand = new RelayCommand<StackPanel>((p) => true, (p) =>
            {
                List<List<Number>> result = UI.Matrix.GetMatrixFromFile(Path.Combine(Directory.GetCurrentDirectory(), "InputMatrix.txt"));
                UI.Matrix.SetMatrix(p, result);
                Watch.Start();
                Task task = Task.Run(() =>
                {
                    while (Watch.IsRunning)
                    {
                        TimeSpan sp = Watch.Elapsed;
                        Time = $"{sp.Minutes.ToString():00}:{sp.Seconds.ToString():00}";
                    }
                });
            });

            IsSudokuCommand = new RelayCommand<object>((p) => true,
                (p) =>
                {
                    string text = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),
                        "Output.txt"));
                    var getList = GetMatrixFromFile(text);
                    MessageBox.Show(UI.Generate.IsSudoku(getList).ToString());

                });

            StartOfflineCommand = new RelayCommand<StackPanel>((p) => true, (p) =>
            {
                UI.Matrix.PanelSudoku = p;
                if (!Watch.IsRunning)
                {
                    Generate gen = new Generate(SelectedLevel);
                    this.MatrixSudoku = gen.SudokuQues;
                    UI.Matrix.SetMatrix(p, gen.SudokuQues);

                    Watch.Start();
                    Task task = Task.Run(() =>
                    {
                        while (Watch.IsRunning)
                        {
                            //var temp = UI.Matrix.GetMatrix(p);
                            //if (!MatrixSudoku.Equals(temp)) MatrixSudoku = temp;
                            TimeSpan sp = Watch.Elapsed;
                            Time = $"{sp.Minutes.ToString():00}:{sp.Seconds.ToString():00}";
                        }
                    });
                }

                //UI.Generate.FillMatrix();
                //UI.Generate.RemoveDigit(50);
                //List<List<Number>> matrix = UI.Generate.GetResult;
            });

            StartOnlineCommand = new RelayCommand<StackPanel>((p) => true, async (p) =>
            {
                if (!Watch.IsRunning)
                {
                    Api api = new Api();
                    Sudoku su = await api.GetJson(SelectedLevel);
                    List<List<Number>> matrix = GetMatrixFromString(su.GetQuestion());
                    UI.Matrix.SetMatrix(p, matrix);

                    //Start timing
                    Watch.Start();
                    Task task = Task.Run(() =>
                    {
                        while (Watch.IsRunning)
                        {
                            TimeSpan sp = Watch.Elapsed;
                            Time = $"{sp.Minutes.ToString():00}:{sp.Seconds.ToString():00}";
                        }
                    });
                }
            });
        }

        private List<List<Number>> GetMatrixFromFile(string text)
        {
            List<List<Number>> result = new List<List<Number>>();
            string[] rows = text.Split('\n');
            foreach (var row in rows)
            {
                string[] t = row.Split(' ');
                List<Number> list = new List<Number>();
                foreach (var t1 in t)
                {
                    if (!string.IsNullOrEmpty(t1))
                        list.Add(new Number() { CanEdit = true, Value = Convert.ToInt32(t1) });
                }

                if (list.Count > 0)
                    result.Add(list);
            }
            return result;
        }

        private List<List<Number>> GetMatrixFromString(string text)
        {
            int count = 0;
            List<List<Number>> result = new List<List<Number>>();
            List<Number> num = new List<Number>();
            for (int i = 0; i < text.Length; i++)
            {
                num.Add((text[i] - '0').Equals(0)
                    ? new Number() { CanEdit = true, Value = (text[i] - '0') }
                    : new Number() { CanEdit = false, Value = (text[i] - '0') });
                if (num.Count.Equals(9))
                {
                    var templist = new List<Number>(num);
                    templist.ForEach(x =>
                    {
                        if (x.Value.Equals(0)) count++;
                    });
                    result.Add(templist);
                    num.Clear();
                }
            }

            MessageBox.Show(count.ToString());
            return result;
        }
    }
}
