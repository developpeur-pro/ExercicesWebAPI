using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOverview_v158.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTaches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activites",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Titre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activites", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ActivitesMetiers",
                columns: table => new
                {
                    CodeActivite = table.Column<string>(type: "varchar(20)", nullable: false),
                    CodeMetier = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitesMetiers", x => new { x.CodeActivite, x.CodeMetier });
                    table.ForeignKey(
                        name: "FK_ActivitesMetiers_Activites_CodeActivite",
                        column: x => x.CodeActivite,
                        principalTable: "Activites",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivitesMetiers_Metiers_CodeMetier",
                        column: x => x.CodeMetier,
                        principalTable: "Metiers",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Taches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DureePrevue = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    DureeRestante = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    CodeActivite = table.Column<string>(type: "varchar(20)", nullable: false),
                    Personne = table.Column<string>(type: "varchar(20)", nullable: false),
                    CodeLogiciel = table.Column<string>(type: "varchar(20)", nullable: false),
                    CodeModule = table.Column<string>(type: "varchar(20)", nullable: false),
                    NumVersion = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taches_Activites_CodeActivite",
                        column: x => x.CodeActivite,
                        principalTable: "Activites",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Taches_Modules_CodeModule_CodeLogiciel",
                        columns: x => new { x.CodeModule, x.CodeLogiciel },
                        principalTable: "Modules",
                        principalColumns: new[] { "Code", "CodeLogiciel" });
                    table.ForeignKey(
                        name: "FK_Taches_Personnes_Personne",
                        column: x => x.Personne,
                        principalTable: "Personnes",
                        principalColumn: "Pseudo");
                    table.ForeignKey(
                        name: "FK_Taches_Versions_NumVersion_CodeLogiciel",
                        columns: x => new { x.NumVersion, x.CodeLogiciel },
                        principalTable: "Versions",
                        principalColumns: new[] { "Numero", "CodeLogiciel" });
                });

            migrationBuilder.CreateTable(
                name: "Travaux",
                columns: table => new
                {
                    DateTravail = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdTache = table.Column<int>(type: "int", nullable: false),
                    Heures = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    TauxProductivite = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 1m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travaux", x => new { x.DateTravail, x.IdTache });
                    table.ForeignKey(
                        name: "FK_Travaux_Taches_IdTache",
                        column: x => x.IdTache,
                        principalTable: "Taches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivitesMetiers_CodeMetier",
                table: "ActivitesMetiers",
                column: "CodeMetier");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_CodeActivite",
                table: "Taches",
                column: "CodeActivite");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_CodeModule_CodeLogiciel",
                table: "Taches",
                columns: new[] { "CodeModule", "CodeLogiciel" });

            migrationBuilder.CreateIndex(
                name: "IX_Taches_NumVersion_CodeLogiciel",
                table: "Taches",
                columns: new[] { "NumVersion", "CodeLogiciel" });

            migrationBuilder.CreateIndex(
                name: "IX_Taches_Personne",
                table: "Taches",
                column: "Personne");

            migrationBuilder.CreateIndex(
                name: "IX_Travaux_IdTache",
                table: "Travaux",
                column: "IdTache");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivitesMetiers");

            migrationBuilder.DropTable(
                name: "Travaux");

            migrationBuilder.DropTable(
                name: "Taches");

            migrationBuilder.DropTable(
                name: "Activites");
        }
    }
}
