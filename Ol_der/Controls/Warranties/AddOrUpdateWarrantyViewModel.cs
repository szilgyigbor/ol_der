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
using Ol_der.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Ol_der.Controls.Warranties
{
    internal class AddOrUpdateWarrantyViewModel : INotifyPropertyChanged
    {
        private WarrantyRepository _warrantyRepository;
        private string _dateString;
        private string _productDescription;
        private string _statusContent;
        private string _itemNumber;
        private Warranty _warranty;
        private bool _isUpdate;
        private WarrantyStatus _selectedWarrantyStatus;

        public Action OnWarrantyFinished;
        public string DateString
        {
            get { return _dateString; }
            set
            {
                _dateString = value;
                OnPropertyChanged(nameof(DateString));
            }
        }

        public string ProductDescription
        {
            get { return _productDescription; }
            set
            {
                _productDescription = value;
                OnPropertyChanged(nameof(ProductDescription));
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

        public string ItemNumber
        {
            get { return _itemNumber; }
            set
            {
                _itemNumber = value;
                OnPropertyChanged(nameof(ItemNumber));
            }
        }

        public Warranty Warranty
        {
            get { return _warranty; }
            set
            {
                _warranty = value;

                var sortedStatuses = new ObservableCollection<WarrantyStatus>(_warranty.WarrantyStatuses.OrderByDescending(s => s.StatusDate));
                _warranty.WarrantyStatuses = sortedStatuses;

                OnPropertyChanged(nameof(Warranty));
            }
        }

        public WarrantyStatus SelectedWarrantyStatus
        {
            get { return _selectedWarrantyStatus; }
            set
            {
                _selectedWarrantyStatus = value;
                StatusContent = _selectedWarrantyStatus.StatusDescription;
                OnPropertyChanged(nameof(SelectedWarrantyStatus));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddProductCommand { get; }
        public ICommand RemoveProductCommand { get; }
        public ICommand AddWarrantyStatusCommand { get; }
        public ICommand SaveWarrantyCommand { get; }
        public ICommand RemoveWarrantyStatusCommand { get; }
        public ICommand UpdateWarrantyStatusCommand { get; }


        public AddOrUpdateWarrantyViewModel(Warranty warranty)
        {
            _warrantyRepository = new();
            CheckWarranty(warranty);
            AddProductCommand = new RelayCommand(param => SearchProductbyItemNumberAsync());
            AddWarrantyStatusCommand = new RelayCommand(param => AddWarrantyStatus());
            SaveWarrantyCommand = new RelayCommand(param => SaveOrUpdateWarrantyAsync());
            RemoveWarrantyStatusCommand = new RelayCommand(param => RemoveWarrantyStatus());
            UpdateWarrantyStatusCommand = new RelayCommand(param => UpdateWarrantyStatus());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task CheckWarranty(Warranty warranty)
        {
            if (warranty == null)
            {
                Warranty = new Warranty();
                Warranty.CreationDate = DateTime.Now;
                DateString = Warranty.CreationDate.ToString("yyyy-MM-dd HH:mm");
                _isUpdate = false;
            }
            else
            {
                Warranty = warranty;
                DateString = Warranty.CreationDate.ToString("yyyy-MM-dd HH:mm");
                ProductDescription = Warranty.Product.ItemNumber + "  " + Warranty.Product.Name;
                _isUpdate = true;
            }
        }

        public async Task SaveOrUpdateWarrantyAsync()
        {
            if (_isUpdate)
            {
                await UpdateWarrantyAsync();
            }
            else
            {
                await SaveWarrantyAsync();
            }
        }

        public async Task UpdateWarrantyAsync()
        {
            if (Warranty.IsCompleted)
            {
                Warranty.FulfilledDate = DateTime.Now;
            }

            await _warrantyRepository.UpdateWarrantyAsync(Warranty);
            MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Sikeresen frissítetted a garanciát!");
            messageBoxOkWindow.ShowDialog();
            OnWarrantyFinished?.Invoke();
        }

        public async Task SaveWarrantyAsync()
        {
            await _warrantyRepository.SaveWarrantyAsync(Warranty);
            MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Sikeresen mentetted a garanciát!");
            messageBoxOkWindow.ShowDialog();
            OnWarrantyFinished?.Invoke();
        }

        public async Task SearchProductbyItemNumberAsync()
        {
            Product product = await _warrantyRepository.SearchProductByItemNumberAsync(ItemNumber);

            if (product != null)
            {
                ProductDescription = product.ItemNumber + "  " + product.Name;
                Warranty.ProductId = product.ProductId;
                Warranty.SupplierId = product.SupplierId;
                ItemNumber = "";
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Nem található ilyen termék!");
                messageBoxOkWindow.ShowDialog();
            }
        }

        public async Task AddWarrantyStatus()
        {
            MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan hozzá akarod adni a státuszt?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult == false)
            {
                return;
            }

            if (Warranty.ProductId == 0)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Nem választottál terméket!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            if (string.IsNullOrEmpty(StatusContent))
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Nem írtál semmit a státuszhoz!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            var newStatus = new WarrantyStatus
            {
                StatusDescription = StatusContent,
                StatusDate = DateTime.Now
            };

            Warranty.WarrantyStatuses.Add(newStatus);

            StatusContent = "";
        }

        public async Task RemoveWarrantyStatus()
        {
            if (SelectedWarrantyStatus == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Előbb válassz ki egy státuszt!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan törölni akarod a kiválasztott státuszt?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult == false)
            {
                return;
            }

            if (_isUpdate && SelectedWarrantyStatus.WarrantyStatusId != 0)
            {
                _warrantyRepository.RemoveWarrantyStatusAsync(SelectedWarrantyStatus);
            }
            Warranty.WarrantyStatuses.Remove(SelectedWarrantyStatus);

            StatusContent = "";
        }


        public async Task UpdateWarrantyStatus()
        {
            MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan frissíteni akarod a státuszt?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult == false)
            {
                return;
            }

            SelectedWarrantyStatus.StatusDescription = StatusContent;

            if (_isUpdate && SelectedWarrantyStatus.WarrantyStatusId != 0)
            {
                await _warrantyRepository.UpdateWarrantyStatusAsync(SelectedWarrantyStatus);

                Warranty = await _warrantyRepository.GetWarrantyByIdAsync(Warranty.WarrantyId);
            }

            StatusContent = "";

        }
    }
}
