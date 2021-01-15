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
    class DeaCustDAL
    {
        public static string sqlLiteConnstring = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + Application.StartupPath + "\\Customerservice.accdb";
        static string myconnstrng = "Data Source=" + Application.StartupPath + "\\CustDemo.db; Version=3;New=True;Compress=True;";//"Provider=Microsoft.ACE.Oledb.12.0;Data Source=Customerservice.accdb"; //ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region SELECT MEthod for Dealer and Customer
        public DataTable Select()
        {
            //SQL Connection for Database Connection
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            //DataTble to hold the value from database and return it
            DataTable dt = new DataTable();
            try
            {
                //Write SQL Query t Select all the DAta from dAtabase
                string sql = "SELECT ID,p_Name as Name,p_Address1 as Address,p_TaxNumber as GST_PAN,p_State as State,p_Contact as Contact,p_StateCode FROM tbl_PartyDetails";

                //Creating SQL Command to execute Query
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                //Creting SQL Data Adapter to Store Data From Database Temporarily
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                //Open Database Connection
                conn.Open();
                //Passign the value from SQL Data Adapter to DAta table
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
        #region INSERT Method to Add details fo Dealer or Customer
        public bool Insert(DeaCustBLL dc)
        {
            //Creting SQL Connection First
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create and Boolean value and set its default value to false
            bool isSuccess = false;

            try
            {
                //Write SQL Query to Insert Details of Dealer or Customer
                string sql = "INSERT INTO tbl_PartyDetails(p_Name, p_Address1, p_Address2, p_TaxNumber, p_State, p_Contact, p_AddedBy,p_Datetime,p_StateCode) VALUES('" + dc.p_Name + "','" + dc.p_Address1 + "','" + dc.p_Address1 + "','" + dc.p_TaxNumber + "','" + dc.p_State + "','" + (dc.p_Contact) + "','" + dc.p_AddedBy + "','" + dc.p_Datetime + "','" + dc.p_StateCode + "')";
                //string sql = "INSERT INTO tbl_PartyDetails(p_Name, p_Address1, p_Address2, p_TaxNumber, p_State, p_Contact, p_AddedBy,p_Datetime) VALUES('" + dc.p_Name + "', '" + dc.p_Address1 + "', '" + dc.p_Address1 + "','" + dc.p_TaxNumber + "', '" + dc.p_State + "', '" + dc.p_Contact + "', '" + dc.p_AddedBy + "', '" + dc.p_Datetime + "')";
                //SQl Command to Pass the values to query and execute
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                //Passing the calues using Parameters
                //cmd.Parameters.AddWithValue("@p_Name", dc.p_Name);
                //cmd.Parameters.AddWithValue("@p_Address1", dc.p_Address1);
                //cmd.Parameters.AddWithValue("@p_Address2", dc.p_Address1);
                //cmd.Parameters.AddWithValue("@p_TaxNumber", dc.p_TaxNumber);
                //cmd.Parameters.AddWithValue("@p_State", dc.p_State);
                //cmd.Parameters.AddWithValue("@p_Contact", dc.p_Contact);
                //cmd.Parameters.AddWithValue("@p_AddedBy", dc.p_AddedBy);
                //cmd.Parameters.AddWithValue("@p_Datetime", dc.p_Datetime);
                //Open DAtabaseConnection
                conn.Open();

                //Int variable to check whether the query is executed successfully or not
                int rows = cmd.ExecuteNonQuery();

                //If the query is executed successfully then the value of rows will be greater than 0 else it will be less than 0
                if(rows>0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
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
        #endregion
        #region UPDATE method for Dealer and Customer Module
        public bool Update(DeaCustBLL dc)
        {
            //SQL Connection for Database Connection
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            //Create Boolean variable and set its default value to false
            bool isSuccess = false;

            try
            {
                //SQL Query to update data in database
                string sql = "UPDATE tbl_PartyDetails SET p_Name='" + dc.p_Name + "', p_Address1='" + dc.p_Address1 + "', p_Address2='" + dc.p_Address1 + "', p_Contact='" + dc.p_Contact + "',p_TaxNumber='" + dc.p_TaxNumber + "',p_State='" + dc.p_State + "',p_AddedBy='" + dc.p_AddedBy + "',p_datetime='" + dc.p_Datetime + "',p_StateCode='" + dc.p_StateCode + "' WHERE ID='" + dc.p_ID + "'";
                //Create SQL Command to pass the value in sql
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                //Passing the values through parametersp_Contact
                //cmd.Parameters.AddWithValue("@p_Name", dc.p_Name);
                //cmd.Parameters.AddWithValue("@p_Address1", dc.p_Address1);
                //cmd.Parameters.AddWithValue("@p_Address2", dc.p_Address1);
                //cmd.Parameters.AddWithValue("@p_Contact", dc.p_Contact);
                //cmd.Parameters.AddWithValue("@p_TaxNumber", dc.p_TaxNumber);
                //cmd.Parameters.AddWithValue("@p_State", dc.p_State);
                //cmd.Parameters.AddWithValue("@p_AddedBy", dc.p_AddedBy);
                //cmd.Parameters.AddWithValue("@p_Datetime", dc.p_Datetime);
                //cmd.Parameters.AddWithValue("@id", dc.p_ID);


                //open the Database Connection
                conn.Open();

                //Int varialbe to check if the query executed successfully or not
                int rows = cmd.ExecuteNonQuery();
                if(rows>0)
                {
                    //Query Executed Successfully 
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
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
        #endregion
        #region DELETE Method for Dealer and Customer Module
        public bool Delete(DeaCustBLL dc)
        {
            //SQLiteConnection for Database Connection
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create a Boolean Variable and set its default value to false
            bool isSuccess = false;

            try
            {
                //SQL Query to Delete Data from dAtabase
                string sql = "DELETE FROM tbl_PartyDetails WHERE ID=@id";

                //SQL command to pass the value
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                //Passing the value
                cmd.Parameters.AddWithValue("@id", dc.p_ID);

                //Open DB Connection
                conn.Open();
                //integer variable
                int rows = cmd.ExecuteNonQuery();
                if(rows>0)
                {
                    //Query Executed Successfully 
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
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
        #endregion
        #region SEARCH METHOD for Dealer and Customer Module
        public DataTable Search(string keyword)
        {
            //Create a Sql Connection
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Creating a Data TAble and returnign its value
            DataTable dt = new DataTable();

            try
            {
                //Write the Query to Search Dealer or Customer Based in id, type and name
                string sql = "SELECT ID,p_Name,p_Address1,p_TaxNumber,p_State,p_Contact,p_StateCode FROM tbl_PartyDetails WHERE ID LIKE '%" + keyword + "%' OR p_Name LIKE '%" + keyword + "%' OR p_TaxNumber LIKE '%" + keyword + "%'";

                //Sql Command to Execute the Query
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                //Sql Dat Adapeter to hold tthe data from dataase temporarily
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                //Open DAta Base Connection
                conn.Open();
                //Pass the value from adapter to data table
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
        #region Siva==>METHOD TO SAERCH DEALER Or CUSTOMER FOR TRANSACTON MODULE
        public DeaCustBLL SearchDealerCustomerForTransaction(string keyword)
        {
            //Create an object for DeaCustBLL class
            DeaCustBLL dc = new DeaCustBLL();

            //Create a DAtabase Connection
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create a DAta Table to hold the value temporarily
            DataTable dt = new DataTable();

            try
            {
                //Write a SQL Query to Search Dealer or Customer Based on Keywords
                string sql = "SELECT p_Name, p_Contact, p_TaxNumber, p_Address1,p_StateCode from tbl_PartyDetails WHERE ID LIKE '%" + keyword + "%' OR p_Name LIKE '%" + keyword + "%' OR p_Contact LIKE '%" + keyword + "%'";

                //Create a Sql Data Adapter to Execute the Query
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);

                //Open the DAtabase Connection
                conn.Open();

                //Transfer the data from SqlData Adapter to DAta Table
                adapter.Fill(dt);

                //If we have values on dt we need to save it in dealerCustomer BLL
                if(dt.Rows.Count>0)
                {
                    dc.p_Name = dt.Rows[0]["p_Name"].ToString();
                    dc.p_Contact = (dt.Rows[0]["p_Contact"].ToString());
                    dc.p_TaxNumber = dt.Rows[0]["p_TaxNumber"].ToString();
                    dc.p_Address1 = dt.Rows[0]["p_Address1"].ToString();
                    dc.p_StateCode = int.Parse(dt.Rows[0]["p_StateCode"].ToString());

                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database connection
                conn.Close();
            }

            return dc;
        }
        #endregion

        public companyBLL SearchBillingSetting()
        {
            //Create an object for DeaCustBLL class
            companyBLL compDetails = new companyBLL();

            //Create a DAtabase Connection
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            //Create a DAta Table to hold the value temporarily
            DataTable dt = new DataTable();

            try
            {
                //Write a SQL Query to Search Dealer or Customer Based on Keywords
                string sql = "SELECT Company_Name, GST_Num, SGST, CGST from tbl_AppSetting";

                //Create a Sql Data Adapter to Execute the Query
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);

                //Open the DAtabase Connection
                conn.Open();

                //Transfer the data from SqlData Adapter to DAta Table
                adapter.Fill(dt);

                //If we have values on dt we need to save it in dealerCustomer BLL
                if (dt.Rows.Count > 0)
                {
                    compDetails.Company_Name = dt.Rows[0]["Company_Name"].ToString();
                    compDetails.GST_Num = dt.Rows[0]["GST_Num"].ToString();
                    compDetails.SGST = int.Parse(dt.Rows[0]["SGST"].ToString());
                    compDetails.CGST = int.Parse(dt.Rows[0]["CGST"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database connection
                conn.Close();
            }

            return compDetails;
        }


        #region METHOD TO GET ID OF THE DEALER OR CUSTOMER BASED ON NAME
        public DeaCustBLL GetDeaCustIDFromName(string Name)
        {
            //First Create an Object of DeaCust BLL and REturn it
            DeaCustBLL dc = new DeaCustBLL();

            //SQL Conection here
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            //Data TAble to Holdthe data temporarily
            DataTable dt = new DataTable();

            try
            {
                //SQL Query to Get id based on Name
                string sql = "SELECT ID FROM tbl_PartyDetails WHERE p_Name='" + Name + "'";
                //Create the SQL Data Adapter to Execute the Query
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);

                conn.Open();

                //Passing the CAlue from Adapter to DAtatable
                adapter.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    //Pass the value from dt to DeaCustBLL dc
                    dc.p_ID = int.Parse(dt.Rows[0]["ID"].ToString());
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

            return dc;
        }
        #endregion

    }
}
