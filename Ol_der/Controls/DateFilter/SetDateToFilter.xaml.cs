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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public SetDateToFilter()
        {
            InitializeComponent();
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
    }
}
