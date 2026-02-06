using Ol_der.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Sales
{
    public static class SalePdfGenerator
    {
        public static void Generate(Sale sale, string filePath)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Eladás #{sale.SaleId}")
                            .FontSize(18)
                            .SemiBold();

                        col.Item().Text($"Dátum: {sale.Date:yyyy.MM.dd HH:mm}");
                        col.Item().Text($"Fizetés: {sale.PaymentType}");
                        col.Item().Text($"Végösszeg: {sale.TotalAmount:N0} Ft");

                        col.Item().LineHorizontal(1);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Db");
                                header.Cell().Text("Termék");
                                header.Cell().AlignRight().Text("Ár");
                                header.Cell().AlignRight().Text("Össz.");
                            });

                            foreach (var item in sale.SaleItems)
                            {
                                table.Cell().Text(item.Quantity.ToString());
                                table.Cell().Text(item.Product.Name);
                                table.Cell().AlignRight().Text($"{item.Price:N0}");
                                table.Cell().AlignRight().Text($"{item.Quantity * item.Price:N0}");
                            }
                        });

                        if (!string.IsNullOrWhiteSpace(sale.Notes))
                        {
                            col.Item().LineHorizontal(1);
                            col.Item().Text($"Megjegyzés: {sale.Notes}");
                        }
                    });
                });
            })
            .GeneratePdf(filePath);
        }
    }
}
