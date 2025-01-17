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

namespace Ol_der.Controls.DetailedSearch
{
    class SearchWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SearchWindowRepository _repository;
        private string _itemNumber;
        private string _productName;
        private List<Product> _products;
        private Product _selectedProduct;

        public string ItemNumber
        {
            get { return _itemNumber; }
            set
            {
                _itemNumber = value;
                OnPropertyChanged();
            }
        }
        public string ProductName 
        {
            get { return _productName; }
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public List<Product> Products
        { 
            get { return _products; }
            set 
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchProductsByItemNumberCommand { get; }
        public ICommand SearchProductsByNameCommand { get; }

        public ICommand SelectSearchedProductCommand { get; }


        public SearchWindowViewModel() 
        {
            _repository = new SearchWindowRepository();
            SearchProductsByItemNumberCommand = new RelayCommand(async param => await SearchProductsByItemNumber());
            SearchProductsByNameCommand = new RelayCommand(async param => await SearchProductsByName());
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SearchProductsByItemNumber()
        {
            if (string.IsNullOrWhiteSpace(ItemNumber))
            {
                return;
            }

            List<Product> products = await _repository.SearchProductsByItemNumberAsync(ItemNumber);

            Products = products.ToList();

            ItemNumber = string.Empty;
            
        }

        public async Task SearchProductsByName()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                return;
            }

            List<Product> products = await _repository.SearchProductsByNameAsync(ProductName);

            Products = products.ToList();

            ProductName = string.Empty;

        }


    }
}
