using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    public class Generate
    {
        private static List<List<Number>> _matrix = CreateMatrix();

        public static List<List<Number>> Matrix { get => _matrix; set => _matrix = value; }

        public Generate()
        {

        }

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

        public static List<List<Number>> FillMatrix()
        {
            var merge = new UI.Matrix();
            List<List<Number>> result = FillBox();
            for (int i = 0; i < 9; i++)
            {
                HashSet<int> hash = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                for (int j = 0; j < 9; j++)
                {
                    Random r = new Random();
                    int pos = r.Next(0, hash.Count - 1);
                }

            }
            return result;
        }

        private static bool isExistsColum(int posCol, Number number)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Matrix[i][posCol].Equals(number))
                    return true;
            }
            return false;
        }

        private static bool isExistsRow(int posRow, Number number)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Matrix[posRow][i].Equals(number))
                    return true;
            }
            return false;
        }

        private static bool isExistsBox(int posRow, int posCol, Number number)
        {
            return Matrix.Any(x => x.Contains(number));
        }

        public static List<List<Number>> GetBox(int posRow, int posCol)
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
                    Random r = new Random();
                    int pos = r.Next(0, number.Count - 1);
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
