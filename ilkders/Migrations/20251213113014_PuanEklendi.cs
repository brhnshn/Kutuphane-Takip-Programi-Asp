using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burhansahin.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class PuanEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Puan",
                table: "Kitaplar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Puan",
                table: "Kitaplar");
        }
    }
}
