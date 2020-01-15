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

        private List<int> columns;
        private List<int> rows;
        private List<List<int>> matrix;

        public MainViewModel()
        {
            columns = new List<int>();
            rows = new List<int>();
            matrix = new List<List<int>>();

            Item = new RelayCommand<object>((p) => { return true; }, (p) => { });

            TestCommand = new RelayCommand<ItemsControl>((p) => { return true; }, (p) =>
            {
                //GetMatrix(p);
                //MessageBox.Show("hi");
                UI.MainWindow.SetMatrix(p, new List<List<int>>() {
                    new List<int>() { 5, 3, 0, 4, 5, 6, 7, 8, 9 },
                    new List<int>() { 6, 0, 0, 4, 5, 6, 7, 8, 9 },
                    new List<int>() { 0, 9, 8, 4, 5, 6, 7, 8, 9 },
                    new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }});
            });

            LoadedWindowCommand = new RelayCommand<ItemsControl>((p) => { return true; }, (p) =>
            {
                #region Add textbox and event
                UI.MainWindow.DrawListTextBox(p);
                UI.MainWindow.AddEvent_TextBox(p);
                #endregion
                //List<List<int>> test = new List<List<int>>() {
                //new List<int>() { 5, 3, 0 },
                //new List<int>() { 6, 0, 0 },
                //new List<int>() { 0, 9, 8 }};
                //MessageBox.Show(UI.Algorithm.isAvaiMatrix(test, 3).ToString());
            });
        }
    }
}
