using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Models
{
    public enum PaymentType
    {
        [Display(Name = "Készpénz")]
        Cash,

        [Display(Name = "Kártya")]
        Card,

        [Display(Name = "Átutalás")]
        Transfer,

        [Display(Name = "RN")]
        RN,

        [Display(Name = "Bizomány")]
        Loan
    }
}
