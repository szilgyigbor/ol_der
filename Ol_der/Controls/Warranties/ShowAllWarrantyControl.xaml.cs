using Ol_der.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ol_der.Controls.Warranties
{
    /// <summary>
    /// Interaction logic for ShowAllWarrantyControl.xaml
    /// </summary>
    public partial class ShowAllWarrantyControl : UserControl
    {
        private ShowAllWarrantyViewModel _viewModel;
        public ShowAllWarrantyControl(int limit)
        {
            InitializeComponent();
            _viewModel = new(limit);
            this.DataContext = _viewModel;
        }

        public Warranty GetSelectedWarranty()
        {
            if (WarrantiesListView.SelectedItem is Warranty SelectedWarranty)
            {
                return SelectedWarranty;
            }
            return null;
        }
    }
}
