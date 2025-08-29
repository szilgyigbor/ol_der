using DocumentFormat.OpenXml.Bibliography;
using Ol_der.Controls.DetailedSearch;
using Ol_der.Controls.Orders;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ol_der.Controls.CustomerSearch
{
    internal class CustomerSearchWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private CustomerSearchWindowRepository _repository;
        private string _customerName;
        private List<Customer> _customers;
        private Customer _selectedCustomer;

        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                OnPropertyChanged();
            }
        }
        public List<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }


        public ICommand SearchCustomersByNameCommand { get; }

        public ICommand SelectSearchedCustomerCommand { get; }

        
        public CustomerSearchWindowViewModel()
        {
            _repository = new CustomerSearchWindowRepository();
            SearchCustomersByNameCommand = new RelayCommand(async _ => await SearchCustomersByNameAsync());
            SetAllCustomers();
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SearchCustomersByNameAsync()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                return;
            }

            List<Customer> customers = await _repository.SearchCustomersByNameAsync(CustomerName);

            Customers = customers.ToList();

            CustomerName = string.Empty;

            if (Customers.Count == 0)
            {
                NoResult();
            }

        }

        public void NoResult()
        {
            MessageBoxOkWindow messageBox = new MessageBoxOkWindow("Nincs ilyen ügyfél");
            messageBox.ShowDialog();
        }

        public async void SetAllCustomers()
        {
            Customers = await _repository.GetAllCustomersAsync();
        }
    }
}
