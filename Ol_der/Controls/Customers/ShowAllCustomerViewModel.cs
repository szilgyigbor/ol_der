using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Customers
{
    class ShowAllCustomerViewModel : INotifyPropertyChanged
    {
        private readonly CustomerRepository _repository = new CustomerRepository();

        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ShowAllCustomerViewModel()
        {
            RefreshData();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void RefreshData()
        {
            var customerList = await _repository.GetAllCustomersAsync();
            Customers = new ObservableCollection<Customer>(customerList);
        }
    }
}
