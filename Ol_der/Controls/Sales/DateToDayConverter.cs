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
    public class DateToDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CollectionViewGroup group)
            {
                DateTime date = (DateTime)group.Name;
                CultureInfo huCulture = new CultureInfo("hu-HU");
                decimal totalCash = group.Items.OfType<Sale>().Where(s => s.PaymentType == PaymentType.Cash).Sum(s => s.TotalAmount);
                decimal totalCard = group.Items.OfType<Sale>().Where(s => s.PaymentType == PaymentType.Card).Sum(s => s.TotalAmount);
                decimal totalRN = group.Items.OfType<Sale>().Where(s => s.PaymentType == PaymentType.RN).Sum(s => s.TotalAmount);
                decimal totalTransfer = group.Items.OfType<Sale>().Where(s => s.PaymentType == PaymentType.Transfer).Sum(s => s.TotalAmount);

                string formattedDate = date.ToString("yyyy-MM-dd, dddd", huCulture);
                return $"{formattedDate}:  (Cash: {totalCash:C0},   Card: {totalCard:C0},   RN: {totalRN:C0},   Transfer: {totalTransfer:C0}   Teljes bevétel: {totalCash + totalCard + totalRN + totalTransfer:C0})";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
