using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SudokuUtility;
using Level = SudokuUtility.Models.Level;
using Sudoku = SudokuUtility.Models.Sudoku;

namespace Sodoku.ViewModels
{
    class NewWindowViewModel : BaseViewModel
    {
        #region PrivateProperties
        private Level _selectedLevel;
        private object _selectedOption;
        private ObservableCollection<Level> _listLevel;
        private Boolean _isEnableBtnStart;
        private string _timerCountDown;
        private readonly Stopwatch stopwatch;
        private Task TaskRunTimer;
        #endregion

        #region Command
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand StartButtonCommand { get; set; }
        public ICommand CheckButtonCommand { get; set; }
        #endregion

        #region PublicProperties
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
        public object SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnableBtnStart
        {
            get => _isEnableBtnStart;
            set
            {
                _isEnableBtnStart = value;
                OnPropertyChanged();
            }
        }

        public string TimerCountDown
        {
            get => _timerCountDown;
            set
            {
                _timerCountDown = value;
                OnPropertyChanged();
            }
        }

        #endregion


        public NewWindowViewModel()
        {
            stopwatch = new Stopwatch();
            ListLevel = new ObservableCollection<Level>();
            ListLevel.Add(new Level() { Index = 0, Name = "Easy" });
            ListLevel.Add(new Level() { Index = 1, Name = "Medium" });
            ListLevel.Add(new Level() { Index = 2, Name = "Hard" });
            ListLevel.Add(new Level() { Index = 3, Name = "Expert" });

            StartButtonCommand = new RelayCommand<StackPanel>((p) => true, async (p) =>
            {
                if (SelectedLevel != null && SelectedOption != null)
                {
                    if (((ComboBoxItem)SelectedOption).Content.Equals("Using API"))
                    {
                        Api api = new Api();
                        Sudoku su = await api.GetJson(SelectedLevel);
                        if (su != null)
                        {
                            WindowSudoku.SetSudoku(su.GetQuestion, p);
                            if (stopwatch.IsRunning)
                            {
                                stopwatch.Reset();
                                TimerCountDown = string.Empty;
                                while (TaskRunTimer.Status == TaskStatus.Running)
                                {
                                    TaskRunTimer.Wait();
                                    TaskRunTimer.Dispose();
                                }
                                TaskRunTimer = null;
                            }
                            stopwatch.Start();
                        }
                    }
                    else
                    {
                        SudokuGenerate gen = new SudokuGenerate(SelectedLevel);
                        WindowSudoku.SetSudoku(gen.Question, p);
                        if (stopwatch.IsRunning)
                        {
                            stopwatch.Reset();
                            TimerCountDown = string.Empty;
                            while (TaskRunTimer.Status == TaskStatus.Running)
                            {
                                TaskRunTimer.Wait();
                                TaskRunTimer.Dispose();
                            }
                            TaskRunTimer = null;
                        }
                        stopwatch.Start();
                    }
                    StartTimer();
                }
            });

            LoadedWindowCommand = new RelayCommand<object[]>((p) => true, (p) =>
            {
                IsEnableBtnStart = true;
                BindingControl bc = new BindingControl()
                { GridSudoku = p[0] as StackPanel, GridButton = p[1] as ItemsControl };
                WindowSudoku.DrawButton(bc.GridButton);
                WindowSudoku.SetEvent(bc.GridSudoku);
            });

            CheckButtonCommand = new RelayCommand<StackPanel>((p) => true, async (p) =>
            {
                IsEnableBtnStart = false;
                int countEmpty = 0;
                Task task = Task.Run(() =>
                {
                    if (Application.Current.Dispatcher != null)
                        Application.Current.Dispatcher.Invoke((Action)(async () =>
                       {
                           foreach (StackPanel stackChild in p.Children)
                           {
                               foreach (UIElement tb in stackChild.Children)
                               {
                                   var textbox = tb as TextBox;
                                   if (string.IsNullOrEmpty(textbox.Text) && !textbox.IsReadOnly)
                                   {
                                       textbox.Background = Brushes.PaleVioletRed;
                                       countEmpty++;
                                       await Task.Delay(50);
                                   }
                               }
                           }
                           IsEnableBtnStart = true;
                           WindowSudoku.GridSudoku = p;
                           if (countEmpty.Equals(0))
                           {
                               stopwatch.Stop();
                               TimeSpan ts = stopwatch.Elapsed;
                               MessageBox.Show($"Hoàn thành trong {ts.Minutes.ToString()}:{ts.Seconds.ToString()}");
                               while (TaskRunTimer.Status == TaskStatus.Running)
                               {
                                   TaskRunTimer.Wait();
                                   TaskRunTimer.Dispose();
                               }
                               TaskRunTimer = null;
                           }
                       }));
                });
                await task;
            });
        }

        private List<List<int>> GetSudokuFromFile(string address)
        {
            List<List<int>> sudoku = new List<List<int>>();
            string stringFromFile = File.ReadAllText(address);
            if (string.IsNullOrEmpty(stringFromFile))
                return null;

            string[] rows = stringFromFile.Split('\n');
            foreach (string row in rows)
            {
                if (!string.IsNullOrEmpty(row))
                {
                    List<int> listLine = new List<int>();
                    string[] line = row.Split(' ');
                    foreach (string s in line)
                        if (!string.IsNullOrEmpty(s))
                            listLine.Add(Convert.ToInt32(s));
                    if (listLine.Count > 0)
                        sudoku.Add(listLine);
                }
            }

            return sudoku;
        }

        private void StartTimer()
        {
            if (TaskRunTimer == null)
            {
                TaskRunTimer = Task.Run(() =>
                {
                    while (stopwatch.IsRunning)
                    {
                        TimeSpan ts = stopwatch.Elapsed;
                        TimerCountDown = $"{ts.Minutes.ToString()}:{ts.Seconds.ToString()}";
                    }
                });
            }
        }
    }

    class BindingControl
    {
        public StackPanel GridSudoku { get; set; }
        public ItemsControl GridButton { get; set; }
    }

}
