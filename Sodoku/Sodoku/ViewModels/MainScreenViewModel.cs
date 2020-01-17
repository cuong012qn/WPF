using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace Sodoku.ViewModels
{
    class MainScreenViewModel : BaseViewModel
    {
        private List<List<int>> _matrix;

        public ICommand LoadedWindowCommand { get; set; }
        public ICommand LoadCommand { get; set; }

        public List<List<int>> Matrix { get => _matrix; set => _matrix = value; }

        public MainScreenViewModel()
        {
            Matrix = new List<List<int>>();
            LoadedWindowCommand = new RelayCommand<StackPanel>((p) => { return true; },
                (p) =>
                {
                    #region Test
                    //List<List<int>> TestCase = new List<List<int>>()
                    //{
                    //    new List<int>() { 1, 2, 3 },
                    //    new List<int>() { 4, 5, 6 },
                    //    new List<int>() { 7, 8, 9 }
                    //};
                    //List<List<int>> insertMatrix = new List<List<int>>()
                    //{
                    //    new List<int>() { 4, 5, 6 },
                    //    new List<int>() { 7, 8, 9 },
                    //    new List<int>() { 1, 2, 3 }
                    //};

                    //List<List<int>> vs = MergeMatrix(TestCase, insertMatrix, 1);
                    //List<List<int>> v = MergeMatrix(vs, TestCase, 1);
                    //string s = string.Empty;
                    //foreach (List<int> item in v)
                    //{
                    //    item.ForEach(x => s += x.ToString() + " ");
                    //    s += "\n";
                    //}
                    //MessageBox.Show(s);
                    #endregion

                    List<List<int>> matrix = new List<List<int>>() {
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
                    SetMatrix(p, matrix);
                    string temp = string.Empty;
                    List<List<int>> getresult = GetMatrixFromFile(Path.Combine(Directory.GetCurrentDirectory(), "InputMatrix.txt"));
                    foreach(List<int> r in getresult)
                    {

                    }
                });

            LoadCommand = new RelayCommand<StackPanel>((p) => { return true; }, (p) =>
            {
                foreach (StackPanel block in p.Children)
                {
                    GetMatrix(block);
                }
            });
        }

        private List<List<int>> GetMatrixFromFile(string address)
        {
            List<List<int>> result = new List<List<int>>();
            try
            {
                string[] inputFromFile = File.ReadAllText(address).Split('\n');
                foreach (string s in inputFromFile)
                {
                    List<int> Lines = new List<int>();
                    string[] vs = s.Split(' ');
                    foreach (string v in vs) { Lines.Add(Convert.ToInt32(v)); }
                    result.Add(Lines);
                }
            }
            catch
            {
                return null;
            }
            return result;
        }

        private List<List<int>> MergeMatrix(List<List<int>> Matrix, List<List<int>> inputMatrix, int BlockCol)
        {
            List<List<int>> result = Matrix;
            int row = 0;
            for (int i = (BlockCol * 3) - 3; i < (BlockCol * 3); i++)
            {
                for (int j = 0; j < 3; j++)
                    Matrix[i].Add(inputMatrix[row][j]);
                row++;
            }

            return result;
        }

        private bool SetMatrix(StackPanel sp, List<List<int>> inputMatrix)
        {
            int ColBlock = 1, RowBlock = 1;
            foreach (StackPanel mainStack in sp.Children)
            {
                foreach (Border bd in mainStack.Children)
                {
                    int posRow = (RowBlock * 3) - 3; //0
                    int posCol = (ColBlock * 3) - 3; //0
                    foreach (StackPanel stackChild in (bd.Child as StackPanel).Children)
                    {
                        foreach (UIElement item in stackChild.Children)
                        {
                            if (item is TextBox)
                            {
                                (item as TextBox).Text =
                                    inputMatrix[posRow][posCol].ToString();
                            }
                            posCol++;
                        }
                        posRow++;
                        posCol = (ColBlock * 3) - 3;
                    }
                    ColBlock++;
                }
                ColBlock = 1; RowBlock++;
            }
            return true;
        }

        private List<List<int>> GetMatrix(StackPanel sp)
        {
            List<List<int>> result = new List<List<int>>();
            foreach (Border bd in sp.Children)
            {
                List<int> tempList = new List<int>();
                List<List<int>> r = GetMiniMatrix(bd.Child as StackPanel);
                foreach (List<int> item in r)
                {
                    item.ForEach(x => tempList.Add(x));
                }
                result.Add(tempList);
            }
            return result;
        }

        private List<List<int>> GetMiniMatrix(StackPanel sp)
        {
            List<List<int>> result = new List<List<int>>();
            foreach (StackPanel item in sp.Children)
            {
                List<int> temp = new List<int>();
                foreach (UIElement UI in item.Children)
                {
                    if (UI is TextBox) temp.Add(Convert.ToInt32((UI as TextBox).Text));
                }
                result.Add(temp);
            }
            return result;
        }
    }
}
