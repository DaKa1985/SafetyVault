using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafetyVault.Api.Migrations
{
    /// <inheritdoc />
    public partial class UniqueChecklistNamePerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Checklists_UserId",
                table: "Checklists");

            migrationBuilder.CreateIndex(
                name: "IX_Checklists_UserId_Name",
                table: "Checklists",
                columns: new[] { "UserId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Checklists_UserId_Name",
                table: "Checklists");

            migrationBuilder.CreateIndex(
                name: "IX_Checklists_UserId",
                table: "Checklists",
                column: "UserId");
        }
    }
}
