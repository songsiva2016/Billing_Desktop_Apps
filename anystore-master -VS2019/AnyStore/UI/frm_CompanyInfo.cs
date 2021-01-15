using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnyStore.DAL;
using AnyStore.BLL;

namespace AnyStore.UI
{
    public partial class frm_CompanyInfo : Form
    {
        public frm_CompanyInfo()
        {
            InitializeComponent();
        }

        companyInfoDAL cinfo = new companyInfoDAL();
        companyBLL companyInfo = new companyBLL();


        private void frm_CompanyInfo_Load(object sender, EventArgs e)
        {
            try {
                companyBLL companyBL = cinfo.GetCompanyData();
                if (companyBL.id != 0)
                {
                    txtCompanyName.Text = companyBL.Company_Name;
                    txtAddress.Text = companyBL.Address1;
                    txtContact.Text = companyBL.Contact;
                    txtGST.Text = companyBL.GST_Num;
                    txtPanNumber.Text = companyBL.PanNo;
                    txtState.Text = companyBL.State;
                    txtCGST.Text = companyBL.CGST.ToString();
                    txtSGST.Text = companyBL.SGST.ToString();
                    txtStateCode.Text = companyBL.StateCode.ToString();
                }
            }
            catch (Exception ex) 
            {

                MessageBox.Show("" + ex.Message); 
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try {

                bool success = false;

                //Get All the Values from Product Form
                companyInfo.Company_Name = txtCompanyName.Text;
                companyInfo.Address1 = txtAddress.Text;
                companyInfo.GST_Num = txtGST.Text;
                companyInfo.PanNo = txtPanNumber.Text;
                companyInfo.Contact = txtContact.Text;
                companyInfo.State = txtState.Text;
                companyInfo.SGST = int.Parse(txtSGST.Text);
                companyInfo.CGST = int.Parse(txtCGST.Text);
                companyInfo.StateCode = int.Parse(txtStateCode.Text);
                if (cinfo.CheckIfDataExists(companyInfo.Company_Name))
                {
                    success = cinfo.update(companyInfo);
                }
                else
                {
                    success = cinfo.Insert(companyInfo);
                }
                //if the companyInfo is added successfully then the value of success will be true else it will be false
                if (success == true)
                {
                    //companyInfo Inserted Successfully
                    MessageBox.Show("companyInfo Added Successfully");
                    this.Close();
                }
                else
                {
                    //Failed to Add New product
                    MessageBox.Show("Failed to Setup for companyInfo");
                }
            
            }
            catch (Exception ex)
            { 
            
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
