using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListView_WPF
{
    public class DataProvider
    {
        #region Singleton Designpattern
        private static DataProvider _instance;

        public static DataProvider Instance
        {
            get
            {
                if (_instance == null) _instance = new DataProvider();
                return _instance;
            }
            private set => _instance = value;
        }
        private DataProvider()
        {

        }
        #endregion

        private string connString = "Data Source=DESKTOP-2GPP93E;Initial Catalog=Test_Question;Integrated Security=True";

        private DataTable GetDataTableListQuestion()
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT * FROM dbo.Question";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        public List<Question> GetLstQuestion()
        {
            List<Question> lstQuestions = new List<Question>();
            DataTable dataTable = GetDataTableListQuestion();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Question question = new Question();
                question.question = dataTable.Rows[i]["question"].ToString();
                question.answerA = dataTable.Rows[i]["answerA"].ToString();
                question.answerB = dataTable.Rows[i]["answerB"].ToString();
                question.answerC = dataTable.Rows[i]["answerC"].ToString();
                question.answerD = dataTable.Rows[i]["answerD"].ToString();
                string result = dataTable.Rows[i]["result"].ToString();
                if (result == "A") question.result = Question.Result.A;
                if (result == "B") question.result = Question.Result.B;
                if (result == "C") question.result = Question.Result.C;
                if (result == "D") question.result = Question.Result.D;
                question.isChecked = false;
                lstQuestions.Add(question);
            }
            return lstQuestions;
        }
    }
}
