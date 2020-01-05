using System.Data.SqlClient;
using System.Data;

namespace QuanLyKho
{
    public class DataProvider
    {
        private readonly string ConnStr = "Data Source=DESKTOP-7UOK1F3;Initial Catalog=QuanLyKho;Integrated Security=True";
        private DataProvider()
        {

        }
        private static DataProvider _instance;

        public static DataProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataProvider();
                return _instance;
            }
            private set => _instance = value;
        }

        public DataTable Login(string username, string password)
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(ConnStr);
            connection.Open();
            try
            {
                //EXEC dbo.Proc_Login @username = N'admin',
                SqlCommand command = new SqlCommand("dbo.Proc_Login", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@username", SqlDbType.NChar).Value = username;
                command.Parameters.Add("@password", SqlDbType.NChar).Value = password;
                command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
                return dataTable;
            }
            catch
            {
                return dataTable;
            }
        }
    }
}
