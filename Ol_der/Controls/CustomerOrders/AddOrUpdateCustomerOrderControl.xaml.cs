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

namespace Ol_der.Controls.CustomerOrders
{
    /// <summary>
    /// Interaction logic for AddOrUpdateCustomerOrderControl.xaml
    /// </summary>
    public partial class AddOrUpdateCustomerOrderControl : UserControl
    {
        private AddOrUpdateCustomerOrderViewModel _viewModel;
        public Action OnCustomerOrderFinished;
        public AddOrUpdateCustomerOrderControl(CustomerOrder customerOrderToUpdate = null)
        {
            InitializeComponent();
            _viewModel = new(customerOrderToUpdate);
            this.DataContext = _viewModel;
            _viewModel.OnCustomerOrderFinished -= ActivateEvent;
            _viewModel.OnCustomerOrderFinished += ActivateEvent;
        }

        public void ActivateEvent()
        {
            OnCustomerOrderFinished?.Invoke();
        }
    }
}
