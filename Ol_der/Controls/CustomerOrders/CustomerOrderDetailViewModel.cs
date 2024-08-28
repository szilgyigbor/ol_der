using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.CustomerOrders
{
    class CustomerOrderDetailViewModel : INotifyPropertyChanged
    {
        private CustomerOrderRepository _customerOrderRepository;
        private CustomerOrder _customerOrder;
        public CustomerOrder CustomerOrder
        {
            get { return _customerOrder; }
            set
            {
                _customerOrder = value;

                var sortedStatuses = new ObservableCollection<CustomerOrderStatus>(_customerOrder.CustomerOrderStatuses.OrderByDescending(s => s.StatusDate));
                _customerOrder.CustomerOrderStatuses = sortedStatuses;

                OnPropertyChanged(nameof(CustomerOrder));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CustomerOrderDetailViewModel(CustomerOrder customerOrderDetails)
        {
            _customerOrderRepository = new CustomerOrderRepository();
            CustomerOrder = customerOrderDetails;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
