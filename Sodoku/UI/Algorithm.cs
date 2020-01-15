namespace UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Algorithm
    {
        private List<List<int>> _matrix;
        public List<List<int>> Matrix { get => _matrix; set => _matrix = value; }
        private readonly List<int> Number
            = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        public Algorithm()
        {

        }

        public Algorithm(List<List<int>> Matrix)
        {
            this.Matrix = Matrix;
        }

        public void SolveSudoku()
        {
            if (Matrix.Equals(null)) return;
        }

        private bool isAvaiRow(List<int> row, int value)
        {
            return row.Contains(value);
        }

        private bool isAvaiColumn(List<int> column, int value)
        {
            return column.Contains(value);
        }

        public bool isAvaiMatrix(List<List<int>> matrix, int value)
        {
            return matrix.Any(i => i.Contains(value));
        }
    }
}
