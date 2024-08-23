using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Models
{
    public class WarrantyStatus
    {
        public int WarrantyStatusId { get; set; }
        public int WarrantyId { get; set; }
        public Warranty Warranty { get; set; }
        public string StatusDescription { get; set; } = string.Empty;
        public DateTime StatusDate { get; set; } = DateTime.Now;
    }
}
