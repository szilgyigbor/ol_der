using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Warranties
{
    class WarrantyDetailsViewModel : INotifyPropertyChanged
    {
        private WarrantyRepository _warrantyRepository;
        private Warranty _warranty;

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

        public event PropertyChangedEventHandler PropertyChanged;

        public WarrantyDetailsViewModel(Warranty warrantyDetails)
        {
            _warrantyRepository = new();
            Warranty = warrantyDetails;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
