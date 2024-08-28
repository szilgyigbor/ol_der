using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Models
{
    public class CustomerOrderStatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerOrderStatusId { get; set; }
        public int CustomerOrderId { get; set; }
        public CustomerOrder CustomerOrder { get; set; }
        public string StatusDescription { get; set; } = string.Empty;
        public DateTime StatusDate { get; set; } = DateTime.Now;
    }
}
