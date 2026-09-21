using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafetyVault.Api.Migrations
{
    /// <inheritdoc />
    public partial class UniqueItemName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_Name",
                table: "Items");
        }
    }
}
