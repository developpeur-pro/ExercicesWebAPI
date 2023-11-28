using Microsoft.EntityFrameworkCore;
using JobOverview_v83.Entities;
using JobOverview_v83.Data;

namespace JobOverview_v83.Services
{
	public interface IServiceTaches
	{
		Task<List<Tache>> ObtenirTaches(string? personne, string? logiciel, float? version);
		Task<Tache?> ObtenirTache(int id);
		Task<Personne?> ObtenirPersonne(string pseudo);
		Task<Tache> AjouterTache(Tache tache);
		Task<Travail> AjouterTravail(int idTache, Travail tache);
	}

	public class ServiceTaches : IServiceTaches
	{
		private readonly ContexteJobOverview _contexte;

		public ServiceTaches(ContexteJobOverview contexte)
		{
			_contexte = contexte;
		}

		// Renvoie le tâches pour une personne, un logiciel et une version donnés
		public async Task<List<Tache>> ObtenirTaches(string? personne, string? logiciel, float? version)
		{
			var req = from t in _contexte.Taches
						 where (personne == null || t.Personne == personne) &&
								(logiciel == null || t.CodeLogiciel == logiciel) &&
								(version == null || t.NumVersion == version)
						 orderby t.CodeLogiciel, t.NumVersion
						 select t;

			return await req.ToListAsync();
		}

		// Renvoie une tâche et ses travaux associés
		public async Task<Tache?> ObtenirTache(int id)
		{
			var req = from t in _contexte.Taches
						 .Include(x => x.Travaux.OrderBy(x => x.DateTravail))
						 where t.Id == id
						 select t;

			return await req.FirstOrDefaultAsync();
		}

		// Renvoie une personne avec son métier et ses activités ou null si le pseudo n'existe pas
		public async Task<Personne?> ObtenirPersonne(string pseudo)
		{
			var req = from p in _contexte.Personnes
						 .Include(p => p.Métier)
						 .ThenInclude(m => m.Activités)
						 where p.Pseudo == pseudo
						 select p;

			var pers = await req.FirstOrDefaultAsync();
			return pers;
		}

		// Ajoute une tâche
		public async Task<Tache> AjouterTache(Tache tache)
		{
			tache.Travaux = null!;

			// Récupère la personne et ses activités
			Personne? p = await ObtenirPersonne(tache.Personne);
			if (p == null)
				throw new ValidationRulesException("Personne", $"Personne {tache.Personne} non trouvée");

			// Vérifie si le code activité de la tâche fait partie de ceux de la personne
			if (p.Métier.Activités.Find(a => a.Code == tache.CodeActivite) == null)
				throw new ValidationRulesException("CodeActivite", "L'activité ne correspond pas au métier de la personne.");

			_contexte.Taches.Add(tache);
			await _contexte.SaveChangesAsync();

			return tache;
		}

		// Ajoute un travail sur une tâche donnée
		public async Task<Travail> AjouterTravail(int idTache, Travail travail)
		{
			ValidationRulesException vre = new();
			if (travail.DateTravail.TimeOfDay != new TimeSpan())
				vre.Errors.Add("Date", new string[] { "La partie horaire de la date doit être à 0" });
			
			if (travail.Heures < 0.5m || travail.Heures > 8 )
				vre.Errors.Add("Heures", new string[] { "Le nombre d'heures doit être compris entre 0.5 et 8" });

			if (vre.Errors.Any()) throw vre;

			// Récupère la tâche
			Tache? tache = await _contexte.Taches.FindAsync(idTache);
			//Tache? tache = await _contexte.Taches.Where(t => t.Id == idTache)
			//					.AsTracking().FirstOrDefaultAsync();
			if (tache == null)
				throw new ValidationRulesException("IdTache", $"Tache {idTache} non trouvée");

			// Récupère la personne associée à la tâche et ses activités
			Personne? p = await ObtenirPersonne(tache.Personne);

			travail.IdTache = idTache;
			travail.TauxProductivite = p!.TauxProductivite;

			// Met à jour la durée de travail restante sur la tâche
			tache.DureeRestante -= travail.Heures;
			if (tache.DureeRestante < 0) tache.DureeRestante = 0;

			_contexte.Travaux.Add(travail);
			await _contexte.SaveChangesAsync();

			return travail;
		}
	}
}
