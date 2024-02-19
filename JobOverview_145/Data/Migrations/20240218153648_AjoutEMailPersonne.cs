using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOverview_v145.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjoutEMailPersonne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Personnes",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Personnes");
        }
    }
}
