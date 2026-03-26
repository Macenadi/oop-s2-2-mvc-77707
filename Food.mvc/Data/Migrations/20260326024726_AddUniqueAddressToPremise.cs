using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Food.mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueAddressToPremise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Premises",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Premises_Address",
                table: "Premises",
                column: "Address",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Premises_Address",
                table: "Premises");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Premises",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
