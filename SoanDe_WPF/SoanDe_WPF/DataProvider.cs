using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoanDe_WPF
{
    public class DataProvider
    {
        private static DataProvider _instance;

        private DataProvider()
        {

        }

        private string ConnStr = @"Data Source=DESKTOP-7UOK1F3;Initial Catalog=ThuVienCauHoi;Integrated Security=True";

        public static DataProvider Instance
        {
            get
            {
                if (_instance == null) _instance = new DataProvider();
                return _instance;
            }
            private set => _instance = value;
        }

        private DataTable GetDataTable()
        {
            //string query = "SELECT dbo.CauHoi.CauHoi,dbo.CauHoi.DapAnA,dbo.CauHoi.DapAnB,dbo.CauHoi.DapAnC,dbo.CauHoi.DapAnD FROM dbo.CauHoi";
            //string query = @"SELECT dbo.CauHoi.MaCauHoi,dbo.CauHoi.MaTheLoai,dbo.CauHoi.CauHoi,dbo.CauHoi.DapAnA,dbo.CauHoi.DapAnB,dbo.CauHoi.DapAnC,dbo.CauHoi.DapAnD,dbo.DapAn.DapAn,dbo.TheLoai.TheLoai
            //                FROM dbo.CauHoi 
            //                JOIN dbo.DapAn ON DapAn.MaCauHoi = CauHoi.MaCauHoi
            //                JOIN dbo.TheLoai ON TheLoai.MaTheLoai = CauHoi.MaTheLoai";
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("GetDataTable", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dt);
                }
            }
            return dt;
        }

        public List<CauHoi> GetLstCauHoi()
        {
            List<CauHoi> cauHois = new List<CauHoi>();
            DataTable dt = GetDataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CauHoi cauHois1 = new CauHoi();
                cauHois1.MaCauHoi = dt.Rows[i]["MaCauHoi"].ToString();
                cauHois1.MaTheLoai = dt.Rows[i]["MaTheLoai"].ToString();
                cauHois1.NoiDung = dt.Rows[i]["CauHoi"].ToString();
                cauHois1.DapAnA = dt.Rows[i]["DapAnA"].ToString();
                cauHois1.DapAnB = dt.Rows[i]["DapAnB"].ToString();
                cauHois1.DapAnC = dt.Rows[i]["DapAnC"].ToString();
                cauHois1.DapAnD = dt.Rows[i]["DapAnD"].ToString();
                if (dt.Rows[i]["DapAn"].ToString() == "1") cauHois1.DapAn = "A";
                else if (dt.Rows[i]["DapAn"].ToString() == "2") cauHois1.DapAn = "B";
                else if (dt.Rows[i]["DapAn"].ToString() == "3") cauHois1.DapAn = "D";
                else cauHois1.DapAn = "D";
                cauHois1.TheLoai = dt.Rows[i]["TheLoai"].ToString();
                cauHois.Add(cauHois1);
            }
            return cauHois;
        }

        public List<LstTheLoai> GetTheLoai()
        {
            string query = "SELECT * FROM	dbo.TheLoai";
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dt);
                }
            }
            List<LstTheLoai> tl = new List<LstTheLoai>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LstTheLoai lstTheLoai = new LstTheLoai();
                lstTheLoai.MaTheLoai = dt.Rows[i]["MaTheLoai"].ToString();
                lstTheLoai.TheLoai = dt.Rows[i]["TheLoai"].ToString();
                tl.Add(lstTheLoai);
            }
            return tl;
        }

        public bool UpdateTheLoai(string MaCauHoi, string MaTheLoai)
        {
            //string query = @"UPDATE dbo.CauHoi
            //                SET dbo.CauHoi.MaTheLoai = @MaTheLoai
            //                WHERE MaCauHoi = @MaCauHoi";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("updateTheLoai", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaTheLoai", MaTheLoai);
                    command.Parameters.AddWithValue("@MaCauHoi", MaCauHoi);
                    int result = command.ExecuteNonQuery();
                    if (result < 1) return false;
                    else return true;
                }
            }
        }

        public bool SuaNoiDungCauHoi(CauHoi cauHoi)
        {
            if (cauHoi != null)
            {
                using (SqlConnection connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SuaNoiDungCauHoi", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MaCauHoi", cauHoi.MaCauHoi)
                    }
                }
            }
            return false;
        }

        public bool XoaCauHoi(string MaCauHoi)
        {
            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DeleteWithQuestion", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaCauHoi", MaCauHoi);
                    int result = command.ExecuteNonQuery();
                    if (result < 1) return false;
                    else return true;
                }
            }
        }
    }
}
