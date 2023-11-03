using JobOverview_v58.Data;
using JobOverview_v58.Entities;
using Microsoft.EntityFrameworkCore;
using Version = JobOverview_v58.Entities.Version;

namespace JobOverview_v58.Services
{
	public interface IServiceLogiciels
	{
		Task<List<Logiciel>> ObtenirLogiciels(string codeFilière);
		Task<Logiciel?> ObtenirLogiciel(string code);
		Task<List<Version>?> ObtenirVersionsLogiciel(string codeLogiciel, int? millésime);
	}
	
	public class ServiceLogiciels : IServiceLogiciels
	{
		private readonly ContexteJobOverview _contexte;

		public ServiceLogiciels(ContexteJobOverview contexte)
		{
			_contexte = contexte;
		}

		public async Task<List<Logiciel>> ObtenirLogiciels(string codeFilière)
		{
			var req = from l in _contexte.Logiciels
						 where l.CodeFiliere == codeFilière
						 select l;

			return await req.ToListAsync();
		}

		public async Task<Logiciel?> ObtenirLogiciel(string code)
		{
			// Récupère le logiciel et ses modules à plat
			var req = from e in _contexte.Logiciels
								.Include(l => l.Modules).ThenInclude(m => m.SousModules)
						 where e.Code == code
						 select e;

			Logiciel? logiciel = await req.FirstOrDefaultAsync();

			if (logiciel == null) return null;

			// Transforme la liste des modules à plat en arborescence
			var req2 = from m in logiciel.Modules
						  where m.CodeModuleParent == null
						  select new Module
						  {
							  Code = m.Code,
							  Nom = m.Nom,
							  CodeLogiciel = m.CodeLogiciel,
							  SousModules = (from sm in m.SousModules select sm).ToList()
						  };

			logiciel.Modules = req2.ToList();

			return logiciel;
		}

		// Versions et releases d'un logiciel
		public async Task<List<Version>?> ObtenirVersionsLogiciel(string codeLogiciel, int? millésime)
		{
			// On vérifie si le logiciel existe
			if (await _contexte.Logiciels.FindAsync(codeLogiciel) == null)
				return null;

			// On récupère ses versions et releases
			var req = from v in _contexte.Versions.Include(v => v.Releases)
						 where v.CodeLogiciel == codeLogiciel &&
								 (millésime == null || v.Millesime == millésime)
						 select v;

			return await req.ToListAsync();
		}
	}
}
