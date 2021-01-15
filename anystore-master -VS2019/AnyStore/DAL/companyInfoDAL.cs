using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using AnyStore.BLL;
using System.Data.SQLite;

namespace AnyStore.DAL
{
     public class companyInfoDAL
    {
        public static string sqlLiteConnstring = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + Application.StartupPath + "\\Customerservice.accdb";
        static string myconnstrng = "Data Source=" + Application.StartupPath + "\\CustDemo.db; Version=3;New=True;Compress=True;";//"Provider=Microsoft.ACE.Oledb.12.0;Data Source=Customerservice.accdb"; //ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region SEARCH Method for Product Module
        public companyBLL GetCompanyData()
         {
             //SQL Connection fro DB Connection
             SQLiteConnection conn = new SQLiteConnection(myconnstrng);
             //Creating DAtaTable to hold value from dAtabase
             DataTable dt = new DataTable();
             companyBLL companyInfo = new companyBLL();
             try
             {
                 //SQL query to search product
                 string sql = "SELECT * FROM tbl_AppSetting";
                 //Sql Command to execute Query
                 SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                 //SQL Data Adapter to hold the data from database temporarily
                 SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                 //Open Database Connection
                 conn.Open();

                 adapter.Fill(dt);

                 if (dt.Rows.Count > 0)
                 {
                     companyInfo.id = int.Parse(dt.Rows[0]["id"].ToString());
                     companyInfo.Company_Name = (dt.Rows[0]["Company_Name"].ToString());
                     companyInfo.Address1 = (dt.Rows[0]["Address1"].ToString());
                     companyInfo.GST_Num = (dt.Rows[0]["GST_Num"].ToString());
                     companyInfo.PanNo = (dt.Rows[0]["PanNo"].ToString());
                     companyInfo.Contact = (dt.Rows[0]["Contact"].ToString());
                     companyInfo.State = (dt.Rows[0]["State"].ToString());
                     companyInfo.SGST = int.Parse(dt.Rows[0]["SGST"].ToString());
                     companyInfo.CGST = int.Parse(dt.Rows[0]["CGST"].ToString());
                    companyInfo.StateCode = int.Parse(dt.Rows[0]["StateCode"].ToString());
      
                 }
                 

             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
             finally
             {
                 conn.Close();
             }

             return companyInfo;
         }
         #endregion

         #region Method to Insert companyDetails in database
         public bool update(companyBLL p)
         {
             //Creating Boolean Variable and set its default value to false
             bool isSuccess = false;

             //Sql Connection for DAtabase
             SQLiteConnection conn = new SQLiteConnection(myconnstrng);

             try
             {

                 //SQL Query to insert company Details into database
                 String sql = "update tbl_AppSetting set Company_Name=@Company_Name,Address1=@Address1,GST_Num=@GST_Num,PanNo=@PanNo,Contact=@Contact,State=@State,SGST=@SGST,CGST=@CGST,StateCode=@StateCode";

                 //Creating SQL Command to pass the values
                 SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                 //Passign the values through parameters
                 cmd.Parameters.AddWithValue("@Company_Name", p.Company_Name);
                 cmd.Parameters.AddWithValue("@Address1", p.Address1);
                 cmd.Parameters.AddWithValue("@GST_Num", p.GST_Num);
                 cmd.Parameters.AddWithValue("@PanNo", p.PanNo);
                 cmd.Parameters.AddWithValue("@Contact", p.Contact);
                 cmd.Parameters.AddWithValue("@State", p.State);
                 cmd.Parameters.AddWithValue("@CGST", p.CGST);
                 cmd.Parameters.AddWithValue("@SGST", p.SGST);
                cmd.Parameters.AddWithValue("@StateCode", p.StateCode);


                //Opening the Database connection
                conn.Open();

                 int rows = cmd.ExecuteNonQuery();

                 //If the query is executed successfully then the value of rows will be greater than 0 else it will be less than 0
                 if (rows > 0)
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
             catch (Exception ex)
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

         #region Method to Insert companyDetails in database
         public bool Insert(companyBLL p)
         {
             //Creating Boolean Variable and set its default value to false
             bool isSuccess = false;

             //Sql Connection for DAtabase
             SQLiteConnection conn = new SQLiteConnection(myconnstrng);

             try
             {

                 //SQL Query to insert company Details into database
                 String sql = "INSERT INTO tbl_AppSetting(Company_Name, Address1,Address2,Address3, GST_Num, PanNo, Contact, State,SGST,CGST,StateCode) VALUES (@Company_Name, @Address1,@Address2,@Address3, @GST_Num, @PanNo, @Contact, @State,@SGST,@CGST,@StateCode)";
                 
                 //Creating SQL Command to pass the values
                 SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                 //Passign the values through parameters
                 cmd.Parameters.AddWithValue("@Company_Name", p.Company_Name);
                 cmd.Parameters.AddWithValue("@Address1", p.Address1);
                cmd.Parameters.AddWithValue("@Address2", "No Data");
                cmd.Parameters.AddWithValue("@Address3", "No Data");
                cmd.Parameters.AddWithValue("@GST_Num", p.GST_Num);
                 cmd.Parameters.AddWithValue("@PanNo", p.PanNo);
                 cmd.Parameters.AddWithValue("@Contact", p.Contact);
                 cmd.Parameters.AddWithValue("@State", p.State);
                 cmd.Parameters.AddWithValue("@SGST", p.SGST);
                 cmd.Parameters.AddWithValue("@CGST", p.CGST);
                cmd.Parameters.AddWithValue("@StateCode", p.StateCode);


                //Opening the Database connection
                conn.Open();

                 int rows = cmd.ExecuteNonQuery();

                 //If the query is executed successfully then the value of rows will be greater than 0 else it will be less than 0
                 if (rows > 0)
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
             catch (Exception ex)
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


         public bool CheckIfDataExists(string companyName)
         {
             //SQL Connection fro DB Connection
             SQLiteConnection conn = new SQLiteConnection(myconnstrng);
             //Creating DAtaTable to hold value from dAtabase
             DataTable dt = new DataTable();
             try {
                 //SQL query to search product
                 string sql = "SELECT * FROM tbl_AppSetting where Company_Name LIKE '%" + companyName + "%' ";
                 //Sql Command to execute Query
                 SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                 //SQL Data Adapter to hold the data from database temporarily
                 SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                 //Open Database Connection
                 conn.Open();

                 adapter.Fill(dt);

                 if (dt.Rows.Count > 0)
                 {
                     return true;
                 }
                 else return false;
             
             }
             catch (Exception ex)
             {
                MessageBox.Show("" + ex.Message);
                return false;
             }
         
         }

    }
}
