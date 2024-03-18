using Microsoft.EntityFrameworkCore;
using JobOverview_v158.Entities;
using JobOverview_v158.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using JobOverview_v158.Tools;

namespace JobOverview_v158.Services
{
	public interface IServiceTaches
	{
		Task<ServiceResult<List<Tache>>> ObtenirTaches(string? personne, string? logiciel, float? version);
		Task<ServiceResult<Tache?>> ObtenirTache(int id);
		Task<ServiceResult<Personne?>> ObtenirPersonne(string pseudo);
		Task<ServiceResult<Tache?>> ModifierAjouterTache(Tache tache);
		Task<ServiceResult<Travail?>> AjouterTravail(int idTache, Travail tache);
		Task<ServiceResult<int>> SupprimerTravail(int idTache, DateTime date);
		Task<ServiceResult<int>> SupprimerTaches(string? personne, string? logiciel, float? version);
	}

	public class ServiceTaches : ServiceBase, IServiceTaches
	{
		private readonly ContexteJobOverview _contexte;

		public ServiceTaches(ContexteJobOverview contexte) : base(contexte)
		{
			_contexte = contexte;
		}

		// Renvoie les tâches pour une personne, un logiciel et une version donnés
		public async Task<ServiceResult<List<Tache>>> ObtenirTaches(string? personne, string? logiciel, float? version)
		{
			var req = from t in _contexte.Taches
						 where (personne == null || t.Personne == personne) &&
								(logiciel == null || t.CodeLogiciel == logiciel) &&
								(version == null || t.NumVersion == version)
						 orderby t.CodeLogiciel, t.NumVersion
						 select t;

			var res = await req.ToListAsync();
			return ResultOk(res);
		}

		// Renvoie une tâche et ses travaux associés
		public async Task<ServiceResult<Tache?>> ObtenirTache(int id)
		{
			var req = from t in _contexte.Taches
						 .Include(x => x.Travaux.OrderBy(x => x.DateTravail))
						 where t.Id == id
						 select t;

			var res = await req.FirstOrDefaultAsync();
			return ResultOkOrNotFound(id, res);
		}

		// Renvoie une personne avec son métier et ses activités ou NotFound si le pseudo n'existe pas
		public async Task<ServiceResult<Personne?>> ObtenirPersonne(string pseudo)
		{
			var req = from p in _contexte.Personnes
						 .Include(p => p.Métier)
						 .ThenInclude(m => m.Activités)
						 where p.Pseudo == pseudo
						 select p;

			var pers = await req.FirstOrDefaultAsync();
			return ResultOkOrNotFound(pseudo, pers);
		}

		// Modifie une tâche ou l'ajoute si elle n'existe pas
		public async Task<ServiceResult<Tache?>> ModifierAjouterTache(Tache tache)
		{
			// Pour la modification, on vérifie tout d'abord si la tâche existe toujours en base
			if (tache.Id != 0)
			{
				var req = from t in _contexte.Taches.AsNoTracking()
							 where t.Id == tache.Id
							 select t.Id;

				if (await req.FirstOrDefaultAsync() == 0)
					return ResultNotFound<Tache?>(tache.Id);
			}

			tache.Travaux = null!;

			// Récupère la personne et ses activités
			var res = await ObtenirPersonne(tache.Personne);

			if (res.ResultKind != ResultKinds.Ok)
				return ResultNotFound<Tache?>($"Personne {tache.Personne} non trouvée");

			// Vérifie si le code activité de la tâche fait partie de ceux de la personne
			if (res.Data!.Métier.Activités.Find(a => a.Code == tache.CodeActivite) == null)
				return ResultInvalidData<Tache>("CodeActivite", "L'activité ne correspond pas au métier de la personne.");

			_contexte.Taches.Update(tache);

			// Génère une nouvelle valeur de jeton d'accès concurrentiel
			tache.Vers = Guid.NewGuid();

			return await SaveAndResultOkAsync(tache);
		}

		// Ajoute un travail sur une tâche donnée
		public async Task<ServiceResult<Travail?>> AjouterTravail(int idTache, Travail travail)
		{
			ErrorsDictionary erreurs = new();

			if (travail.DateTravail.TimeOfDay != new TimeSpan())
				erreurs.Add("Date", "La partie horaire de la date doit être à 0");

			if (travail.Heures < 0.5m || travail.Heures > 8)
				erreurs.Add("Heures", "Le nombre d'heures doit être compris entre 0.5 et 8");

			if (erreurs.Any())
				return ResultInvalidData<Travail?>(erreurs);

			// Récupère la tâche
			Tache? tache = await _contexte.Taches.Where(t => t.Id == idTache)
								.AsTracking().FirstOrDefaultAsync();

			if (tache == null)
				return ResultNotFound<Travail?>($"Tache {idTache} non trouvée");

			// Récupère la personne associée à la tâche et ses activités
			var res = await ObtenirPersonne(tache.Personne);

			if (res.ResultKind != ResultKinds.Ok)
				return ResultNotFound<Travail?>($"Personne {tache.Personne} non trouvée");

			travail.IdTache = idTache;
			travail.TauxProductivite = res.Data!.TauxProductivite;

			// Met à jour la durée de travail restante sur la tâche
			tache.DureeRestante -= travail.Heures;
			if (tache.DureeRestante < 0) tache.DureeRestante = 0;

			_contexte.Travaux.Add(travail);

			return await SaveAndResultOkAsync(travail);
		}

		// Supprime un travail
		public async Task<ServiceResult<int>> SupprimerTravail(int idTache, DateTime date)
		{
			// Récupère la tâche et ses travaux
			var res = await ObtenirTache(idTache);
			if (res.ResultKind != ResultKinds.Ok)
				return ResultNotFound<int>($"Tache {idTache} non trouvée");

			// Recherche le travail à supprimer
			Tache tache = res.Data!;
			Travail? travail = tache.Travaux.Where(t => t.DateTravail == date).FirstOrDefault();
			if (travail == null)
				return ResultNotFound<int>($"Aucun travail trouvé à la date du {date} sur la tâche {idTache}.");

			// Met à jour la durée de travail restante sur la tâche
			tache.DureeRestante += travail.Heures;

			// Rattache l'entité au suivi, sans ses filles, en passant son état à Modified
			EntityEntry<Tache> ent = _contexte.Entry(tache);
			ent.State = EntityState.Modified;

			// Supprime le travail
			_contexte.Remove(travail);

			return await SaveAndResultOkAsync();
		}

		// Supprime les tâches correspondant au filtre
		// et leurs travaux associés par cascade
		public async Task<ServiceResult<int>> SupprimerTaches(string? personne, string? logiciel, float? version)
		{
			var req = _contexte.Taches.Where(t =>
						(personne == null || t.Personne == personne) &&
						(logiciel == null || t.CodeLogiciel == logiciel) &&
						(version == null || t.NumVersion == version));

			int nbSuppr = await req.ExecuteDeleteAsync();

			return ResultOk(nbSuppr);
		}
	}
}
