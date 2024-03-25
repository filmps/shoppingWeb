using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineWeb.Migrations
{
    /// <inheritdoc />
    public partial class totaL2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Carts",
                newName: "AllTotalPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllTotalPrice",
                table: "Carts",
                newName: "TotalPrice");
        }
    }
}
