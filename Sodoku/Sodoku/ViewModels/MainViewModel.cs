using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI;

namespace Sodoku.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public ICommand Item { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand TestCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand FindCommand { get; set; }
        private List<List<int>> matrix;

        private int _column = -1;
        private int _row = -1;

        public int Column
        {
            get => _column;
            set
            {
                _column = value;
                OnPropertyChanged();
            }
        }
        public int Row
        {
            get => _row;
            set
            {
                _row = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {

            Item = new RelayCommand<object>((p) => { return true; }, (p) => { });

            LoadCommand = new RelayCommand<ItemsControl>((p) => { return true; }, (p) =>
            {
                matrix = new List<List<int>>() {
                new List<int>() { 5, 3, 0, 0, 7, 0, 0, 0, 0 }, //0
                new List<int>() { 6, 0, 0, 1, 9, 5, 0, 0, 0 }, //1
                new List<int>() { 0, 9, 8, 0, 0, 0, 0, 4, 0 }, //2
                new List<int>() { 8, 0, 0, 4, 5, 6, 7, 8, 9 }, //3
                new List<int>() { 4, 0, 0, 4, 4, 6, 7, 6, 9 }, //4
                new List<int>() { 7, 0, 0, 4, 5, 6, 7, 5, 9 }, //5
                new List<int>() { 0, 6, 0, 4, 3, 6, 7, 8, 9 }, //6
                new List<int>() { 0, 0, 0, 4, 1, 6, 7, 1, 9 }, //7
                new List<int>() { 0, 0, 0, 4, 5, 6, 7, 8, 9 } }; //8
                                                                 //0, 1, 2, 3, 4, 5, 6, 7 ,8                                  
                UI.MainWindow.SetMatrix(p, matrix);
            });

            FindCommand = new RelayCommand<object>((p) =>
            {
                if (Row < 0 || Column < 0) return false;
                return true;
            }, (p) =>
            {
                string result = string.Empty;
                List<List<int>> vs = UI.Algorithm.GetBoxMatrix(matrix, Column, Row);
                foreach (List<int> t in vs)
                {
                    t.ForEach(x => result += x.ToString() + " ");
                    result += "\n";
                }
                MessageBox.Show(string.Format("[{0}][{1}] = {2}\n{3}",
                    Row, Column,
                    matrix[Row][Column].ToString(), result));
            });

            LoadedWindowCommand = new RelayCommand<ItemsControl>((p) => { return true; }, (p) =>
            {
                #region Add textbox and event
                UI.MainWindow.DrawListTextBox(p);
                UI.MainWindow.AddEvent_TextBox(p);
                #endregion
            });
        }
    }
}
