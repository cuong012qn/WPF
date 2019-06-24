using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListView_WPF
{
    public class Question
    {
        public enum Result { A, B, C, D }

        public string question { get; set; }
        public string answerA { get; set; }
        public string answerB { get; set; }
        public string answerC { get; set; }
        public string answerD { get; set; }
        public Result result { get; set; }
        public bool isChecked { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }

        public string Exportquestions()
        {
            return question + "\nA. " + answerA + "\nB. " + answerB + "\nC. " + answerC + "\nD. " + answerD + "\nKey "
    + result + "\n";
        }
    }
}
