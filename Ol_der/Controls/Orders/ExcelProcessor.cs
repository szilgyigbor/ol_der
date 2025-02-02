using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using Microsoft.Win32;
using Ol_der.Models;

namespace Ol_der.Controls.Orders
{
    public class ExcelProcessor
    {
        public void ProcessExcelFile()
        {
            string filePath = SelectFile();
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs fájl kiválaszva!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            var mergedItems = ReadAndMergeExcelData(filePath);

            if (!mergedItems.Any())
            {
                MessageBoxOkWindow messageBoxOkWindow1 = new("A fájl nem tartalmaz feldolgozható adatot.");
                messageBoxOkWindow1.ShowDialog();
                return;
            }

            SaveMergedDataToExcel(filePath, mergedItems);
            MessageBoxOkWindow messageBoxOkWindow2 = new("Duplikációk szűrve, a fájl sikeresen mentve lett!");
            messageBoxOkWindow2.ShowDialog();
        }

        private string SelectFile()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls",
                Title = "Válassz egy Excel fájlt",
                Multiselect = false
            };

            return (openFileDialog.ShowDialog() == DialogResult.OK) ? openFileDialog.FileName : null;
        }

        private List<OrderItem> ReadAndMergeExcelData(string filePath)
        {
            var orderItems = new List<OrderItem>();
            var productDictionary = new Dictionary<string, Product>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed();

                foreach (var row in rows)
                {
                    string itemNumber = row.Cell(2).GetValue<string>();
                    if (string.IsNullOrWhiteSpace(itemNumber)) continue;

                    var product = productDictionary.ContainsKey(itemNumber)
                        ? productDictionary[itemNumber]
                        : new Product { ItemNumber = itemNumber, Name = row.Cell(3).GetValue<string>() };

                    if (!productDictionary.ContainsKey(itemNumber))
                    {
                        productDictionary[itemNumber] = product;
                    }

                    int quantityOrdered;
                    try
                    {
                        quantityOrdered = row.Cell(1).GetValue<int>();
                    }
                    catch (Exception)
                    {
                        MessageBoxOkWindow messageBoxOkWindow = new($"Hiba! A mennyiségnél nem szám szerepel ennél a cikkszámnál: {itemNumber}");
                        messageBoxOkWindow.ShowDialog();


                        return new List<OrderItem>();
                    }

                    var orderItem = new OrderItem
                    {
                        QuantityOrdered = quantityOrdered,
                        Product = product,
                        ProductId = product.ProductId,
                        Comment = row.Cell(4).GetValue<string>()
                    };

                    orderItems.Add(orderItem);
                }
            }

            return orderItems
                .GroupBy(o => o.Product.ItemNumber)
                .Select(g => new OrderItem
                {
                    Product = g.First().Product,
                    QuantityOrdered = g.Sum(o => o.QuantityOrdered),
                    Comment = string.Join(" + ", g.Select(o => o.Comment).Where(c => !string.IsNullOrWhiteSpace(c)))
                })
                .ToList();
        }

        private void SaveMergedDataToExcel(string originalFilePath, List<OrderItem> mergedItems)
        {
            string directory = Path.GetDirectoryName(originalFilePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFilePath);
            string newFilePath = Path.Combine(directory, fileNameWithoutExtension + "_WithoutDuplicates.xlsx");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Merged Data");

                worksheet.Cell(1, 1).Value = "Mennyiség";
                worksheet.Cell(1, 2).Value = "Cikkszám";
                worksheet.Cell(1, 3).Value = "Megnevezés";
                worksheet.Cell(1, 4).Value = "Megjegyzés";

                int row = 2;
                foreach (var item in mergedItems)
                {
                    worksheet.Cell(row, 1).Value = item.QuantityOrdered;
                    worksheet.Cell(row, 2).Value = item.Product.ItemNumber;
                    worksheet.Cell(row, 3).Value = item.Product.Name;
                    worksheet.Cell(row, 4).Value = item.Comment;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(newFilePath);
            }
        }
    }
}
