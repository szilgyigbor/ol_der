using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ol_der.Migrations
{
    /// <inheritdoc />
    public partial class SaleIsPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPackage",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPackage",
                table: "Sales");
        }
    }
}
