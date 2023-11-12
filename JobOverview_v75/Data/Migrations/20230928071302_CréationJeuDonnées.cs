using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobOverview_v75.Data.Migrations
{
    /// <inheritdoc />
    public partial class CréationJeuDonnées : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Filieres",
                columns: new[] { "Code", "Nom" },
                values: new object[,]
                {
                    { "BIOA", "Support animale" },
                    { "BIOH", "Biologie humaine" },
                    { "BIOV", "Biologie végétale" }
                });

            migrationBuilder.InsertData(
                table: "Logiciels",
                columns: new[] { "Code", "CodeFiliere", "Nom" },
                values: new object[,]
                {
                    { "ANATOMIA", "BIOH", "Anatomia" },
                    { "GENOMICA", "BIOH", "Genomica" }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Code", "CodeLogiciel", "CodeLogicielParent", "CodeModuleParent", "Nom" },
                values: new object[,]
                {
                    { "FONC", "ANATOMIA", null, null, "Anatomie fonctionnelle" },
                    { "MICRO", "ANATOMIA", null, null, "Anatomie microscopique" },
                    { "PARAMETRES", "GENOMICA", null, null, "Paramètres" },
                    { "PATHO", "ANATOMIA", null, null, "Anatomie pathologique" },
                    { "POLYMORPHISME", "GENOMICA", null, null, "Polymorphisme génétique" },
                    { "RADIO", "ANATOMIA", null, null, "Anatomie radiologique" },
                    { "SEQUENCAGE", "GENOMICA", null, null, "Séquençage" },
                    { "TOPO", "ANATOMIA", null, null, "Anatomie topographique" },
                    { "UTILS_ROLES", "GENOMICA", null, null, "Utilisateurs et rôles" },
                    { "VAR_ALLELE", "GENOMICA", null, null, "Variations alléliques" }
                });

            migrationBuilder.InsertData(
                table: "Versions",
                columns: new[] { "CodeLogiciel", "Numero", "DateOuverture", "DateSortiePrevue", "DateSortieReelle", "Millesime" },
                values: new object[,]
                {
                    { "GENOMICA", 1f, new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)2023 },
                    { "GENOMICA", 2f, new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (short)2024 },
                    { "ANATOMIA", 4.5f, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)2022 },
                    { "ANATOMIA", 5f, new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)2023 },
                    { "ANATOMIA", 5.5f, new DateTime(2023, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (short)2024 }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Code", "CodeLogiciel", "CodeLogicielParent", "CodeModuleParent", "Nom" },
                values: new object[,]
                {
                    { "ANALYSE", "GENOMICA", "GENOMICA", "SEQUENCAGE", "Analyse" },
                    { "MARQUAGE", "GENOMICA", "GENOMICA", "SEQUENCAGE", "Marquage" },
                    { "SEPARATION", "GENOMICA", "GENOMICA", "SEQUENCAGE", "Séparation" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Filieres",
                keyColumn: "Code",
                keyValue: "BIOA");

            migrationBuilder.DeleteData(
                table: "Filieres",
                keyColumn: "Code",
                keyValue: "BIOV");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "ANALYSE", "GENOMICA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "FONC", "ANATOMIA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "MARQUAGE", "GENOMICA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "MICRO", "ANATOMIA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "PARAMETRES", "GENOMICA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "PATHO", "ANATOMIA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "POLYMORPHISME", "GENOMICA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "RADIO", "ANATOMIA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "SEPARATION", "GENOMICA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "TOPO", "ANATOMIA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "UTILS_ROLES", "GENOMICA" });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "VAR_ALLELE", "GENOMICA" });

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "GENOMICA", 1f });

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "GENOMICA", 2f });

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "ANATOMIA", 4.5f });

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "ANATOMIA", 5f });

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumns: new[] { "CodeLogiciel", "Numero" },
                keyValues: new object[] { "ANATOMIA", 5.5f });

            migrationBuilder.DeleteData(
                table: "Logiciels",
                keyColumn: "Code",
                keyValue: "ANATOMIA");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumns: new[] { "Code", "CodeLogiciel" },
                keyValues: new object[] { "SEQUENCAGE", "GENOMICA" });

            migrationBuilder.DeleteData(
                table: "Logiciels",
                keyColumn: "Code",
                keyValue: "GENOMICA");

            migrationBuilder.DeleteData(
                table: "Filieres",
                keyColumn: "Code",
                keyValue: "BIOH");
        }
    }
}
