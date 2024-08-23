using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ol_der.Controls.Warranties
{
    /// <summary>
    /// Interaction logic for AddOrUpdateWarrantyControl.xaml
    /// </summary>
    public partial class AddOrUpdateWarrantyControl : UserControl
    {
        private AddOrUpdateWarrantyViewModel _viewModel;
        public Action OnWarrantyFinished;
        public AddOrUpdateWarrantyControl(Warranty warrantyToUpdate = null)
        {
            InitializeComponent();
            _viewModel = new(warrantyToUpdate);
            this.DataContext = _viewModel;
            _viewModel.OnWarrantyFinished -= ActivateEvent;
            _viewModel.OnWarrantyFinished += ActivateEvent;
        }

        public void ActivateEvent()
        {
            OnWarrantyFinished?.Invoke();
        }
    }
}
