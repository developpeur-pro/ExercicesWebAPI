using JobOverview_v59.Entities;
using JobOverview_v59.Data;
using Microsoft.EntityFrameworkCore;

namespace JobOverview_v59.Services
{
	public interface IServiceEquipes
	{
		Task<List<Equipe>> ObtenirEquipes(string codeFilière);
		Task<Equipe?> ObtenirEquipe(string codeFilière, string codeEquipe);
	}

	public class ServiceEquipes : IServiceEquipes
	{
		private readonly ContexteJobOverview _contexte;

		public ServiceEquipes(ContexteJobOverview contexte)
		{
			_contexte = contexte;
		}

		public async Task<List<Equipe>> ObtenirEquipes(string codeFilière)
		{
			var req = from e in _contexte.Equipes.Include(e => e.Service)
						 where e.CodeFiliere == codeFilière
						 select e;

			return await req.ToListAsync();
		}

		public async Task<Equipe?> ObtenirEquipe(string codeFilière, string codeEquipe)
		{
			var req = from e in _contexte.Equipes
						 .Include(e => e.Service)
						 .Include(e => e.Personnes)
						 .ThenInclude(p => p.Métier)
						 where e.Code == codeEquipe
						 select e;

			return await req.FirstOrDefaultAsync();
		}
	}
}
