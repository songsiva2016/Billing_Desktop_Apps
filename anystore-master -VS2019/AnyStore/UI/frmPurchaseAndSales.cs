using AnyStore.BLL;
using AnyStore.DAL;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public frmPurchaseAndSales()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();

        DataTable transactionDT = new DataTable();

        public decimal sGSTValue;
        public decimal cGSTValue;
        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            try
            {
                //Get the transactionType value from frmUserDashboard
                string type = frmUserDashboard.transactionType;
                //Set the value on lblTop
                lblTop.Text = type;
               
                //Specify Columns for our TransactionDataTable
                transactionDT.Columns.Add("ServiceName");
                transactionDT.Columns.Add("Rate");
                transactionDT.Columns.Add("Quantity");
                transactionDT.Columns.Add("Total");
                transactionDT.Columns.Add("SACCode");
                transactionDT.Columns.Add("Description");
                chkEditMode.Checked = false;
                cmbInvoiceType.Text = "TAX-INVOICE";



            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
          
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            try
            {
                //Get the keyword fro the text box
                string keyword = txtSearch.Text;

                if (keyword == "")
                {
                    //Clear all the textboxes
                    txtName.Text = "";
                    txtTaxNumber.Text = "";
                    txtContact.Text = "";

                    return;
                }

                //Write the code to get the details and set the value on text boxes
                DeaCustBLL dc = dcDAL.SearchDealerCustomerForTransaction(keyword);
                if (dc.p_Name != null)
                {
                    //Now transfer or set the value from DeCustBLL to textboxes
                    txtName.Text = dc.p_Name;
                    txtTaxNumber.Text = dc.p_TaxNumber;
                    txtContact.Text = dc.p_Contact.ToString();


                    companyBLL cBLL = dcDAL.SearchBillingSetting();
                    txtCGST.Text = cBLL.CGST.ToString();
                    txtSGST.Text = cBLL.SGST.ToString();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            
            


        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {

            try
            {
                //Get the keyword from productsearch textbox
                string keyword = txtSearchProduct.Text;
                //Check if we have value to txtSearchProduct or not
                if (keyword == "")
                {
                    txtServiceName.Text = "";
                    txtDescription.Text = "";
                    txtRate.Text = "";
                    TxtQty.Text = "";
                    return;
                }
                //Search the product and display on respective textboxes
                productsBLL p = pDAL.GetServiceDetails(keyword);
                if (p.ServiceName != null)
                {
                    txtServiceName.Text = p.ServiceName;
                    txtDescription.Text = p.Description;
                    txtRate.Text = p.Rate.ToString();
                    txtSACCode.Text = p.SAC_Code.ToString();
                    TxtQty.Text = "1";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtServiceName.Text != "")
                {
                    //Get Product Name, Rate and Qty customer wants to buy
                    string ServiceName = txtServiceName.Text;
                    decimal Rate = decimal.Parse(txtRate.Text);
                    decimal Qty = decimal.Parse(TxtQty.Text);
                    String Description = txtDescription.Text;
                    int SAC_Code = int.Parse(txtSACCode.Text);
                    decimal Total = Rate * Qty; //Total=RatexQty
                    string desc = txtDescription.Text;
                    //Display the Subtotal in textbox
                    //Get the subtotal value from textbox
                    decimal subTotal = decimal.Parse(txtSubTotal.Text);
                    subTotal = subTotal + Total;


                    //Check whether the product is selected or not
                    if (ServiceName == "")
                    {
                        //Display error MEssage
                        MessageBox.Show("Select the product first. Try Again.");
                    }
                    else
                    {
                        //Add product to the dAta Grid View
                        transactionDT.Rows.Add(ServiceName, Rate, Qty, Total, SAC_Code, desc);

                        //Show in DAta Grid View
                        dgvAddedProducts.DataSource = transactionDT;
                        //Display the Subtotal in textbox
                        txtSubTotal.Text = subTotal.ToString();


                        companyBLL cBLL = dcDAL.SearchBillingSetting();
                        txtCGST.Text = cBLL.CGST.ToString();
                        txtSGST.Text = cBLL.SGST.ToString();
                        taxCalculation();
                        //Clear the Textboxes
                        txtSearchProduct.Text = "";
                        txtServiceName.Text = "";
                        txtDescription.Text = "";
                        txtRate.Text = "0.00";
                        TxtQty.Text = "0.00";
                        txtSACCode.Text = "0";
                        txtDiscount.Text = "0";
                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Get the value fro discount textbox
                string value = txtDiscount.Text;

                if (value == "")
                {
                    //Display Error Message
                    MessageBox.Show("Please Add Discount First");
                }
                else
                {
                    //Get the discount in decimal value
                    //decimal subTotal = decimal.Parse(txtSubTotal.Text);
                    //decimal discount = decimal.Parse(txtDiscount.Text);

                    ////Calculate the grandtotal based on discount
                    //decimal grandTotal = ((100 - discount) / 100) * subTotal;

                    ////Display the GrandTotla in TextBox
                    //txtGrandTotal.Text = grandTotal.ToString();

                    taxCalculation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            
            
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            try
            {

                //Check if the grandTotal has value or not if it has not value then calculate the discount first
                //string check = txtGrandTotal.Text;
                //if(check=="")
                //{
                //    //Deisplay the error message to calcuate discount
                //    MessageBox.Show("Calculate the discount and set the Grand Total First.");
                //}
                //else
                //{
                //    //Calculate VAT
                //    //Getting the VAT Percent first
                //    decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                //    decimal SGST = decimal.Parse(txtSGST.Text);
                //    decimal CGST = decimal.Parse(txtCGST.Text);
                //    decimal grandTotalWithVAT = ((100 + SGST) / 100) * previousGT + ((100 + CGST) / 100) * previousGT;

                //    //Displaying new grand total with vat
                //    txtGrandTotal.Text = grandTotalWithVAT.ToString();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {

            try
            {
                //Get the paid amount and grand total
                decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
                decimal paidAmount = decimal.Parse(txtPaidAmount.Text);

                decimal returnAmount = paidAmount - grandTotal;

                //Display the return amount as well
                txtReturnAmount.Text = returnAmount.ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                //Get the Values from PurchaseSales Form First
                transactionsBLL transaction = new transactionsBLL();
                string dta = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
              
                //Lets get name of the dealer or customer first
                string deaCustName = txtName.Text;
                DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);
                transaction.cust_id = dc.p_ID;
                transaction.InvoiceNo = txtInvoiceNumber.Text;
                transaction.InvoiceDate = dta;
                transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text), 2);
                transaction.transaction_date = dta;
                //   transaction.tax = decimal.Parse(txtSGST.Text);
                transaction.discount = decimal.Parse(txtDiscount.Text);
                //taxCalculation();
                transaction.cGST = cGSTValue;
                transaction.sGST = sGSTValue;
                transaction.Invoice_Type = cmbInvoiceType.Text;
                //Get the Username of Logged in user
                string username = frmLogin.loggedIn;
                userBLL u = uDAL.GetIDFromUsername(username);

                transaction.added_by = u.id;
                //  transaction.transactionDetails = transactionDT;

                //Lets Create a Boolean Variable and set its value to false
                bool success = false;

                //Actual Code to Insert Transaction And Transaction Details
                //using (TransactionScope scope = new TransactionScope())
                //{
                int transactionID = -1;
                //Create aboolean value and insert transaction 
                bool w = tDAL.Insert_Transaction(transaction, out transactionID);
                if (w)
                {
                    //Use for loop to insert Transaction Details
                    for (int i = 0; i < dgvAddedProducts.Rows.Count; i++)
                    {
                        //Get all the details of the product
                        transactionDetailBLL transactionDetail = new transactionDetailBLL();
                        //Get the Product name and convert it to id
                        string ProductName = transactionDT.Rows[i][0].ToString();
                        productsBLL p = pDAL.GetProductIDFromName(ProductName);

                        transactionDetail.service_Name = ProductName;
                        transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                        transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                        transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()), 2);
                        transactionDetail.added_date = DateTime.Parse(dta);
                        transactionDetail.added_by = u.id;
                        transactionDetail.trans_ID = transactionID;
                        transactionDetail.SAC_Code = int.Parse(transactionDT.Rows[i][4].ToString());
                        transactionDetail.Service_Desc = transactionDT.Rows[i][5].ToString();
                        //Here Increase or Decrease Product Quantity based on Purchase or sales
                        string transactionType = lblTop.Text;

                        ////Lets check whether we are on Purchase or Sales
                        //bool x = false;
                        //if (transactionType == "Purchase")
                        //{
                        //    //Increase the Product
                        //    x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                        //}
                        //else if (transactionType == "Sales")
                        //{
                        //    //Decrease the Product Quntiyt
                        //    x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                        //}

                        //Insert Transaction Details inside the database
                        bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                        success = w && y;
                    }

                }

                if (success == true)
                {
                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();

                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtTaxNumber.Text = "";
                    txtContact.Text = "";

                    txtSearchProduct.Text = "";
                    txtServiceName.Text = "";
                    txtDescription.Text = "0";
                    txtRate.Text = "0";
                    TxtQty.Text = "0";
                    txtSubTotal.Text = "0";
                    txtDiscount.Text = "0";
                    txtSGST.Text = "0";
                    txtCGST.Text = "0";
                    txtGrandTotal.Text = "0";
                    txtPaidAmount.Text = "0";
                    txtReturnAmount.Text = "0";
                    cmbExistingInvoice.DataSource = null;
                    cmbInvoiceType.SelectedItem = 1;
                    //int invoiceNumber = tDAL.GetNextInvoiceNo() + 1;
                    //txtInvoiceNumber.Text = DateTime.Now.Year.ToString() + "-" + invoiceNumber.ToString();
                    chkEditMode.Checked = false;
                    cmbInvoiceType.Text = "TAX-INVOICE";
                    frmreportBill Billingreport = new frmreportBill(transactionID);
                    Billingreport.Show();
                }
                else
                {
                    //Transaction Failed
                    MessageBox.Show("Transaction Failed");
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            
            
        }

        private void taxCalculation()
        {
            try
            {

                string value = txtDiscount.Text;
                if (value != "")
                {
                    //Get the discount in decimal value
                    decimal subTotal = decimal.Parse(txtSubTotal.Text);
                    decimal discount = decimal.Parse(txtDiscount.Text);

                    //Calculate the grandtotal based on discount
                    decimal grandTotal = ((100 - discount) / 100) * subTotal;

                    //Display the GrandTotla in TextBox
                    txtGrandTotal.Text = grandTotal.ToString();

                    decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                    decimal SGST = decimal.Parse(txtSGST.Text);
                    decimal CGST = decimal.Parse(txtCGST.Text);
                    sGSTValue = previousGT * (SGST / 100);
                    cGSTValue = previousGT * (CGST / 100);
                    decimal grandTotalWithVAT = previousGT + sGSTValue + cGSTValue;
                    txtGrandTotal.Text = grandTotalWithVAT.ToString();
                    txtPaidAmount.Text = grandTotalWithVAT.ToString();
                }
                else
                {
                    decimal subTotal = decimal.Parse(txtSubTotal.Text);
                    txtGrandTotal.Text = subTotal.ToString();
                    decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                    decimal SGST = decimal.Parse(txtSGST.Text);
                    decimal CGST = decimal.Parse(txtCGST.Text);
                    sGSTValue = previousGT * (SGST / 100);
                    cGSTValue = previousGT * (CGST / 100);
                    decimal grandTotalWithVAT = previousGT + sGSTValue + cGSTValue;
                    txtGrandTotal.Text = grandTotalWithVAT.ToString();
                    txtPaidAmount.Text = grandTotalWithVAT.ToString();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }

            
        
        }

       

        

        private void dgvAddedProducts_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
           try
            {

                int deleteAmount = int.Parse(dgvAddedProducts.Rows[e.Row.Index].Cells[3].Value.ToString());
                txtSubTotal.Text = (int.Parse(txtSubTotal.Text) - deleteAmount).ToString();
                taxCalculation();

            }
            catch(Exception ex)
            {


            }
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = new DataTable();
                //DateTime dtime = dtpselectInvoiceDate.Value;
                dt = tDAL.FetchInvoiceNo(dtpselectInvoiceDate.Value);
                if (dt.Rows.Count > 0)
                {
                    cmbExistingInvoice.DataSource = dt;
                    cmbExistingInvoice.ValueMember = "InvoiceNo";
                    cmbExistingInvoice.DisplayMember = "InvoiceNo";
                }

            }
            catch (Exception ex)
            { }
        }
        

        private void cmbExistingInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbExistingInvoice.Text != "")
                {
                    DataTable dt = new DataTable();
                    //DateTime dtime = dtpselectInvoiceDate.Value;
                    dt = tDAL.fetchExistingInvoice(cmbExistingInvoice.Text);

                    if (dt.Rows.Count > 0)
                    {
                        txtName.Text = dt.Rows[0]["p_Name"].ToString();
                        txtContact.Text = dt.Rows[0]["p_Contact"].ToString();
                        txtTaxNumber.Text = dt.Rows[0]["p_TaxNumber"].ToString();
                        txtInvoiceNumber.Text = dt.Rows[0]["InvoiceNo"].ToString();
                        txtSubTotal.Text = dt.Rows[0]["TotalAmt"].ToString();
                        txtDiscount.Text = dt.Rows[0]["discount"].ToString();
                        companyBLL cBLL = dcDAL.SearchBillingSetting();
                        txtCGST.Text = cBLL.CGST.ToString();
                        txtSGST.Text = cBLL.SGST.ToString();
                        taxCalculation();
                        txtGrandTotal.Text = dt.Rows[0]["grandTotal"].ToString();
                        txtPaidAmount.Text = dt.Rows[0]["grandTotal"].ToString();
                     
                        dtpBillDate.Value = Convert.ToDateTime(dt.Rows[0]["InvoiceDate"].ToString());
                        int trans_ID = int.Parse(dt.Rows[0]["ID"].ToString());
                        cmbInvoiceType.Text = dt.Rows[0]["trans_Type"].ToString();
                        transactionDT = tdDAL.fetchExistingInvoiceDetails(trans_ID);
                        dgvAddedProducts.DataSource = transactionDT;
                    }
                }

            }
            catch (Exception ex)
            { }
        }

        private void pnlDeaCust_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(!chkEditMode.Checked)
                {
                    int invoiceNumber = tDAL.GetNextInvoiceNo() + 1;
                    if (cmbInvoiceType.Text == "TAX-INVOICE")
                    {
                        txtInvoiceNumber.Text = "T" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + "-" + invoiceNumber.ToString();

                    }
                    else
                    {
                        txtInvoiceNumber.Text = "R" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + "-" + invoiceNumber.ToString();

                    }

                }
              
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        private void chkEditMode_CheckedChanged(object sender, EventArgs e)
        {
            if(chkEditMode.Checked)
            {
                existingPanel.Enabled = true;


            }
            else
            {
                existingPanel.Enabled = false;

            }
        }
    }
}
