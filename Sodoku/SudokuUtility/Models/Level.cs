using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuUtility.Models
{
    public class Level
    {
        private static int _count = 0;
        private string _name;
        private readonly Random random = new Random();
        private readonly object SyncLock = new object();

        public int Index { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (!string.IsNullOrEmpty(Name))
                {
                    Index = _count;
                    _count++;
                }
            }
        }

        public int Difficult(LevelConstants lvConstants = 0)
        {
            //Easy 40 - 47
            //Medium 47 - 50
            //Hard 53 - 55
            //Expert 57 - 59
            if (!string.IsNullOrEmpty(Name))
            {
                LevelConstants lv;
                Enum.TryParse(Name, out lv);
                switch (lv)
                {
                    case LevelConstants.Easy:
                        return RandomNumber(40, 47);
                    case LevelConstants.Medium:
                        return RandomNumber(47, 50);
                    case LevelConstants.Hard:
                        return RandomNumber(53, 55);
                    case LevelConstants.Expert:
                        return RandomNumber(57, 59);
                }
            }
            else if (lvConstants != 0)
            {
                switch (lvConstants)
                {
                    case LevelConstants.Easy:
                        return RandomNumber(40, 47);
                    case LevelConstants.Medium:
                        return RandomNumber(47, 50);
                    case LevelConstants.Hard:
                        return RandomNumber(53, 55);
                    case LevelConstants.Expert:
                        return RandomNumber(57, 59);
                }
            }
            return 0;
        }

        private int RandomNumber(int start, int end)
        {
            lock (SyncLock)
            {
                return random.Next(start, end);
            }
        }

        public override string ToString()
        {
            return $"Index {Index}\nName {Name}";
        }

        public enum LevelConstants
        {
            None = 0,
            Easy,
            Medium,
            Hard,
            Expert
        }
    }
}
