using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI
{
    public class Generate
    {
        private static List<List<Number>> _matrix = CreateMatrix();
        private static readonly Random Random = new Random();
        private static readonly object Synclock = new object();
        public static List<List<Number>> Matrix { get => _matrix; set => _matrix = value; }

        public Generate()
        {

        }

        /// <summary>
        /// Create Matrix 9x9 only 0
        /// </summary>
        /// <returns>Matrix 9x9 only 0</returns>
        private static List<List<Number>> CreateMatrix()
        {
            List<List<Number>> result = new List<List<Number>>();
            for (int i = 0; i < 9; i++)
            {
                List<Number> temp = new List<Number>();
                for (int j = 0; j < 9; j++)
                    temp.Add(new Number() { Value = 0, CanEdit = true });
                result.Add(temp);
            }
            return result;
        }

        public static void FillMatrix()
        {
            for (int row = 0; row < 9; row++)
            {
                List<int> lstNumber = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                for (int column = 0; column < 9; column++)
                {
                    List<int> lstFillCell = GetLstFillCell(lstNumber, row, column);
                    if (lstFillCell.Count.Equals(0))
                    {
                        int k = 0;
                        while (IsFill(row))
                        {
                            if (k.Equals(0))
                            {
                                for (int i = 8; i >= 0; i--)
                                {
                                    if (Matrix[row][i].Value.Equals(0))
                                        k = i;
                                }
                            }

                            for (int i = 8; i >= k; i--)
                            {
                                if (!Matrix[row][i].Value.Equals(0))
                                {
                                    lstNumber.Add(Matrix[row][i].Value);
                                    Matrix[row][i].Value = 0;
                                }
                            }

                            for (int i = k; i < 9; i++)
                            {
                                var temp = GetLstFillCell(lstNumber, row, i);
                                if (temp.Count > 0)
                                {
                                    int pos = GetRandomPos(0, temp.Count);
                                    Matrix[row][i].Value = temp[pos];
                                    lstNumber.Remove(Matrix[row][i].Value);
                                }
                            }
                            k--;
                        }
                    }
                    else
                    {
                        int pos = GetRandomPos(0, lstFillCell.Count);
                        Matrix[row][column].Value = lstFillCell[pos];
                        lstNumber.Remove(Matrix[row][column].Value);
                    }
                }
            }
        }

        /// <summary>
        /// Check in current row exists 0 in Matrix
        /// </summary>
        /// <param name="posRow">Input current row</param>
        /// <returns>True if has 0 in row, otherwise return false</returns>
        private static bool IsFill(int posRow)
        {
            return Matrix[posRow].Any(x => x.Value.Equals(0));
        }

        private static List<int> GetLstFillCell(List<int> inputList, int posRow, int posCol)
        {
            List<int> result = new List<int>(inputList);
            //Check value on row remove if exists
            for (int i = posRow; i >= 0; i--)
            {
                if (result.Contains(Matrix[i][posCol].Value))
                    result.Remove(Matrix[i][posCol].Value);
            }
            //Check value on box remove if exists
            var box = GetBox(posCol, posRow);
            for (int i = 0; i < box.Count; i++)
            {
                for (int j = 0; j < box.Count; j++)
                {
                    if (result.Contains(box[i][j].Value))
                        result.Remove(box[i][j].Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Get random number from min to max
        /// </summary>
        /// <param name="min">Input min value</param>
        /// <param name="max">Input max value</param>
        /// <returns>Random number</returns>
        private static int GetRandomPos(int min, int max)
        {
            lock (Synclock)
            {
                return Random.Next(min, max);
            }
        }

        /// <summary>
        /// Get matrix 3x3 from position column and position row
        /// </summary>
        /// <param name="posCol">Position column</param>
        /// <param name="posRow">Position row</param>
        /// <returns>Matrix 3x3</returns>
        public static List<List<Number>> GetBox(int posCol, int posRow)
        {
            var result = new List<List<Number>>();
            int col = -1, row = -1;
            for (int i = 2; i <= 8; i += 3)
            {
                col = posCol <= i && col.Equals(-1) ? (i + 1) / 3 : col;
                row = posRow <= i && row.Equals(-1) ? (i + 1) / 3 : row;
                if (!col.Equals(-1) && !row.Equals(-1)) break;
            }

            for (int i = (row * 3) - 3; i < (row * 3); i++)
            {
                List<Number> tempList = new List<Number>();
                for (int j = (col * 3) - 3; j < (col * 3); j++)
                {
                    tempList.Add(Matrix[i][j]);
                }
                result.Add(tempList);
            }
            return result;
        }

        public static List<List<Number>> FillBox()
        {
            List<int> number = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<List<Number>> result = new List<List<Number>>();
            for (int i = 0; i < 3; i++)
            {
                List<Number> temp = new List<Number>();
                for (int j = 0; j < 3; j++)
                {
                    int pos = GetRandomPos(0, number.Count);
                    temp.Add(new Number()
                    {
                        Value = number[pos],
                        CanEdit = true
                    });
                    number.RemoveAt(pos);
                }
                result.Add(temp);
            }
            return result;
        }
    }
}
