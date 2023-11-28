using Microsoft.EntityFrameworkCore;
using JobOverview_v82.Entities;
using JobOverview_v82.Data;

namespace JobOverview_v82.Services
{
	public interface IServiceTaches
	{
		Task<List<Tache>> ObtenirTaches(string? personne, string? logiciel, float? version);
		Task<Tache?> ObtenirTache(int id);
		Task<Personne?> ObtenirPersonne(string peudo);
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

			_contexte.Taches.Add(tache);
			await _contexte.SaveChangesAsync();

			return tache;
		}

		// Ajoute un travail sur une tâche donnée
		public async Task<Travail> AjouterTravail(int idTache, Travail travail)
		{
			travail.IdTache = idTache;
			_contexte.Travaux.Add(travail);
			await _contexte.SaveChangesAsync();

			return travail;
		}
	}
}
