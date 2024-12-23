﻿using System;
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
using System.Windows.Shapes;

namespace Ol_der.Controls.Sales
{
    /// <summary>
    /// Interaction logic for InputSaleNumberWindow.xaml
    /// </summary>
    public partial class InputSaleNumberWindow : Window
    {
        public int? NumberResult { get; private set; }

        public InputSaleNumberWindow()
        {
            InitializeComponent();
            NumberInput.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumberInput.Text, out int number))
            {
                NumberResult = number;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Kérlek, érvényes számot adj meg!");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
