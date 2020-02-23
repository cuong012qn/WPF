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
using MaterialDesignThemes.Wpf;
using Sodoku.Views;
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
        private bool _isEnableBtnStart;
        private bool _isEnableBtnResume;
        private string _timerCountDown;
        private readonly Stopwatch _stopwatch;
        private Task TaskRunTimer;
        private List<List<int>> _SudokuResult = new List<List<int>>();
        #endregion

        #region Command
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand StartButtonCommand { get; set; }
        public ICommand CheckButtonCommand { get; set; }
        public ICommand PauseButtonCommand { get; set; }
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

        public bool IsEnableBtnResume
        {
            get => _isEnableBtnResume;
            set
            {
                _isEnableBtnResume = value;
                OnPropertyChanged();
            }
        }

        #endregion


        public NewWindowViewModel()
        {
            _stopwatch = new Stopwatch();
            ListLevel = new ObservableCollection<Level>();
            ListLevel.Add(new Level() { Index = 0, Name = "Easy" });
            ListLevel.Add(new Level() { Index = 1, Name = "Medium" });
            ListLevel.Add(new Level() { Index = 2, Name = "Hard" });
            ListLevel.Add(new Level() { Index = 3, Name = "Expert" });

            StartButtonCommand = new RelayCommand<StackPanel>((p) => true, async (p) =>
            {
                if (SelectedLevel != null && SelectedOption != null)
                {
                    DialogHost.CloseDialogCommand.Execute(null,p);
                    if (((ComboBoxItem)SelectedOption).Content.Equals("Using API"))
                    {
                        if (_SudokuResult.Count != 0) _SudokuResult.Clear();
                        Api api = new Api();
                        Sudoku su = await api.GetJson(SelectedLevel);
                        if (su != null)
                        {
                            _SudokuResult = su.GetAnswer;
                            WindowSudoku.SetSudoku(su.GetQuestion, p);
                            if (_stopwatch.IsRunning)
                            {
                                _stopwatch.Reset();
                                TimerCountDown = string.Empty;
                                while (TaskRunTimer.Status == TaskStatus.Running)
                                {
                                    TaskRunTimer.Wait();
                                    TaskRunTimer.Dispose();
                                }
                                TaskRunTimer = null;
                            }
                            _stopwatch.Start();
                        }
                    }
                    else
                    {
                        if (_SudokuResult.Count != 0) _SudokuResult.Clear();
                        SudokuGenerate gen = new SudokuGenerate(SelectedLevel);
                        WindowSudoku.SetSudoku(gen.Question, p);
                        _SudokuResult = gen.Answer;
                        if (_stopwatch.IsRunning)
                        {
                            _stopwatch.Reset();
                            TimerCountDown = string.Empty;
                            while (TaskRunTimer.Status == TaskStatus.Running)
                            {
                                TaskRunTimer.Wait();
                                TaskRunTimer.Dispose();
                            }
                            TaskRunTimer = null;
                        }
                        _stopwatch.Start();
                    }
                    StartTimer();
                    IsEnableBtnResume = true;
                }
            });

            LoadedWindowCommand = new RelayCommand<object[]>((p) => true, (p) =>
            {
                Task task = Task.Run(() =>
                {
                    if (Application.Current.Dispatcher != null)
                        Application.Current.Dispatcher.Invoke((Action)(async () =>
                        {
                            UCLoadingView loading = new UCLoadingView();
                            await DialogHost.Show(loading, "Sudoku");
                        }));
                });
                IsEnableBtnStart = true;
                IsEnableBtnResume = false;
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
                           #region oldMethod
                           //foreach (StackPanel stackChild in p.Children)
                           //{
                           //    foreach (UIElement tb in stackChild.Children)
                           //    {
                           //        var textbox = tb as TextBox;
                           //        if (string.IsNullOrEmpty(textbox.Text) && !textbox.IsReadOnly)
                           //        {
                           //            textbox.Background = Brushes.PaleVioletRed;
                           //            countEmpty++;
                           //            await Task.Delay(50);
                           //        }
                           //    }
                           //}
                           #endregion

                           Task<bool> taskRun = IsValidSudoku(p, _SudokuResult);
                           bool result = await taskRun;

                           IsEnableBtnStart = true;
                           WindowSudoku.GridSudoku = p;
                           if (result)
                           {
                               if (countEmpty.Equals(0))
                               {
                                   _stopwatch.Stop();
                                   TimeSpan ts = _stopwatch.Elapsed;
                                   MessageBox.Show($"Hoàn thành trong {ts.Minutes.ToString()}:{ts.Seconds.ToString()}");
                                   while (TaskRunTimer.Status == TaskStatus.Running)
                                   {
                                       TaskRunTimer.Wait();
                                       TaskRunTimer.Dispose();
                                   }

                                   TaskRunTimer = null;
                               }
                           }
                       }));
                });
                await task;
            });

            PauseButtonCommand = new RelayCommand<Button>(p => true, (p) =>
            {
                //var child = (StackPanel)p.Content;
                //((MaterialDesignThemes.Wpf.PackIcon)child.Children[0]).Kind = PackIconKind.Play;
                if (_stopwatch.IsRunning)
                {
                    var child = (StackPanel)p.Content;
                    ((MaterialDesignThemes.Wpf.PackIcon)child.Children[0]).Kind = PackIconKind.Play;
                    ((TextBlock)child.Children[1]).Text = "Resume";
                    _stopwatch.Stop();
                    while (TaskRunTimer.Status == TaskStatus.Running)
                    {
                        TaskRunTimer.Wait();
                        TaskRunTimer.Dispose();
                    }
                    TaskRunTimer = null;
                }
                else if (!string.IsNullOrEmpty(TimerCountDown))
                {
                    var child = (StackPanel)p.Content;
                    ((MaterialDesignThemes.Wpf.PackIcon)child.Children[0]).Kind = PackIconKind.Pause;
                    ((TextBlock)child.Children[1]).Text = "Pause";
                    _stopwatch.Start();
                    StartTimer();
                }
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

        private async Task<bool> IsValidSudoku(StackPanel sp, List<List<int>> sudokuResult)
        {
            if (sp.Children.Count != 9) return false;
            if (((StackPanel)sp.Children[0]).Children.Count != 9) return false;

            int countEmptyCell = 0, countWrongCell = 0;
            int posRow = 0, posCol = 0;
            foreach (StackPanel spchil in sp.Children)
            {
                foreach (UIElement txb in spchil.Children)
                {
                    var textbox = (TextBox)txb;
                    if (!textbox.Background.Equals(Brushes.DarkGray) &&
                        !textbox.IsReadOnly)
                    {
                        if (string.IsNullOrEmpty(textbox.Text))
                        {
                            textbox.Background = Brushes.PaleVioletRed;
                            countEmptyCell++;
                            await Task.Delay(20);
                        }
                        else if (!Convert.ToInt32(textbox.Text).Equals(sudokuResult[posRow][posCol]))
                        {
                            textbox.Background = Brushes.PaleVioletRed;
                            countWrongCell++;
                            await Task.Delay(20);
                        }
                    }
                    posCol++;
                }
                posCol = 0;
                posRow++;
            }

            return (countEmptyCell.Equals(0) && countWrongCell.Equals(0));
        }

        private void StartTimer()
        {
            if (TaskRunTimer == null)
            {
                TaskRunTimer = Task.Run(() =>
                {
                    while (_stopwatch.IsRunning)
                    {
                        TimeSpan ts = _stopwatch.Elapsed;
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
