using JobOverview_v158.Data;
using JobOverview_v158.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using JobOverview_v158.Tools;
using System.Text.RegularExpressions;
using Version = JobOverview_v158.Entities.Version;

namespace JobOverview_v158.Services
{
	public interface IServiceLogiciels
	{
		Task<ServiceResult<List<Logiciel>>> ObtenirLogiciels(string codeFilière);
		Task<ServiceResult<Logiciel?>> ObtenirLogiciel(string code);
		Task<ServiceResult<List<Version>?>> ObtenirVersionsLogiciel(string codeLogiciel, int? millésime);
		Task<ServiceResult<Version?>> AjouterVersion(string codeLogiciel, Version version);
		Task<ServiceResult<int>> ModifierVersion(string codeLogiciel, float numVersion, JsonPatchDocument<Version> patch);
		Task<ServiceResult<Release?>> ObtenirRelease(string codeLogiciel, float numVersion, short numRelease);
		Task<ServiceResult<Release?>> AjouterRelease(string codeLogiciel, float numVersion, Release release);
	}

	public class ServiceLogiciels : ServiceBase, IServiceLogiciels
	{
		private readonly ContexteJobOverview _contexte;

		public ServiceLogiciels(ContexteJobOverview contexte) : base(contexte)
		{
			_contexte = contexte;
		}

		public async Task<ServiceResult<List<Logiciel>>> ObtenirLogiciels(string codeFilière)
		{
			var req = from l in _contexte.Logiciels
						 where l.CodeFiliere == codeFilière
						 select l;

			var res = await req.ToListAsync();
			return ResultOk(res);
		}

		public async Task<ServiceResult<Logiciel?>> ObtenirLogiciel(string code)
		{
			// Récupère le logiciel et ses modules à plat
			var req = from e in _contexte.Logiciels
								.Include(l => l.Modules).ThenInclude(m => m.SousModules)
						 where e.Code == code
						 select e;

			Logiciel? logiciel = await req.FirstOrDefaultAsync();

			if (logiciel == null)
				return ResultNotFound<Logiciel?>(code);

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

			return ResultOk<Logiciel?>(logiciel);
		}

		// Versions et releases d'un logiciel
		public async Task<ServiceResult<List<Version>?>> ObtenirVersionsLogiciel(string codeLogiciel, int? millésime)
		{
			// On vérifie si le logiciel existe
			if (await _contexte.Logiciels.FindAsync(codeLogiciel) == null)
				return ResultNotFound<List<Version>?>(codeLogiciel);

			// On récupère ses versions et releases
			var req = from v in _contexte.Versions.Include(v => v.Releases)
						 where v.CodeLogiciel == codeLogiciel &&
								 (millésime == null || v.Millesime == millésime)
						 select v;

			var res = await req.ToListAsync();
			return ResultOk<List<Version>?>(res);
		}

		public async Task<ServiceResult<Version?>> AjouterVersion(string codeLogiciel, Version version)
		{
			version.CodeLogiciel = codeLogiciel;
			ErrorsDictionary erreurs = new();

			// Règles de validation
			Regex regex = new Regex(@"^\d{1,3}(.\d{1,2})?$");
			if (!regex.IsMatch(version.Numero.ToString()))
				erreurs.Add("Numero", $"Le numéro de version ({version.Numero}) doit avoir au maximum 3 chiffres avant la virgule et 2 après.");

			if (version.Millesime < 2020 || version.Millesime > 2100)
				erreurs.Add("Millesime", $"Le millésime ({version.Millesime}) doit être compris entre 2020 et 2100 inclus");

			if (version.DateOuverture >= version.DateSortiePrevue)
				erreurs.Add("DateOuverture", $"La date d'ouverture doit être < à la date de sortie prévue.");

			if (version.DateSortieReelle != null && version.DateOuverture >= version.DateSortieReelle)
				erreurs.Add("DateOuverture", $"La date d'ouverture doit être < à la date de sortie réelle.");

			if (erreurs.Any()) return ResultInvalidData<Version>(erreurs);

			// Enregistrement en base
			_contexte.Versions.Add(version);
			return await SaveAndResultCreatedAsync(version);
		}

		public async Task<ServiceResult<Release?>> ObtenirRelease(string codeLogiciel, float numVersion, short numRelease)
		{
			var res = await _contexte.Releases.FindAsync(numRelease, numVersion, codeLogiciel);
			return ResultOkOrNotFound(numRelease, res);
		}

		public async Task<ServiceResult<Release?>> AjouterRelease(string codeLogiciel, float numVersion, Release release)
		{
			release.CodeLogiciel = codeLogiciel;
			release.NumeroVersion = numVersion;

			// Récupère le N° de release maxi pour le logiciel et la version
			var req1 = from r in _contexte.Releases
						  where r.CodeLogiciel == codeLogiciel && r.NumeroVersion == numVersion
						  orderby r.Numero
						  select r.Numero;

			short relMax = await req1.LastOrDefaultAsync(); // renvoie 0 s'il n'y a aucune release

			if (relMax > 0)
			{
				if (release.Numero <= relMax)
					return ResultInvalidData<Release?>("Numero", $"Le N° de release doit être > {relMax}");				

				// Récupère la date de publication de la release précédente
				var req2 = from r in _contexte.Releases
							  where r.CodeLogiciel == codeLogiciel && r.NumeroVersion == numVersion && r.Numero == relMax
							  select r.DatePubli;
				DateTime datePubliPrec = await req2.FirstOrDefaultAsync();

				if (release.DatePubli < datePubliPrec)
					return ResultInvalidData<Release?>("DatePubli", $"La date de publication de la release doit être >= {datePubliPrec}");
			}

			_contexte.Releases.Add(release);
			return await SaveAndResultCreatedAsync<Release?>(release);
		}

		// Applique les modifications décrites par un patch sur une version donnée
		public async Task<ServiceResult<int>> ModifierVersion(string codeLogiciel, float numVersion, JsonPatchDocument<Version> patch)
		{
			// Si le suivi des modifs est désacativé au niveau du contexte, utiliser cette syntaxe
			//var req = from v in _contexte.Versions.AsTracking()
			//			 where v.Numero == numVersion && v.CodeLogiciel == codeLogiciel
			//			 select v;
			//var version = await req.FirstOrDefaultAsync();

			Version? version = await _contexte.Versions.FindAsync(codeLogiciel, numVersion);
			if (version == null)
				return ResultNotFound<int>(numVersion);

			// Applique les modifications demandées
			patch.ApplyTo(version);
			return await SaveAndResultOkAsync();
		}
	}
}
