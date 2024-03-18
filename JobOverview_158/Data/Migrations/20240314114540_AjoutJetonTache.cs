using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOverview_v158.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjoutJetonTache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Vers",
                table: "Taches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vers",
                table: "Taches");
        }
    }
}
