using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class reportDAL
    {

        public static string sqlLiteConnstring = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + Application.StartupPath + "\\Customerservice.accdb";
        static string myconnstrng = "Data Source=" + Application.StartupPath + "\\CustDemo.db; Version=3;New=True;Compress=True;";//"Provider=Microsoft.ACE.Oledb.12.0;Data Source=Customerservice.accdb"; //ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        SQLiteConnection con;
        SQLiteDataAdapter da;
        SQLiteCommand cmd;
        DataSet ds;
        DataSet1 dataSet1 = new DataSet1();
        #region Select Data from Database
        public DataTable SelectData(int transID)
        {
            //Static MEthod to connect Database
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            //TO hold the data from database 
            DataTable dt = new DataTable();
            try
            {
                //SQL Query to Get Data From DAtabase
                String sql = "SELECT tr.InvoiceNo, tr.InvoiceDate, pd.p_Name, pd.p_TaxNumber, trDet.ServiceName, trDet.rate, trDet.SAC_Code, trDet.qty, trDet.total, tr.sGST, tr.cGST, tr.grandTotal, pd.p_Address1, pd.p_State, pd.p_Contact,trDet.Description,tr.trans_Type" +
                    " FROM ((tbl_Transaction tr INNER JOIN "+
                                             "tbl_TransDetails trDet ON tr.ID = trDet.transID) INNER JOIN "+
                                             "tbl_PartyDetails pd ON tr.cust_id = pd.ID)"+
                                            
                    " WHERE (tr.ID = " + transID + ")";

                //For Executing Command
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

               // cmd.Parameters.AddWithValue("@TransID", transID);
                //Getting DAta from dAtabase
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                //Database Connection Open
                conn.Open();
                
                //Fill Data in our DataTable
                adapter.Fill(dataSet1.BillingDT);
                //dt = dataSet1.BillingDT;
            }
            catch (Exception ex)
            {
                //Throw Message if any error occurs
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Closing Connection
                conn.Close();
            }
            //Return the value in DataTable
            return dataSet1.BillingDT;
        }
        #endregion

    }
}
