using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebShowStep.Migrations
{
    /// <inheritdoc />
    public partial class editpage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditPage",
                table: "ProductTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PartialPage",
                table: "ProductTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditPage",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "PartialPage",
                table: "ProductTypes");
        }
    }
}
