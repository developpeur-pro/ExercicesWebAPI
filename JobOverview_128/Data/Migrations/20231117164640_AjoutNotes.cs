using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOverview_v128.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjoutNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Versions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Releases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "GENOMICA", 1f },
                column: "Notes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "GENOMICA", 2f },
                column: "Notes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "ANATOMIA", 4.5f },
                column: "Notes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "ANATOMIA", 5f },
                column: "Notes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "ANATOMIA", 5.5f },
                column: "Notes",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Releases");
        }
    }
}
