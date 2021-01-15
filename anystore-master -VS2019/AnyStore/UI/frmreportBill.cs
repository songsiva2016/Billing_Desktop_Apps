using AnyStore.BLL;
using AnyStore.DAL;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmreportBill : Form
    {
        reportDAL reportDAL = new reportDAL();
        companyInfoDAL companyInfoDAL = new companyInfoDAL();
        companyBLL companyBLLData = new companyBLL();
        int TransID = 0;
        public frmreportBill(int transID)
        {
            InitializeComponent();
            TransID = transID;
        }

        private void frmreportBill_Load(object sender, EventArgs e)
        {

            try
            {
                DataSet1 dataSet1 = new DataSet1();

                companyBLLData = companyInfoDAL.GetCompanyData();
                DataTable dataTable = reportDAL.SelectData(TransID);
                string transID = dataTable.Rows[0]["trans_Type"].ToString();
                

                DataRow dr = dataTable.NewRow();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    dataTable.Rows[i]["Companyname"] = companyBLLData.Company_Name;
                    dataTable.Rows[i]["CompanyTaxNumber"] = companyBLLData.GST_Num;
                    dataTable.Rows[i]["c_Address"] = companyBLLData.Address1;
                    dataTable.Rows[i]["c_PanNo"] = companyBLLData.PanNo;
                    dataTable.Rows[i]["c_SGST"] = companyBLLData.SGST;
                    dataTable.Rows[i]["c_CGST"] = companyBLLData.CGST;
                    dataTable.Rows[i]["State_Code"] = companyBLLData.StateCode;
                    if (transID == "TAX-INVOICE")
                    {
                        dataTable.Rows[i]["ReportHeadName"] = "Tax Invoice";
                    }
                    else
                    {
                        dataTable.Rows[i]["ReportHeadName"] = "REIMBURSEMENT OF EXPENSE";
                    }

                }
                ReportDocument rd = new ReportDocument();
                rd.Load(Application.StartupPath + "\\Billing.rpt");
                rd.SetDataSource(dataTable);
                crystalReportViewer1.ReportSource = rd;




            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
