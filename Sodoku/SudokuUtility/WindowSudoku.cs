
using System.Reflection.Emit;
using System.Windows.Media.TextFormatting;

namespace SudokuUtility
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using System.Windows.Media;

    public class WindowSudoku : IDisposable
    {
        private static TextBox tmpTextBox = new TextBox();
        public static StackPanel GridSudoku { get; set; }

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
                        {
                            if (!inputSudoku[i][j].Equals(0))
                            {
                                //#FFB9B9B9
                                var textbox = (tb as TextBox);
                                textbox.Background = Brushes.DarkGray;
                                textbox.Text = inputSudoku[i][j].ToString();
                                textbox.IsReadOnly = true;
                            }
                            else
                            {
                                var textbox = tb as TextBox;
                                textbox.Text = string.Empty;
                                textbox.Background = Brushes.Transparent;
                                textbox.IsReadOnly = false;
                            }
                        }

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

        public static bool SetEvent(StackPanel Grid)
        {
            if (Grid.Children.Count != 9) return false;
            if (((StackPanel)Grid.Children[0]).Children.Count != 9) return false;

            try
            {
                foreach (StackPanel grid in Grid.Children)
                {
                    foreach (UIElement tb in grid.Children)
                    {
                        var textbox = (TextBox)tb;
                        textbox.Background = Brushes.Transparent;
                        textbox.GotFocus += TextBox_GotFocus;
                        textbox.LostFocus += TextBox_LostFocus;
                        textbox.PreviewTextInput += TextBox_PreviewTextInput;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #region Event
        private static void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            if (!tb.Background.Equals(Brushes.DarkGray))
                tb.Background = Brushes.Transparent;
            tb.Text = tmpTextBox.Text;
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            if (tb.Background.Equals(Brushes.Transparent))
                tb.Background = Brushes.DeepSkyBlue;
            else if (tb.Background.Equals(Brushes.PaleVioletRed))
            {
                foreach (StackPanel gridSudokuChild in GridSudoku.Children)
                {
                    foreach (UIElement txb in gridSudokuChild.Children)
                    {
                        var textbox = (TextBox)txb;
                        if (textbox.Background.Equals(Brushes.PaleVioletRed))
                            textbox.Background = Brushes.Transparent;
                    }
                }

                tb.Background = Brushes.DeepSkyBlue;
            }
            tmpTextBox = tb;
        }

        public static void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tmpTextBox.Background != null)
                if (tmpTextBox.Background.Equals(Brushes.Transparent))
                    tmpTextBox.Text = ((Button)sender).Content.ToString();
        }

        #endregion

        public static void DrawButton(ItemsControl ic)
        {
            int count = 1;
            for (int i = 0; i < 3; i++)
            {
                StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
                for (int j = 0; j < 3; j++)
                {
                    var button = new Button
                    {
                        Content = count.ToString()
                    };
                    button.Click += Button_Click;
                    sp.Children.Add(button);
                    count++;
                }
                ic.Items.Add(sp);
            }
        }

        private static bool IsFullvalue()
        {
            if (GridSudoku != null)
            {
                foreach (StackPanel gridSudokuChild in GridSudoku.Children)
                {
                    foreach (UIElement textbox in gridSudokuChild.Children)
                    {
                        if (textbox != null)
                        {
                            var tb = textbox as TextBox;
                            if (string.IsNullOrEmpty(tb.Text))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else return false;

            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
