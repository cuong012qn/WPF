namespace SudokuUtility
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;
    using System;

    public class Window
    {
        public static bool SetSudoku(List<List<int>> inputSudoku, StackPanel targetPanel)
        {
            if (inputSudoku.Count > 9 || inputSudoku[0].Count > 9) return false;
            try
            {
                int i = 0;
                foreach (StackPanel grid in targetPanel.Children)
                {
                    int j = 0;
                    foreach (UIElement tb in grid.Children)
                    {
                        if (tb != null && tb is TextBox)
                            (tb as TextBox).Text = inputSudoku[i][j].ToString();
                        j++;
                    }
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public static List<List<int>> GetSudoku(StackPanel grids)
        {
            List<List<int>> sudoku = new List<List<int>>();
            if (grids != null)
            {
                foreach (StackPanel grid in grids.Children)
                {
                    List<int> row = new List<int>();
                    foreach (UIElement tb in grid.Children)
                    {
                        if (tb != null && tb is TextBox)
                            row.Add(Convert.ToInt32((tb as TextBox).Text));
                    }
                    sudoku.Add(row);
                }
            }
            return sudoku;
        }
    }
}
