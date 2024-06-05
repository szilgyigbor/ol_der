﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Models
{
    public class SaleItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleItemId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public int Quantity { get; set; }
        public bool NeedToOrder { get; set; } = true;
        public bool IsOrdered { get; set; }
        public decimal Price { get; set; }
    }
}
