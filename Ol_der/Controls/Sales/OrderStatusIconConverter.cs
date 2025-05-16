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
    public class OrderStatusIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SaleItem saleItem)
            {
                if (!saleItem.NeedToOrder && !saleItem.IsOrdered)
                    return "/Icons/rejected.png";
                else if (!saleItem.NeedToOrder && saleItem.IsOrdered)
                    return "/Icons/approved.png";
                else
                    return saleItem.IsOrdered ? "/Icons/approved.png" : "/Icons/time.png";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
