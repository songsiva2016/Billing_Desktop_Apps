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
    class transactionDAL
    {
        public static string sqlLiteConnstring = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + Application.StartupPath + "\\Customerservice.accdb";
        static string myconnstrng = "Data Source=" + Application.StartupPath + "\\CustDemo.db; Version=3;New=True;Compress=True;";//"Provider=Microsoft.ACE.Oledb.12.0;Data Source=Customerservice.accdb"; //ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        DataTable dt = new DataTable();
        #region Insert Transaction Method
        public int GetNextInvoiceNo()
        {
            try
            {
                string sql;

                //if(InvoiceType== "TAX-INVOICE")
                //{

                     sql = "SELECT Max(ID) FROM tbl_Transaction";

                //}
                //else if(InvoiceType=="REIMBURSEMENT")
                //{
                //     sql = "SELECT Max(ID) FROM tbl_Transaction";
                //}
                SQLiteConnection conn = new SQLiteConnection(myconnstrng);
                
                DataTable dt = new DataTable();
                //SQLiteCommand to Execute Query
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                //SQLiteDataAdapter to Hold the data from database
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();

                adapter.Fill(dt);

                int invoiceNo = int.Parse(dt.Rows[0][0].ToString());
                return invoiceNo;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        #endregion
        public bool CheckIfAlreadyExist_Transaction(transactionsBLL t)
        { 
        try
            {
                SQLiteConnection conn = new SQLiteConnection(myconnstrng);
                string sql = "select * from tbl_Transaction where InvoiceNo='" + t.InvoiceNo + "'";

                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();

                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    return true;
                }
                else
                {
                   return false;
                }
            }
            catch(Exception ex)
            {


                return false;
            }
        
        }

        #region Insert Transaction Method
        public bool Insert_Transaction(transactionsBLL t, out int transactionID)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;
            //Set the out transactionID value to negative 1 i.e. -1
            transactionID = -1;
            int rows = -1;
            //Create a SQLiteConnection first
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            try
            {

               
           if (!CheckIfAlreadyExist_Transaction(t))
            {
                    //SQL Query to Insert Transactions
                    string sql = "INSERT INTO tbl_Transaction(InvoiceNo, InvoiceDate, cust_id, grandTotal, transaction_date, sGST, cGST,discount,added_by,trans_Type) VALUES ('" + t.InvoiceNo + "', '" + t.InvoiceDate + "'," + t.cust_id + "," + t.grandTotal + ", '" + t.transaction_date + "'," + t.sGST + "," + t.cGST + "," + t.discount + "," + t.added_by + ",'" + t.Invoice_Type + "')";
                    //SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                   //Open Database Connection
                    conn.Open();

                    //Execute the Query
                     rows = cmd.ExecuteNonQuery();
                    transactionID = GetNextInvoiceNo();
                }
                else
                {
                    string sql = "update tbl_Transaction set cust_id=" + t.cust_id + ", grandTotal=" + t.grandTotal + ",InvoiceDate='" + t.InvoiceDate + "',sGST=" + t.sGST + ",cGST=" + t.cGST + ",discount=" + t.discount + ",trans_Type='" + t.Invoice_Type + "' where InvoiceNo='" + t.InvoiceNo + "'";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    conn.Open();
                    //Execute the Query
                    rows = cmd.ExecuteNonQuery();
                    conn.Close();


                    string getIDsql = "select ID from tbl_Transaction where InvoiceNo='" + t.InvoiceNo + "'";
                    SQLiteCommand getcmd = new SQLiteCommand(getIDsql, conn);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(getcmd);
                    conn.Open();
                    //Execute the Query
                    adapter.Fill(dt);
                    int transID = int.Parse(dt.Rows[0][0].ToString());
                    conn.Close();

                    sql = "delete from tbl_TransDetails where transID=" + transID + "";
                    SQLiteCommand deletecmd = new SQLiteCommand(sql, conn);
                    conn.Open();
                    //Execute the Query
                    rows = deletecmd.ExecuteNonQuery();
                    conn.Close();
                    transactionID = transID;
                }
                    //If the query is executed successfully then the value will not be null else it will be null
                    if (rows > 0)
                    {
                        //Query Executed Successfully

                        isSuccess = true;
                    }
                    else
                    {
                        //failed to execute query
                        isSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    //Close the connection 
                    conn.Close();
                }
                
                return isSuccess;
        
        }
        #endregion
        #region METHOD TO DISPLAY ALL THE TRANSACTION
        public DataTable DisplayAllTransactions()
        {
            //SQLiteConnection First
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create a DAta Table to hold the datafrom database temporarily
            DataTable dt = new DataTable();

            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "SELECT * FROM tbl_transactions";

                //SQLiteCommand to Execute Query
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                //SQLiteDataAdapter to Hold the data from database
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();

                adapter.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        #endregion
        #region METHOD TO DISPLAY TRANSACTION BASED ON TRANSACTION TYPE
        public DataTable DisplayTransactionByType(string type)
        {
            //Create SQL Connection
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create a DataTable
            DataTable dt = new DataTable();

            try
            {
                //Write SQL Query
                string sql = "SELECT * FROM tbl_transactions WHERE type='"+type+"'";

                //SQL Command to Execute Query
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                //SQLiteDataAdapter to hold the data from database
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();
                adapter.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        #endregion



        public DataTable FetchInvoiceNo(DateTime InvoiceDate)
        {
            //SQLiteConnection First
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create a DAta Table to hold the datafrom database temporarily
            DataTable dt = new DataTable();

            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "Select InvoiceNo from tbl_Transaction where Date(InvoiceDate)='" + InvoiceDate.ToString("yyyy-MM-dd") + "'";

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

        public DataTable fetchExistingInvoice(string  InvoiceNumber)
        {
            //SQLiteConnection First
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create a DAta Table to hold the datafrom database temporarily
            DataTable dt = new DataTable();

            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "select tp.p_Name,tp.p_TaxNumber,tp.p_Contact,th.InvoiceNo,th.InvoiceDate,th.grandTotal,th.discount,(th.grandTotal-(th.sGST+th.cGST)) as TotalAmt, th.sGST,th.cGST,th.ID,th.trans_Type from tbl_Transaction th Inner Join tbl_PartyDetails tp ON th.cust_id=tp.ID where th.InvoiceNo='" + InvoiceNumber + "'";

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
