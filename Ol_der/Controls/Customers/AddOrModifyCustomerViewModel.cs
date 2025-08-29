using Ol_der.Controls.Orders;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Customers
{
    public class AddOrModifyCustomerViewModel : INotifyPropertyChanged
    {
        private int customerId;
        private string _buttonText = "Hozzáadás";
        private string _name = string.Empty;
        private string _address = string.Empty;
        private string _phone = string.Empty;
        private string _email = string.Empty;
        private string _taxNumber = string.Empty;
        private string _notes = string.Empty;
        private bool _isBusy = false;

        private readonly CustomerRepository _repository = new CustomerRepository();

        public Action OnFinished { get; set; } = () => { };

        public RelayCommand AddCommand { get; }

        public AddOrModifyCustomerViewModel(int customerId)
        {
            AddCommand = new RelayCommand(async _ => await ExecuteAddAsync(), _ => CanAdd());
            this.customerId = customerId;
            if (customerId > 0)
            {
                LoadCustomerData(customerId);
            }
        }

        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    AddCommand.RaiseCanExecuteChanged(); // if name changes, re-evaluate CanAdd
                }
            }
        }

        public string Address { get => _address; set => SetProperty(ref _address, value); }
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string TaxNumber { get => _taxNumber; set => SetProperty(ref _taxNumber, value); }
        public string Notes { get => _notes; set => SetProperty(ref _notes, value); }

        public bool IsBusy { get => _isBusy; private set { if (SetProperty(ref _isBusy, value)) AddCommand.RaiseCanExecuteChanged(); } }

        private bool CanAdd() => !IsBusy && !string.IsNullOrWhiteSpace(Name);

        private async Task ExecuteAddAsync()
        {
            if (customerId > 0)
            {
                await UpdateCustomerAsync();
                OnFinished();
            }
            else
            {
                await AddCustomerAsync();
                OnFinished();
            }
        }

        private async Task AddCustomerAsync()
        {
            try
            {
                IsBusy = true;

                var newCustomer = new Customer
                {
                    Name = this.Name.Trim(),
                    Address = this.Address?.Trim() ?? string.Empty,
                    Phone = this.Phone?.Trim() ?? string.Empty,
                    Email = this.Email?.Trim() ?? string.Empty,
                    TaxNumber = this.TaxNumber?.Trim() ?? string.Empty,
                    Notes = this.Notes?.Trim() ?? string.Empty
                };

                await _repository.AddCustomerAsync(newCustomer);

                // successfuly saved, clear fields
                ClearFields();
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Az ügyfél sikeresen hozzáadva.");
                messageBoxOkWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // can logs here
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow($"Hiba történt: {ex.Message}");
                messageBoxOkWindow.ShowDialog();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdateCustomerAsync()
        {
            try
            {
                IsBusy = true;
                var modifiedDatas = new Customer
                {
                    Name = this.Name.Trim(),
                    Address = this.Address?.Trim() ?? string.Empty,
                    Phone = this.Phone?.Trim() ?? string.Empty,
                    Email = this.Email?.Trim() ?? string.Empty,
                    TaxNumber = this.TaxNumber?.Trim() ?? string.Empty,
                    Notes = this.Notes?.Trim() ?? string.Empty
                };
                await _repository.UpdateCustomerAsync(modifiedDatas, customerId);

                // successfuly updated, clear fields
                ClearFields();
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Az ügyfél sikeresen módosítva.");
                messageBoxOkWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // can logs here
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow($"Hiba történt: {ex.Message}");
                messageBoxOkWindow.ShowDialog();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ClearFields()
        {
            Name = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            TaxNumber = string.Empty;
            Notes = string.Empty;
        }

        private async void LoadCustomerData(int customerId)
        {
            var customer = await _repository.GetCustomerByIdAsync(customerId);
            if (customer != null)
            {
                Name = customer.Name;
                Address = customer.Address;
                Phone = customer.Phone;
                Email = customer.Email;
                TaxNumber = customer.TaxNumber;
                Notes = customer.Notes;
                ButtonText = "Módosítás";
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value!;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion
    }
}
