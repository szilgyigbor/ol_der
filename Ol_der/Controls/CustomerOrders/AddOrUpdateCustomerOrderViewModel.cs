using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Primitives;
using Ol_der.Controls.Orders;
using Ol_der.Controls.Warranties;
using System.Globalization;

namespace Ol_der.Controls.CustomerOrders
{
    internal class AddOrUpdateCustomerOrderViewModel : INotifyPropertyChanged
    {
        private CustomerOrderRepository _customerOrderRepository;
        private string _dateString;
        private string _statusContent;
        private bool _isUpdate;
        private CustomerOrder _customerOrder;
        private CustomerOrderStatus _selectedCustomerOrderStatus;

        public Action OnCustomerOrderFinished;
        public string DateString
        {
            get { return _dateString; }
            set
            {
                _dateString = value;
                OnPropertyChanged(nameof(DateString));
            }
        }

        public string StatusContent
        {
            get { return _statusContent; }
            set
            {
                _statusContent = value;
                OnPropertyChanged(nameof(StatusContent));
            }
        }

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

        public CustomerOrderStatus SelectedCustomerOrderStatus
        {
            get { return _selectedCustomerOrderStatus; }
            set
            {
                _selectedCustomerOrderStatus = value;
                StatusContent = _selectedCustomerOrderStatus?.StatusDescription;
                OnPropertyChanged(nameof(SelectedCustomerOrderStatus));
            }
        }

        public ICommand AddCustomerOrderStatusCommand { get; }
        public ICommand SaveCustomerOrderCommand { get; }
        public ICommand RemoveCustomerOrderStatusCommand { get; }
        public ICommand UpdateCustomerOrderStatusCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddOrUpdateCustomerOrderViewModel(CustomerOrder customerOrder)
        {
            _customerOrderRepository = new();
            CheckCustomerOrder(customerOrder);
            AddCustomerOrderStatusCommand = new RelayCommand(param => AddCustomerOrderStatus());
            SaveCustomerOrderCommand = new RelayCommand(param => SaveOrUpdateCustomerOrderAsync());
            RemoveCustomerOrderStatusCommand = new RelayCommand(param => DeleteCustomerOrderStatus());
            UpdateCustomerOrderStatusCommand = new RelayCommand(param => UpdateCustomerOrderStatus());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task CheckCustomerOrder(CustomerOrder customerOrder)
        {
            if (customerOrder == null)
            {
                CustomerOrder = new CustomerOrder();
                CustomerOrder.CreationDate = DateTime.Now;
                DateString = CustomerOrder.CreationDate.ToString("yyyy-MM-dd HH:mm");
                _isUpdate = false;
            }
            else
            {
                CustomerOrder = customerOrder;
                DateString = CustomerOrder.CreationDate.ToString("yyyy-MM-dd HH:mm");
                _isUpdate = true;
            }
        }

        public async Task SaveOrUpdateCustomerOrderAsync()
        {
            if (_isUpdate)
            {
                await UpdateCustomerOrderAsync();
            }
            else
            {
                await SaveCustomerOrderAsync();
            }
        }

        public bool UpdateDate() 
        {
            if (DateTime.TryParseExact(DateString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
            {
                CustomerOrder.CreationDate = newDate;
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("A dátum formátuma nem megfelelő. Kérlek, használd a következő formátumot: yyyy-MM-dd HH:mm");
                messageBoxOkWindow.ShowDialog();
                return false;
            }

            return true;
        }

        public async Task UpdateCustomerOrderAsync()
        {
            bool dateSuccess = UpdateDate();

            if (!dateSuccess)
            {
                return;
            }

            if (CustomerOrder.IsCompleted)
            {
                CustomerOrder.FulfilledDate = DateTime.Now;
            }

            await _customerOrderRepository.UpdateCustomerOrderAsync(CustomerOrder);
            MessageBoxOkWindow messageBoxOkWindow1 = new MessageBoxOkWindow("Sikeresen frissítetted az ügyfélrendelést!");
            messageBoxOkWindow1.ShowDialog();
            OnCustomerOrderFinished?.Invoke();
        }

        public async Task SaveCustomerOrderAsync()
        {
            bool dateSuccess = UpdateDate();

            if (!dateSuccess)
            {
                return;
            }

            await _customerOrderRepository.UpdateCustomerOrderAsync(CustomerOrder);
            MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Sikeresen mentetted az ügyfélrendelést!");
            messageBoxOkWindow.ShowDialog();
            OnCustomerOrderFinished?.Invoke();
        }

        public async Task AddCustomerOrderStatus()
        {
            MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan hozzá akarod adni a státuszt?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(StatusContent))
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Nem írtál semmit a státuszhoz!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            var newStatus = new CustomerOrderStatus
            {
                StatusDescription = StatusContent,
                StatusDate = DateTime.Now
            };

            CustomerOrder.CustomerOrderStatuses.Add(newStatus);
            await _customerOrderRepository.UpdateCustomerOrderAsync(CustomerOrder);
            CustomerOrder = await _customerOrderRepository.GetCustomerOrderByIdAsync(CustomerOrder.CustomerOrderId);
            StatusContent = "";
        }

        public async Task DeleteCustomerOrderStatus()
        {
            if (SelectedCustomerOrderStatus == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Nem választottál státuszt!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan törölni akarod a státuszt?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult == false)
            {
                return;
            }

            CustomerOrder.CustomerOrderStatuses.Remove(SelectedCustomerOrderStatus);
            await _customerOrderRepository.UpdateCustomerOrderAsync(CustomerOrder);
            CustomerOrder = await _customerOrderRepository.GetCustomerOrderByIdAsync(CustomerOrder.CustomerOrderId);
        }

        public async Task UpdateCustomerOrderStatus()
        {
            if (SelectedCustomerOrderStatus == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Előbb válassz ki egy státuszt!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan frissíteni akarod a státuszt?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult == false)
            {
                return;
            }

            SelectedCustomerOrderStatus.StatusDescription = StatusContent;

            await _customerOrderRepository.UpdateCustomerOrderStatusAsync(SelectedCustomerOrderStatus);
            CustomerOrder = await _customerOrderRepository.GetCustomerOrderByIdAsync(CustomerOrder.CustomerOrderId);

            StatusContent = "";
        }
    }
}
