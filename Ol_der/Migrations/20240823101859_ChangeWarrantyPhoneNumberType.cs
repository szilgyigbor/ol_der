using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ol_der.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWarrantyPhoneNumberType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warranty_Products_ProductId",
                table: "Warranty");

            migrationBuilder.DropForeignKey(
                name: "FK_Warranty_Suppliers_SupplierId",
                table: "Warranty");

            migrationBuilder.DropForeignKey(
                name: "FK_WarrantyStatus_Warranty_WarrantyId",
                table: "WarrantyStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarrantyStatus",
                table: "WarrantyStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warranty",
                table: "Warranty");

            migrationBuilder.RenameTable(
                name: "WarrantyStatus",
                newName: "WarrantyStatuses");

            migrationBuilder.RenameTable(
                name: "Warranty",
                newName: "Warranties");

            migrationBuilder.RenameIndex(
                name: "IX_WarrantyStatus_WarrantyId",
                table: "WarrantyStatuses",
                newName: "IX_WarrantyStatuses_WarrantyId");

            migrationBuilder.RenameIndex(
                name: "IX_Warranty_SupplierId",
                table: "Warranties",
                newName: "IX_Warranties_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Warranty_ProductId",
                table: "Warranties",
                newName: "IX_Warranties_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Warranties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarrantyStatuses",
                table: "WarrantyStatuses",
                column: "WarrantyStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warranties",
                table: "Warranties",
                column: "WarrantyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warranties_Products_ProductId",
                table: "Warranties",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warranties_Suppliers_SupplierId",
                table: "Warranties",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarrantyStatuses_Warranties_WarrantyId",
                table: "WarrantyStatuses",
                column: "WarrantyId",
                principalTable: "Warranties",
                principalColumn: "WarrantyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warranties_Products_ProductId",
                table: "Warranties");

            migrationBuilder.DropForeignKey(
                name: "FK_Warranties_Suppliers_SupplierId",
                table: "Warranties");

            migrationBuilder.DropForeignKey(
                name: "FK_WarrantyStatuses_Warranties_WarrantyId",
                table: "WarrantyStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarrantyStatuses",
                table: "WarrantyStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warranties",
                table: "Warranties");

            migrationBuilder.RenameTable(
                name: "WarrantyStatuses",
                newName: "WarrantyStatus");

            migrationBuilder.RenameTable(
                name: "Warranties",
                newName: "Warranty");

            migrationBuilder.RenameIndex(
                name: "IX_WarrantyStatuses_WarrantyId",
                table: "WarrantyStatus",
                newName: "IX_WarrantyStatus_WarrantyId");

            migrationBuilder.RenameIndex(
                name: "IX_Warranties_SupplierId",
                table: "Warranty",
                newName: "IX_Warranty_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Warranties_ProductId",
                table: "Warranty",
                newName: "IX_Warranty_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Warranty",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarrantyStatus",
                table: "WarrantyStatus",
                column: "WarrantyStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warranty",
                table: "Warranty",
                column: "WarrantyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warranty_Products_ProductId",
                table: "Warranty",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warranty_Suppliers_SupplierId",
                table: "Warranty",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarrantyStatus_Warranty_WarrantyId",
                table: "WarrantyStatus",
                column: "WarrantyId",
                principalTable: "Warranty",
                principalColumn: "WarrantyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
