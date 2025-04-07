using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuturesPrice.DAL.Migrations
{
    /// <inheritdoc />
    public partial class propertycolumnremoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Properties",
                table: "LogEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "LogEntries",
                type: "text",
                nullable: true);
        }
    }
}
