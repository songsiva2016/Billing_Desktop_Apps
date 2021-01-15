using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmDeaCust : Form
    {
        public frmDeaCust()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Write the code to close this form
            this.Hide();
        }

        DeaCustBLL dc = new DeaCustBLL();
        DeaCustDAL dcDal = new DeaCustDAL();

        userDAL uDal = new userDAL();
        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {

                //Get the Values from Form
                dc.p_Name = txtName.Text;
                dc.p_Contact = txtContact.Text.ToString();
                dc.p_Address1 = txtAddress.Text;
                dc.p_TaxNumber = txtTaxNumber.Text;
                dc.p_State = cmbDeaCust.Text;
                dc.p_Datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                dc.p_StateCode = int.Parse(txtStateCode.Text);
                //Getting the ID to Logged in user and passign its value in dealer or cutomer module
                string loggedUsr = frmLogin.loggedIn;
                userBLL usr = uDal.GetIDFromUsername(loggedUsr);
                dc.p_AddedBy = usr.id;

                //Creating boolean variable to check whether the dealer or cutomer is added or not
                bool success = dcDal.Insert(dc);

                if (success == true)
                {
                    //Dealer or Cutomer inserted successfully 
                    MessageBox.Show("Dealer or Customer Added Successfully");
                    Clear();
                    //Refresh Data Grid View
                    DataTable dt = dcDal.Select();
                    dgvDeaCust.DataSource = dt;
                }
                else
                {
                    //failed to insert dealer or customer
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
           
        }
        public void Clear()
        {

            try
            {
                txtDeaCustID.Text = "";
                txtName.Text = "";
                txtContact.Text = "";
                txtTaxNumber.Text = "";
                txtAddress.Text = "";
                txtSearch.Text = "";
                txtStateCode.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {

            try
            {
                //Refresh Data Grid View
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;


            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
           
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            try
            {

                //int variable to get the identityof row clicked
                int rowIndex = e.RowIndex;

                txtDeaCustID.Text = dgvDeaCust.Rows[rowIndex].Cells[0].Value.ToString();
                txtName.Text = dgvDeaCust.Rows[rowIndex].Cells[1].Value.ToString();
                txtAddress.Text = dgvDeaCust.Rows[rowIndex].Cells[2].Value.ToString();
                txtTaxNumber.Text = dgvDeaCust.Rows[rowIndex].Cells[3].Value.ToString();
                cmbDeaCust.Text = dgvDeaCust.Rows[rowIndex].Cells[4].Value.ToString();
                txtContact.Text = dgvDeaCust.Rows[rowIndex].Cells[5].Value.ToString();
                txtStateCode.Text= dgvDeaCust.Rows[rowIndex].Cells[6].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the values from Form
                dc.p_ID = int.Parse(txtDeaCustID.Text);
                dc.p_Name = txtName.Text;
                dc.p_Address1 = txtAddress.Text;
                dc.p_Contact = txtContact.Text;
                dc.p_TaxNumber = txtTaxNumber.Text;
                dc.p_State = cmbDeaCust.Text;
                dc.p_StateCode =int.Parse(txtStateCode.Text);
                dc.p_Datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                //Getting the ID to Logged in user and passign its value in dealer or cutomer module
                string loggedUsr = frmLogin.loggedIn;
                userBLL usr = uDal.GetIDFromUsername(loggedUsr);
                dc.p_AddedBy = usr.id;

                //create boolean variable to check whether the dealer or customer is updated or not
                bool success = dcDal.Update(dc);

                if (success == true)
                {
                    //Dealer and Customer update Successfully
                    MessageBox.Show("Dealer or Customer updated Successfully");
                    Clear();
                    //Refresh the Data Grid View
                    DataTable dt = dcDal.Select();
                    dgvDeaCust.DataSource = dt;
                }
                else
                {
                    //Failed to udate Dealer or Customer
                    MessageBox.Show("Failed to Udpate Dealer or Customer");
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            //Get the id of the user to be deleted from form
            dc.p_ID = int.Parse(txtDeaCustID.Text);

            //Create boolean variable to check wheteher the dealer or customer is deleted or not
            bool success = dcDal.Delete(dc);

            if (success == true)
            {
                //Dealer or Customer Deleted Successfully
                MessageBox.Show("Dealer or Customer Deleted Successfully");
                Clear();
                //Refresh the Data Grid View
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //Dealer or Customer Failed to Delete
                MessageBox.Show("Failed to Delete Dealer or Customer");
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Get the keyowrd from text box
                string keyword = txtSearch.Text;

                if (keyword != null)
                {
                    //Search the Dealer or Customer
                    DataTable dt = dcDal.Search(keyword);
                    dgvDeaCust.DataSource = dt;
                }
                else
                {
                    //Show all the Dealer or Customer
                    DataTable dt = dcDal.Select();
                    dgvDeaCust.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            
        }
    }
}
