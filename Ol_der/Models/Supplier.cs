using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Models
{
    class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
