using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burhansahin.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class DurumEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Durum",
                table: "Kitaplar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Kitaplar");
        }
    }
}
