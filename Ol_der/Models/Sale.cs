using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public DateTime Date { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? CardTransactionAmount { get; set; }
        public bool IsCardTransactionProcessed { get; set; }
        public string CustomerName { get; set; }
        public string Notes { get; set; }
    }
}
