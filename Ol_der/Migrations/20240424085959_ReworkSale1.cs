using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ol_der.Migrations
{
    /// <inheritdoc />
    public partial class ReworkSale1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Sales",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "ItemNumber",
                table: "Sales",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "IsOrdered",
                table: "Sales",
                newName: "IsCardTransactionProcessed");

            migrationBuilder.AddColumn<decimal>(
                name: "CardTransactionAmount",
                table: "Sales",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Sales",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "SaleItem",
                columns: table => new
                {
                    SaleItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsOrdered = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItem", x => x.SaleItemId);
                    table.ForeignKey(
                        name: "FK_SaleItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleItem_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleItem_ProductId",
                table: "SaleItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItem_SaleId",
                table: "SaleItem",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleItem");

            migrationBuilder.DropColumn(
                name: "CardTransactionAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Sales",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Sales",
                newName: "ItemNumber");

            migrationBuilder.RenameColumn(
                name: "IsCardTransactionProcessed",
                table: "Sales",
                newName: "IsOrdered");
        }
    }
}
