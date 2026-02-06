using Ol_der.Controls.Sales;
using Ol_der.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.SalePackages
{
    public static class SalePackagePdfGenerator
    {
        public static void Generate(Sale sale, string filePath)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Content().Column(col =>
                    {
                        // ===== HEADER =====
                        col.Item().Text("Olszer Kft.")
                            .FontSize(18)
                            .SemiBold();

                        col.Item().Text("2040 Budaörs, Szabadság út 34/2.");
                        col.Item().Text("+36 20 667 1000");

                        col.Item().PaddingVertical(10).LineHorizontal(1);

                        // ===== SALE DATAS =====
                        col.Item().Text($"Vásárlás száma: {sale.SaleId}");
                        col.Item().Text($"Dátum: {sale.Date:yyyy.MM.dd HH:mm}");
                        col.Item().Text($"Végösszeg: {sale.TotalAmount:N0} Ft")
                            .SemiBold();

                        col.Item().PaddingVertical(10).LineHorizontal(1);

                        // ===== ITEMS =====
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);   // Quantity
                                columns.ConstantColumn(80);   // ItemNumber
                                columns.RelativeColumn();     // Name
                                columns.ConstantColumn(70);   // Price
                                columns.ConstantColumn(80);   // Subtotal
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Db").SemiBold();
                                header.Cell().Text("Cikkszám").SemiBold();
                                header.Cell().Text("Megnevezés").SemiBold();
                                header.Cell().AlignRight().Text("Ár").SemiBold();
                                header.Cell().AlignRight().Text("Össz.").SemiBold();
                            });

                            foreach (var item in sale.SaleItems)
                            {
                                table.Cell().Text(item.Quantity.ToString());
                                table.Cell().Text(item.Product?.ItemNumber ?? "-");
                                table.Cell().Text(item.Product?.Name ?? "(ismeretlen)");
                                table.Cell().AlignRight().Text($"{item.Price:N0}");
                                table.Cell().AlignRight().Text($"{item.Quantity * item.Price:N0}");
                            }
                        });

                        col.Item().PaddingTop(20)
                            .AlignRight()
                            .Text("Köszönjük a vásárlást!")
                            .Italic();
                    });
                });
            })
            .GeneratePdf(filePath);
        }
    }
}
