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

namespace Ol_der.Controls.Customers
{
    /// <summary>
    /// Interaction logic for AddOrModifyCustomerControl.xaml
    /// </summary>
    public partial class AddOrModifyCustomerControl : UserControl
    {
        private AddOrModifyCustomerViewModel _viewModel;
        public Action OnFinished = () => { };
        public AddOrModifyCustomerControl(int customerId = -1)
        {
            InitializeComponent();
            _viewModel = new AddOrModifyCustomerViewModel(customerId);
            this.DataContext = _viewModel;
            _viewModel.OnFinished -= ActivateEvent;
            _viewModel.OnFinished += ActivateEvent;
        }

        public void ActivateEvent()
        {
            OnFinished();
        }
    }
}
