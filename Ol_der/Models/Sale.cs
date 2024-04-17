using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Models
{
    class Sale
    {
        public int SaleId { get; set; }
        public string ItemNumber { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public bool IsOrdered { get; set; }
    }
}
