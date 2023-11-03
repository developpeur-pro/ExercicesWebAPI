using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOverview_v55.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreationBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filieres",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filieres", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Logiciels",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CodeFiliere = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logiciels", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Logiciels_Filieres_CodeFiliere",
                        column: x => x.CodeFiliere,
                        principalTable: "Filieres",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CodeLogiciel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    CodeModuleParent = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CodeLogicielParent = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => new { x.Code, x.CodeLogiciel });
                    table.ForeignKey(
                        name: "FK_Modules_Logiciels_CodeLogiciel",
                        column: x => x.CodeLogiciel,
                        principalTable: "Logiciels",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Modules_Modules_CodeModuleParent_CodeLogicielParent",
                        columns: x => new { x.CodeModuleParent, x.CodeLogicielParent },
                        principalTable: "Modules",
                        principalColumns: new[] { "Code", "CodeLogiciel" });
                });

            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Numero = table.Column<float>(type: "real", nullable: false),
                    CodeLogiciel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Millesime = table.Column<short>(type: "smallint", nullable: false),
                    DateOuverture = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateSortiePrevue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateSortieReelle = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => new { x.Numero, x.CodeLogiciel });
                    table.ForeignKey(
                        name: "FK_Versions_Logiciels_CodeLogiciel",
                        column: x => x.CodeLogiciel,
                        principalTable: "Logiciels",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    Numero = table.Column<short>(type: "smallint", nullable: false),
                    NumeroVersion = table.Column<float>(type: "real", nullable: false),
                    CodeLogiciel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    DatePubli = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => new { x.Numero, x.NumeroVersion, x.CodeLogiciel });
                    table.ForeignKey(
                        name: "FK_Releases_Versions_NumeroVersion_CodeLogiciel",
                        columns: x => new { x.NumeroVersion, x.CodeLogiciel },
                        principalTable: "Versions",
                        principalColumns: new[] { "Numero", "CodeLogiciel" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logiciels_CodeFiliere",
                table: "Logiciels",
                column: "CodeFiliere");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CodeLogiciel",
                table: "Modules",
                column: "CodeLogiciel");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CodeModuleParent_CodeLogicielParent",
                table: "Modules",
                columns: new[] { "CodeModuleParent", "CodeLogicielParent" });

            migrationBuilder.CreateIndex(
                name: "IX_Releases_NumeroVersion_CodeLogiciel",
                table: "Releases",
                columns: new[] { "NumeroVersion", "CodeLogiciel" });

            migrationBuilder.CreateIndex(
                name: "IX_Versions_CodeLogiciel",
                table: "Versions",
                column: "CodeLogiciel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "Versions");

            migrationBuilder.DropTable(
                name: "Logiciels");

            migrationBuilder.DropTable(
                name: "Filieres");
        }
    }
}
