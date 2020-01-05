using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy_selenium_WPF
{
    public class DataProvider
    {
        private readonly string ConnectionString = "Data Source=DESKTOP-7UOK1F3;Initial Catalog=Captcha;Integrated Security=True";
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

        private DataProvider()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="k_value">MD5</param>
        /// <returns></returns>
        public int InsertData(string value, string k_value)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("PROC_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@value", value);
                        command.Parameters.AddWithValue("@v_key", k_value);
                        return command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    return -1;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">MD5</param>
        /// <returns>Trả về value, nếu không trả về null</returns>
        public string FindValueCaptcha(string input)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("PROC_FindValue", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@value_key", input);
                        command.Parameters.Add("@value", SqlDbType.NChar, 4).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        return command.Parameters["@value"].Value.ToString();
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public DataTable ExecuteTable()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM dbo.Key_value";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dt);
                }
            }
            return dt;
        }

    }
}
