using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        public string ItemNumber { get; set; }
        public string Name { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
