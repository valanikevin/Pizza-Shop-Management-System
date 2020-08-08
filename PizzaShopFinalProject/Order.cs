using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShopFinalProject
{
    class Order
    {
        public int oID { get; set; }
        public int uID { get; set; }
        public DateTime oDate { get; set; }
        public double oTotal { get; set; }
        public string pSize { get; set; }
        public string pCheese { get; set; }
        public string pTop1 { get; set; }
        public string pTop2 { get; set; }
        public string pTop3 { get; set; }
        public string pTop4 { get; set; }
        public string pComboNote { get; set; }

    }
}
