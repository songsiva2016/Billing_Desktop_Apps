using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyStore.BLL
{
    class transactionsBLL
    {
        public int id { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public int cust_id { get; set; }
        public decimal grandTotal { get; set; }
        public string transaction_date { get; set; }
        public decimal sGST { get; set; }
        public decimal cGST { get; set; }
        public decimal discount { get; set; }
        public int added_by { get; set; }
        public string Invoice_Type { get; set; }


    }
}
