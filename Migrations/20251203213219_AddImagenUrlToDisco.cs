using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace escuchify_api.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenUrlToDisco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Discos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Discos");
        }
    }
}
