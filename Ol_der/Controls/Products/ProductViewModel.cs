using Microsoft.EntityFrameworkCore;
using Ol_der.Controls.Sales;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Products
{
    internal class ProductViewModel : INotifyPropertyChanged
    {
        private int _productCount;
        private readonly ProductRepository productRepository;
        public int ProductCount
        {
            get => _productCount;
            set
            {
                if (_productCount != value)
                {
                    _productCount = value;
                    OnPropertyChanged(nameof(ProductCount));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public ProductViewModel() 
        {
            productRepository = new ProductRepository();
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            return await productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await productRepository.UpdateProductAsync(product);
        }

        public async Task<List<Product>> SearchProductByItemNumberAsync(string itemNumber)
        {
            return await productRepository.SearchProductByItemNumberAsync(itemNumber);
        }

        public async Task DeleteProductAsync(Product product)
        {
            await productRepository.DeleteProductAsync(product);
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            return await productRepository.GetAllProductAsync();
        }

        public async Task<List<Supplier>> GetAllSupplierAsync()
        {
            return await productRepository.GetAllSupplierAsync();
        }

        public async Task<int> GetProductCountAsync()
        {
            return await productRepository.GetProductCountAsync();
        }
    }
}
