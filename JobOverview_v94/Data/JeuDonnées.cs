using JobOverview_v94.Entities;
using Microsoft.EntityFrameworkCore;
using Version = JobOverview_v94.Entities.Version;

namespace JobOverview_v94.Data
{
	public class JeuDonnées
	{
		/* Crée un jeu de données pour la partie Logiciels */
		public static void Créer(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Filiere>().HasData(
				 new Filiere { Code = "BIOV", Nom = "Biologie végétale" },
				 new Filiere { Code = "BIOH", Nom = "Biologie humaine" },
				 new Filiere { Code = "BIOA", Nom = "Support animale" }
				 );

			modelBuilder.Entity<Logiciel>().HasData(
				 new Logiciel { CodeFiliere = "BIOH", Code = "GENOMICA", Nom = "Genomica" },
				 new Logiciel { CodeFiliere = "BIOH", Code = "ANATOMIA", Nom = "Anatomia" }
				 );

			modelBuilder.Entity<Module>().HasData(
				 new Module
				 {
					 CodeLogiciel = "GENOMICA",
					 Code = "SEQUENCAGE",
					 Nom = "Séquençage"
				 },
				 new Module
				 {
					 CodeLogiciel = "GENOMICA",
					 Code = "MARQUAGE",
					 Nom = "Marquage",
					 CodeLogicielParent = "GENOMICA",
					 CodeModuleParent = "SEQUENCAGE"
				 },
				 new Module
				 {
					 CodeLogiciel = "GENOMICA",
					 Code = "SEPARATION",
					 Nom = "Séparation",
					 CodeLogicielParent = "GENOMICA",
					 CodeModuleParent = "SEQUENCAGE"
				 },
				 new Module
				 {
					 CodeLogiciel = "GENOMICA",
					 Code = "ANALYSE",
					 Nom = "Analyse",
					 CodeLogicielParent = "GENOMICA",
					 CodeModuleParent = "SEQUENCAGE"
				 },
				 new Module { CodeLogiciel = "GENOMICA", Code = "POLYMORPHISME", Nom = "Polymorphisme génétique" },
				 new Module { CodeLogiciel = "GENOMICA", Code = "VAR_ALLELE", Nom = "Variations alléliques" },
				 new Module { CodeLogiciel = "GENOMICA", Code = "UTILS_ROLES", Nom = "Utilisateurs et rôles" },
				 new Module { CodeLogiciel = "GENOMICA", Code = "PARAMETRES", Nom = "Paramètres" },
				 new Module { CodeLogiciel = "ANATOMIA", Code = "MICRO", Nom = "Anatomie microscopique" },
				 new Module { CodeLogiciel = "ANATOMIA", Code = "PATHO", Nom = "Anatomie pathologique" },
				 new Module { CodeLogiciel = "ANATOMIA", Code = "FONC", Nom = "Anatomie fonctionnelle" },
				 new Module { CodeLogiciel = "ANATOMIA", Code = "RADIO", Nom = "Anatomie radiologique" },
				 new Module { CodeLogiciel = "ANATOMIA", Code = "TOPO", Nom = "Anatomie topographique" }
				 );

			modelBuilder.Entity<Version>().HasData(
				 new Version
				 {
					 CodeLogiciel = "GENOMICA",
					 Numero = 1f,
					 Millesime = 2023,
					 DateOuverture = new DateTime(2022, 1, 2),
					 DateSortiePrevue = new DateTime(2023, 1, 8),
					 DateSortieReelle = new DateTime(2023, 1, 20)
				 },
				 new Version
				 {
					 CodeLogiciel = "GENOMICA",
					 Numero = 2f,
					 Millesime = 2024,
					 DateOuverture = new DateTime(2022, 12, 28),
					 DateSortiePrevue = new DateTime(2024, 2, 28)
				 },
				 new Version
				 {
					 CodeLogiciel = "ANATOMIA",
					 Numero = 4.5f,
					 Millesime = 2022,
					 DateOuverture = new DateTime(2021, 9, 1),
					 DateSortiePrevue = new DateTime(2022, 7, 7),
					 DateSortieReelle = new DateTime(2022, 7, 20)
				 },
				 new Version
				 {
					 CodeLogiciel = "ANATOMIA",
					 Numero = 5f,
					 Millesime = 2023,
					 DateOuverture = new DateTime(2022, 8, 1),
					 DateSortiePrevue = new DateTime(2023, 3, 30),
					 DateSortieReelle = new DateTime(2023, 3, 25)
				 },
				 new Version
				 {
					 CodeLogiciel = "ANATOMIA",
					 Numero = 5.5f,
					 Millesime = 2024,
					 DateOuverture = new DateTime(2023, 3, 30),
					 DateSortiePrevue = new DateTime(2023, 11, 20)
				 }
			);
		}
	}
}
