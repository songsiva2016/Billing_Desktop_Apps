using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SQLite;

namespace AnyStore.DAL
{
    class loginDAL
    {
        public static string sqlLiteConnstring = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + Application.StartupPath + "\\Customerservice.accdb";
        static string myconnstrng = "Data Source=" + Application.StartupPath + "\\CustDemo.db; Version=3;New=True;Compress=True;";//"Provider=Microsoft.ACE.Oledb.12.0;Data Source=Customerservice.accdb"; //ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
      
        public bool loginCheck(loginBLL l)
        {
            //Create a boolean variable and set its value to false and return it
            bool isSuccess = false;

            //Connecting To DAtabase
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            try
            {
                //SQL Query to check login
                string sql = "SELECT * FROM tbl_users WHERE username=@username AND password=@password AND user_type=@user_type";

                //Creating SQL Command to pass value
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username", l.username);
                cmd.Parameters.AddWithValue("@password", l.password);
                cmd.Parameters.AddWithValue("@user_type", l.user_type);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                //Checking The rows in DataTable 
                if(dt.Rows.Count>0)
                {
                    //Login Sucessful
                    isSuccess = true;
                }
                else
                {
                    //Login Failed
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
    }
}
