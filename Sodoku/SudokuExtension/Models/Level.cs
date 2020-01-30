using System.IO.Pipes;

namespace SudokuExtension.Models
{
    public class Level
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return string.Format("Index {0}\nName {1}", Index, Name);
        }
    }
}
