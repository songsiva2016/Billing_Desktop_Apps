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
    class userDAL
    {
        public static string sqlLiteConnstring = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + Application.StartupPath + "\\Customerservice.accdb";
        static string myconnstrng = "Data Source=" + Application.StartupPath + "\\CustDemo.db; Version=3;New=True;Compress=True;";//"Provider=Microsoft.ACE.Oledb.12.0;Data Source=Customerservice.accdb"; //ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        SQLiteConnection con;
        SQLiteDataAdapter da;
        SQLiteCommand cmd;
        DataSet ds;
        

        #region Select Data from Database
        public DataTable Select()
        {
            //Static MEthod to connect Database
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            //TO hold the data from database 
            DataTable dt = new DataTable();
            try
            {
                //SQL Query to Get Data From DAtabase
                String sql = "select ID, first_name as Firstname, last_name as LastName, email as EMail, username as Username, [password] as PasswordDet, contact as Contact, address as Address, gender as Gender, user_type UserType, added_date, added_by from tbl_users";
                //For Executing Command
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                //Getting DAta from dAtabase
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                //Database Connection Open
                conn.Open();
                //Fill Data in our DataTable
                adapter.Fill(dt);
            }
            catch(Exception ex)
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
            return dt;
        }
        #endregion
        #region Insert Data in Database
        public bool Insert(userBLL u)
        {
            bool isSuccess = false;
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            try
            {
                String sql = "INSERT INTO tbl_users(first_name, last_name, email, username, [password], contact, address, gender, user_type, added_date, added_by) VALUES ('" + u.first_name + "', '" + u.last_name + "', '" + u.email + "','" + u.username + "', '" + u.password + "'," + u.contact + ",'" + u.address + "', '" + u.gender + "', '" + u.user_type + "', '" + u.added_date.ToString("yyyy-MM-dd hh:mm:ss") + "', " + u.added_by + ")";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                //cmd.Parameters.AddWithValue("@first_name", u.first_name);
                //cmd.Parameters.AddWithValue("@last_name", u.last_name);
                //cmd.Parameters.AddWithValue("@email", u.email);
                //cmd.Parameters.AddWithValue("@username", u.username);
                //cmd.Parameters.AddWithValue("@password", u.password);
                //cmd.Parameters.AddWithValue("@contact", u.contact);
                //cmd.Parameters.AddWithValue("@address", u.address);
                //cmd.Parameters.AddWithValue("@gender", u.gender);
                //cmd.Parameters.AddWithValue("@user_type", u.user_type);
                //cmd.Parameters.AddWithValue("@added_date", u.added_date);
                //cmd.Parameters.AddWithValue("@added_by", u.added_by);

                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if(rows>0)
                {
                    //Query Sucessfull
                    isSuccess = true;
                }
                else
                {
                    //Query Failed
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
        #region Update data in Database
        public bool Update(userBLL u)
        {
            bool isSuccess = false;
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            
            try
            {
                string sql = "UPDATE tbl_users SET first_name=@first_name, last_name=@last_name, email=@email, username=@username, [password]=@password, contact=@contact, address=@address, gender=@gender, user_type=@user_type, added_date=@added_date, added_by=@added_by WHERE id=@id";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                cmd.Parameters.AddWithValue("@first_name", u.first_name);
                cmd.Parameters.AddWithValue("@last_name", u.last_name);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@username", u.username);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@contact", u.contact);
                cmd.Parameters.AddWithValue("@address", u.address);
                cmd.Parameters.AddWithValue("@gender", u.gender);
                cmd.Parameters.AddWithValue("@user_type", u.user_type);
                cmd.Parameters.AddWithValue("@added_date", u.added_date);
                cmd.Parameters.AddWithValue("@added_by", u.added_by);
                cmd.Parameters.AddWithValue("@id", u.id);

                conn.Open();

                int rows = cmd.ExecuteNonQuery();
                if(rows>0)
                {
                    //Query Successfull
                    isSuccess = true;
                }
                else
                {
                    //Query Failed
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
        #region Delete Data from DAtabase
        public bool Delete(userBLL u)
        {
            bool isSuccess = false;
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);

            try
            {
                string sql = "DELETE FROM tbl_users WHERE id=@id";

                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", u.id);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if(rows>0)
                {
                    //Query Successfull
                    isSuccess = true;
                }
                else
                {
                    //Query Failed
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
        #region Search User on Database usingKeywords
        public DataTable Search(string keywords)
        {
            //Static MEthod to connect Database
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            //TO hold the data from database 
            DataTable dt = new DataTable();
            try
            {
                //SQL Query to Get Data From DAtabase
                String sql = "SELECT * FROM tbl_users WHERE id LIKE '%"+keywords+"%' OR first_name LIKE '%"+keywords+"%' OR last_name LIKE '%"+keywords+"%' OR username LIKE '%"+keywords+"%'";
                //For Executing Command
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                //Getting DAta from dAtabase
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                //Database Connection Open
                conn.Open();
                //Fill Data in our DataTable
                adapter.Fill(dt);
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
            return dt;
        }
        #endregion
        #region Getting User ID from Username
        public userBLL GetIDFromUsername (string username)
        {
            userBLL u = new userBLL();
            SQLiteConnection conn = new SQLiteConnection(myconnstrng);
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT ID FROM tbl_users WHERE username='"+username+"'";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                conn.Open();

                adapter.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    u.id = int.Parse(dt.Rows[0]["ID"].ToString());
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
            return u;
        }
        #endregion
    }
}
