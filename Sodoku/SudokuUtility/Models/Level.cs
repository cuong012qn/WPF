using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuUtility.Models
{
    public class Level
    {
        public int Index { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Index {Index}\nName {Name}";
        }

        public enum LevelConstants
        {
            Easy,
            Medium,
            Hard,
            Expert
        }
    }
}
