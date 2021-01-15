using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyStore.BLL
{
    class DeaCustBLL
    {
        public int p_ID { get; set; }
        public string p_Name { get; set; }
        public string p_Address1 { get; set; }
        public string p_Address2 { get; set; }
        public string p_TaxNumber { get; set; }
        public string p_State { get; set; }
        public string p_Contact { get; set; }
        public int p_AddedBy { get; set; }
        public string p_Datetime { get; set; }

        public int p_StateCode { get; set; }
    }
}
