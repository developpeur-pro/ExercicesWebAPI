using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace TestJobOverview
{
	[TestClass]
	public class TestTaches
	{
		private IServiceTaches _service = null!;
		private Tache _nouvelleTache = null!;

		[TestInitialize]
		public void InitialiserTest()
		{
			// Initialise un DbContext et le service métier
			_service = new ServiceTaches(new ContexteJobOverview(TestInit.ContextOptions));

			// Initialise une nouvelle commande
			_nouvelleTache = new Tache()
			{
				Titre = "DEV Anatomia Topo",
				DureePrevue = 10.0m,
				DureeRestante = 10.0m,
				CodeActivite = "DEV",
				Personne = "LBREGON",
				CodeLogiciel = "ANATOMIA",
				CodeModule = "TOPO",
				NumVersion = 5f,
				Description = "Tache ajoutée"
			};
		}

		[TestMethod]
		public async Task ObtenirTache()
		{
			int idTache = 1;
			int nbTravaux = 9;

			var res = await _service.ObtenirTache(idTache);

			Assert.AreEqual(ResultKinds.Ok, res.ResultKind);
			Assert.IsNotNull(res.Data);
			Assert.AreEqual(nbTravaux, res.Data.Travaux.Count);
		}

		[DataTestMethod]
		[DataRow("RFORTIER", "GENOMICA", 1f, 6)]
		[DataRow("RFORTIER", null, null, 12)]
		[DataRow(null, "GENOMICA", 1f, 19)]
		public async Task ObtenirTaches2(string? personne, string? logiciel, float? version, int nbTaches)
		{
			var res = await _service.ObtenirTaches(personne, logiciel, version);

			Assert.AreEqual(nbTaches, res.Data.Count);
		}

		[TestMethod()]
		public async Task AjouterTacheCodePersonneInconnue()
		{
			_nouvelleTache.Personne = "AZERTY";
			var res = await _service.ModifierAjouterTache(_nouvelleTache);

			Assert.AreEqual(ResultKinds.NotFound, res.ResultKind);
			Assert.AreEqual(1, res.Errors.Count);
		}

		[TestMethod()]
		public async Task AjouterTacheCodeActivitéIncorrecte()
		{
			_nouvelleTache.CodeActivite = "AAA";
			var res = await _service.ModifierAjouterTache(_nouvelleTache);

			Assert.AreEqual(ResultKinds.InvalidData, res.ResultKind);
			Assert.AreEqual(1, res.Errors.Count);
		}

		[TestMethod()]
		public async Task AjouterTacheCorrecte()
		{
			var res = await _service.ModifierAjouterTache(_nouvelleTache);

			Assert.AreEqual(ResultKinds.Ok, res.ResultKind);
			Assert.IsTrue(_nouvelleTache.Id > 0);
		}

		[TestMethod]
		public async Task ModifierTache()
		{
			// Initialise une tâche déjà existante dans la base
			// en modifiant certaines de ses propriétés
			Tache tache = new Tache()
			{
				Id = 2,
				Titre = "AF Marquage",
				DureePrevue = 30.0m, // 48m
				DureeRestante = 2.0m,   //0
				CodeActivite = "ANF",
				Personne = "MWEBER", // RBEAUMONT
				CodeLogiciel = "GENOMICA",
				CodeModule = "MARQUAGE",
				NumVersion = 1,
				Description = "Tache modifiée" // vide
			};

			// Enregistre les modifications en base
			await _service.ModifierAjouterTache(tache);

			// Récupère la tâche modifiée et enlève ses travaux pour pouvoir
			// la comparer à la tâche initiale
			var res = await _service.ObtenirTache(tache.Id);

			Assert.IsNotNull(res.Data);
			Tache tacheLue = res.Data;
			tacheLue.Travaux = null!;

			// Pour comparer les 2 objets, on les sérialise en JSON
			string json1 = JsonSerializer.Serialize(tache, TestInit.JsonOptions);
			string json2 = JsonSerializer.Serialize(tacheLue, TestInit.JsonOptions);

			Assert.AreEqual(json1, json2);
		}

		[TestMethod()]
		public async Task AjouterTravailDateAvecHeures()
		{
			int idTache = 2;
			Travail travail = new Travail()
			{
				DateTravail = new DateTime(2024, 12, 1, 8, 0, 0),
				Heures = 5
			};

			var res = await _service.AjouterTravail(idTache, travail);

			Assert.AreEqual(ResultKinds.InvalidData, res.ResultKind);
			Assert.AreEqual(1, res.Errors.Count);
		}

		[TestMethod()]
		public async Task AjouterTravailHeuresIncorrectes()
		{
			int idTache = 2;
			Travail travail = new Travail()
			{
				DateTravail = new DateTime(2024, 12, 1),
				Heures = 8.5m
			};

			var res = await _service.AjouterTravail(idTache, travail);

			Assert.AreEqual(ResultKinds.InvalidData, res.ResultKind);
			Assert.AreEqual(1, res.Errors.Count);
		}

		[TestMethod()]
		public async Task AjouterTravailCorrect()
		{
			int idTache = 7;
			Travail travail = new Travail()
			{
				DateTravail = new DateTime(2022, 2, 28),
				Heures = 4m
			};

			await _service.AjouterTravail(idTache, travail);
			var res = await _service.ObtenirTache(idTache);

			// Vérifie que le travail a été ajouté
			Assert.AreEqual(ResultKinds.Ok, res.ResultKind);
			Assert.IsFalse(res.Errors.Any());
			Assert.IsNotNull(res.Data);
			Assert.AreEqual(12, res.Data.Travaux.Count, "Nombre travaux");

			// Vérifie que la durée restante sur la tâche a été diminuée
			Assert.AreEqual(12, res.Data.DureeRestante, "Durée restante");
		}

		[TestMethod]
		public async Task SupprimerTravailInexistant()
		{
			int idTache = 23;
			DateTime dateTravail = new DateTime(2023, 1, 19);

			var res = await _service.SupprimerTravail(idTache, dateTravail);

			Assert.AreEqual(ResultKinds.NotFound, res.ResultKind);
			Assert.AreEqual(1, res.Errors.Count);
		}

		[TestMethod]
		public async Task SupprimerTravail()
		{
			// Caractéristiques de la tâche contenant le travail
			int idTache = 23;
			decimal duréeRest = 19m;
			int nbTravaux = 12;

			// Travail existant en base, à supprimer
			Travail travail = new Travail()
			{
				DateTravail = new DateTime(2023, 1, 20),
				Heures = 2m
			};

			// Supprime le travail et récupère la tâche correspondante
			await _service.SupprimerTravail(idTache, travail.DateTravail);
			var res = await _service.ObtenirTache(idTache);

			Assert.AreEqual(ResultKinds.Ok, res.ResultKind);
			Assert.IsNotNull(res.Data);

			// Vérifie que le travail a été supprimé sur la tâche
			Assert.AreEqual(nbTravaux - 1, res.Data.Travaux.Count);

			// Vérifie que la durée restante sur la tâche a été augmentée
			Assert.AreEqual(duréeRest + travail.Heures, res.Data.DureeRestante);
		}

		[DataTestMethod]
		[DataRow("MWEBER", "GENOMICA", 2f, 6)]
		[DataRow("AZERTY", null, null, 0)]
		public async Task SupprimerTaches(string? personne, string? logiciel, float? version, int nbTaches)
		{
			var res = await _service.SupprimerTaches(personne, logiciel, version);

			Assert.AreEqual(ResultKinds.Ok, res.ResultKind);
			Assert.AreEqual(nbTaches, res.Data);
		}
	}
}