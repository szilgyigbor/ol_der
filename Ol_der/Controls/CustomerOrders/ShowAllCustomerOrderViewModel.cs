using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.CustomerOrders
{
    public class ShowAllCustomerOrderViewModel : INotifyPropertyChanged
    {
        private CustomerOrderRepository _customerOrderRepository;

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<CustomerOrder> _customerOrders;
        public ObservableCollection<CustomerOrder> CustomerOrders
        {
            get { return _customerOrders; }
            set
            {
                _customerOrders = value;
                OnPropertyChanged(nameof(CustomerOrders));
            }
        }

        public ShowAllCustomerOrderViewModel(int limit)
        {
            _customerOrderRepository = new CustomerOrderRepository();
            LoadCustomerOrdersAsync(limit);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadCustomerOrdersAsync(int limit)
        {
            var customerOrders = await _customerOrderRepository.GetLimitedNumberOfCustomerOrderAsync(limit);
            CustomerOrders = new ObservableCollection<CustomerOrder>(customerOrders);
        }
    }
}
