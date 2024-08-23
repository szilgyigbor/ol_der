using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ol_der.Models;

namespace Ol_der.Controls.Warranties
{
    internal class ShowAllWarrantyViewModel : INotifyPropertyChanged
    {
        private WarrantyRepository _warrantyRepository;

        private int _limit;

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Warranty> _warranties;
        public ObservableCollection<Warranty> Warranties
        {
            get { return _warranties; }
            set
            {
                _warranties = value;
                OnPropertyChanged(nameof(Warranties));
            }
        }
        
        public ShowAllWarrantyViewModel(int limit)
        {
            _warrantyRepository = new();
            _limit = limit;
            LoadWarrantiesAsync(_limit);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadWarrantiesAsync(int limit)
        {
            var warranties = await _warrantyRepository.GetLimitedNumberOfWarrantyAsync(limit);
            Warranties = new ObservableCollection<Warranty>(warranties);
        }
    }
}
