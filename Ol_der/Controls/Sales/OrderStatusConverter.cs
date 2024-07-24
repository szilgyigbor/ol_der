using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ol_der.Controls.Sales
{
    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SaleItem saleItem)
            {
                if (!saleItem.NeedToOrder && !saleItem.IsOrdered)
                {
                    return "(Nem lesz rendelve)";
                }
                else if (!saleItem.NeedToOrder && saleItem.IsOrdered)
                {
                    return "(Rendelésben van)";
                }
                else
                {
                    return saleItem.IsOrdered ? "(Rendelésben van)" : "(Még nincs rendelve)";
                }
            }
            return "Ismeretlen állapot";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
