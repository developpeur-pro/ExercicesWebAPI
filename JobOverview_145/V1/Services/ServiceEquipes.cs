using JobOverview_v145.Entities;
using JobOverview_v145.Data;
using Microsoft.EntityFrameworkCore;

namespace JobOverview_v145.V1.Services
{
    public interface IServiceEquipes
    {
        Task<List<Equipe>> ObtenirEquipes(string codeFilière);
        Task<Equipe?> ObtenirEquipe(string codeFilière, string codeEquipe);
        Task<Equipe> AjouterEquipe(string codeFilière, Equipe équipe);
        Task<Personne> AjouterPersonne(string codeEquipe, Personne personne);
        Task<int> ChangerManagerEquipe(string equipe, string manager);
    }

    public class ServiceEquipes : IServiceEquipes
    {
        private readonly ContexteJobOverview _contexte;

        public ServiceEquipes(ContexteJobOverview contexte)
        {
            _contexte = contexte;
        }

        // Renvoie les équipes d'une filière donnée avec leur service
        public async Task<List<Equipe>> ObtenirEquipes(string codeFilière)
        {
            var req = from e in _contexte.Equipes.Include(e => e.Service)
                      where e.CodeFiliere == codeFilière
                      select e;

            return await req.ToListAsync();
        }

        // Renvoie une équipe de code donné d'une filière donnée
        // avec son service, ses personnes et leur métier
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

        // Ajoute une équipe avec des personnes dans une filière donnée
        public async Task<Equipe> AjouterEquipe(string codeFilière, Equipe équipe)
        {
            équipe.CodeFiliere = codeFilière;
            équipe.Service = null!;
            foreach (Personne p in équipe.Personnes)
            {
                p.Métier = null!;
            }
            _contexte.Equipes.Add(équipe);

            await _contexte.SaveChangesAsync();

            return équipe;
        }

        // Ajoute une personne dans une équipe donnée
        public async Task<Personne> AjouterPersonne(string codeEquipe, Personne personne)
        {
            personne.CodeEquipe = codeEquipe;
            personne.Métier = null!;

            _contexte.Personnes.Add(personne);
            await _contexte.SaveChangesAsync();

            return personne;
        }

        // Change le manager d'une équipe
        public async Task<int> ChangerManagerEquipe(string equipe, string manager)
        {
            using (var transaction = _contexte.Database.BeginTransaction())
            {
                // Modifie le manager de toutes les personnes de l'équipe
                int nbModifs = await _contexte.Personnes
                .Where(p => p.CodeEquipe == equipe)
                .ExecuteUpdateAsync(setter => setter.SetProperty(p => p.Manager, manager));

                // Remet à null le champ manager pour le manager lui-même
                await _contexte.Personnes
                    .Where(p => p.Pseudo == manager)
                    .ExecuteUpdateAsync(setter => setter.SetProperty(p => p.Manager, (string?)null));

                transaction.Commit();
                return nbModifs;
            }
        }
    }
}
