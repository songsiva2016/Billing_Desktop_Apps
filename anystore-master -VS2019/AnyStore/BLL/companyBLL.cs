using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyStore.BLL
{
    public class companyBLL
    {
        public int id { get; set; }
        public string Company_Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string GST_Num { get; set; }
        public string State { get; set; }
        public string PanNo { get; set; }
        public string Contact { get; set; }
        public int SGST { get; set; }
        public int CGST { get; set; }
        public int StateCode { get; set; }
    }
}
