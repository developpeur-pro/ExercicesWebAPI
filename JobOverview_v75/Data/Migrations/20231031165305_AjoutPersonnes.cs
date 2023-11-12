using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOverview_v75.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjoutPersonnes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Equipes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    CodeService = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CodeFiliere = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipes", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Equipes_Filieres_CodeFiliere",
                        column: x => x.CodeFiliere,
                        principalTable: "Filieres",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Equipes_Services_CodeService",
                        column: x => x.CodeService,
                        principalTable: "Services",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Metiers",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Titre = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    CodeService = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metiers", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Metiers_Services_CodeService",
                        column: x => x.CodeService,
                        principalTable: "Services",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Personnes",
                columns: table => new
                {
                    Pseudo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    TauxProductivite = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 1m),
                    CodeEquipe = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CodeMetier = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Manager = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnes", x => x.Pseudo);
                    table.ForeignKey(
                        name: "FK_Personnes_Equipes_CodeEquipe",
                        column: x => x.CodeEquipe,
                        principalTable: "Equipes",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Personnes_Metiers_CodeMetier",
                        column: x => x.CodeMetier,
                        principalTable: "Metiers",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Personnes_Personnes_Manager",
                        column: x => x.Manager,
                        principalTable: "Personnes",
                        principalColumn: "Pseudo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_CodeFiliere",
                table: "Equipes",
                column: "CodeFiliere");

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_CodeService",
                table: "Equipes",
                column: "CodeService");

            migrationBuilder.CreateIndex(
                name: "IX_Metiers_CodeService",
                table: "Metiers",
                column: "CodeService");

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_CodeEquipe",
                table: "Personnes",
                column: "CodeEquipe");

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_CodeMetier",
                table: "Personnes",
                column: "CodeMetier");

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_Manager",
                table: "Personnes",
                column: "Manager");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personnes");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropTable(
                name: "Metiers");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
