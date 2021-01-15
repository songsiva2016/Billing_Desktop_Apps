using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyStore.BLL
{
    class productsBLL
    {
        //Getters and Setters for Product Module
        public int id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }

        public int SAC_Code { get; set; }
        public DateTime Added_Date { get; set; }
    }
}
