using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Data;

namespace AnyStore.DAL
{
    class transactionDetailDAL
    {
        public static string sqlLiteConnstring = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + Application.StartupPath + "\\Customerservice.accdb";
        static string myconnstrng = "Data Source=" + Application.StartupPath + "\\CustDemo.db; Version=3;New=True;Compress=True;";//"Provider=Microsoft.ACE.Oledb.12.0;Data Source=Customerservice.accdb"; //ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Insert Method for Transaction Detail
        public bool InsertTransactionDetail(transactionDetailBLL td)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;

            //Create a database connection here
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            try
            {
                //Sql Query to Insert Transaction detais
                string sql = "INSERT INTO tbl_TransDetails(service_ID, transID, rate, qty, total, added_date, added_by,SAC_Code,ServiceName,Description) VALUES "
                    + "(" + 1 + ", " + td.trans_ID + "," + td.rate + ", " + td.qty + ", " + td.total + ",'" + td.added_date + "'," + td.added_by + ",'" + td.SAC_Code + "','" + td.service_Name + "','" + td.Service_Desc+ "')";

                //Passing the value to the SQL Query
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                //Passing the values using cmd
                //cmd.Parameters.AddWithValue("@service_ID", td.service_Name);
                //cmd.Parameters.AddWithValue("@transID", td.trans_ID);
                //cmd.Parameters.AddWithValue("@rate", td.rate);
                //cmd.Parameters.AddWithValue("@qty", td.qty);
                //cmd.Parameters.AddWithValue("@total", td.total);
                //cmd.Parameters.AddWithValue("@added_date", td.added_date);
                //cmd.Parameters.AddWithValue("@added_by", td.added_by);

                //Open Database connection
                conn.Open();

                //declare the int variable and execute the query
                int rows = cmd.ExecuteNonQuery();

                if(rows>0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //FAiled to Execute Query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database Connection
                conn.Close();
            }
            return isSuccess;
        }
        #endregion


        public DataTable fetchExistingInvoiceDetails(int transID)
        {
            //SQLiteConnection First
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create a DAta Table to hold the datafrom database temporarily
            DataTable dt = new DataTable();

            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "select [ServiceName] as ServiceName,rate as Rate,qty as Quantity,[total] as Total,SAC_Code as SACCode,Description as Description from tbl_TransDetails where transID= '" + transID + "'";

                //SQLiteCommand to Execute Query
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                //SQLiteDataAdapter to Hold the data from database
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();

                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
    }
}
