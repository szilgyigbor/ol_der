using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ol_der.Controls.SalePackages
{
    public class DateToDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CollectionViewGroup group)
            {
                DateTime date = (DateTime)group.Name;
                CultureInfo huCulture = new CultureInfo("hu-HU");
                decimal totalTransfer = group.Items.OfType<Sale>()
                        .Where(s => s.PaymentType == PaymentType.Transfer && s.IsPackage)
                        .Sum(s => s.TotalAmount);
                string formattedDate = date.ToString("yyyy-MM-dd, dddd", huCulture);
                return $"{formattedDate}:  (Napi össz csomag érték: {totalTransfer:0}.-Ft)";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
