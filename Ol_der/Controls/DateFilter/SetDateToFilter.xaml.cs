using Ol_der.Controls.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ol_der.Controls.DateFilter
{
    /// <summary>
    /// Interaction logic for SetDateToFilter.xaml
    /// </summary>
    public partial class SetDateToFilter : Window
    {
        private DateTime? StartDate { get; set; }
        private DateTime? EndDate { get; set; }

        public List<DateTime> dateTimes { get; private set; }

        public SetDateToFilter()
        {
            InitializeComponent();
            dateTimes = new List<DateTime>();
        }

        private void StartDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StartDate = StartDatePicker.SelectedDate;
            UpdateSelectedRange();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            EndDate = EndDatePicker.SelectedDate;
            UpdateSelectedRange();
        }

        private void UpdateSelectedRange()
        {
            if (StartDate.HasValue && EndDate.HasValue)
            {
                SelectedRangeTextBlock.Text = $"Kezdő: {StartDate.Value.ToShortDateString()}, Vég: {EndDate.Value.ToShortDateString()}";
            }
            else
            {
                SelectedRangeTextBlock.Text = "Nincs teljes intervallum kiválasztva.";
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDate == null || EndDate == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Kell kezdő és vég dátum is");
                messageBoxOkWindow.ShowDialog();
            }

            else 
            {
                dateTimes.Clear();
                dateTimes.Add(StartDate.Value);
                dateTimes.Add(EndDate.Value);
                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
