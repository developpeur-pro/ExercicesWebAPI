using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JobOverview_v145.Controllers;
using JobOverview_v145.V1.Services;
using JobOverview_v145.Entities;
using JobOverview_v145.V1.Entities;
using Asp.Versioning;

namespace JobOverview_v145.V1.Controllers
{
	[Route("api/Filieres/{codeFiliere}/[controller]")]
	[ApiVersion(1.0)]
	[ApiController]
	[Authorize(Policy = "GérerEquipes")]
	public class EquipesController : ControllerBase
	{
		private readonly IServiceEquipes _service;
		private readonly ILogger<EquipesController> _logger;

		public EquipesController(IServiceEquipes service, ILogger<EquipesController> logger)
		{
			_service = service;
			_logger = logger;
		}

		// GET: api/Filieres/BIOH/Equipes
		[HttpGet]
		public async Task<ActionResult<Filiere?>> GetEquipes(string codeFiliere)
		{
			var équipes = await _service.ObtenirEquipes(codeFiliere);
			if (!équipes.Any()) return NotFound();
			return Ok(équipes);
		}

		// GET: api/Filieres/BIOH/Equipes/BIOH_DEV
		[HttpGet("{codeEquipe}")]
		public async Task<ActionResult<Equipe?>> GetEquipe(string codeFiliere, string codeEquipe)
		{
			var équipe = await _service.ObtenirEquipe(codeFiliere, codeEquipe);
			if (équipe == null) return NotFound();
			return Ok(équipe);
		}

		// POST: api/Filieres/BIOV/Equipes
		[HttpPost]
		public async Task<ActionResult<Equipe>> PostEquipe(string codeFiliere, EquipeDTO eqDTO)
		{
			Equipe eq = new Equipe
			{
				Code = eqDTO.Code,
				CodeFiliere = eqDTO.CodeFiliere,
				CodeService = eqDTO.CodeService,
				Nom = eqDTO.Nom,
				Service = eqDTO.Service,
				Personnes = new()
			};

			foreach (PersonneDTO p in eqDTO.Personnes)
				eq.Personnes.Add(GetPersonneFromDTO(p));

			try
			{
				Equipe res = await _service.AjouterEquipe(codeFiliere, eq);

				// Renvoie une réponse de code 201 avec l'en-tête 
				// "location: <url d'accès à l’équipe>" et un corps contenant l’équipe
				return CreatedAtAction(nameof(GetEquipe), new { codeFiliere = res.CodeFiliere, codeEquipe = res.Code }, res);
			}
			catch (Exception e)
			{
				// Journalise des détails sur l'erreur et renvoie la réponse HTTP 
				return this.CustomResponseForError(e, eq, _logger);
			}
		}

		private Personne GetPersonneFromDTO(PersonneDTO persDTO)
		{
			return new Personne
			{
				Pseudo = persDTO.Pseudo,
				Nom = persDTO.Nom,
				Prenom = persDTO.Prenom,
				Email = string.Empty,
				CodeEquipe = persDTO.CodeEquipe,
				CodeMetier = persDTO.CodeMetier,
				Manager = persDTO.Manager,
				Métier = persDTO.Métier,
				TauxProductivite = persDTO.TauxProductivite
			};
		}

		// POST: api/Filieres/BIOV/Equipes/BIOV_MKT
		[HttpPost("{codeEquipe}")]
		public async Task<ActionResult<Equipe>> PostPersonne(string codeFiliere, string codeEquipe, PersonneDTO persDTO)
		{
			Personne pers = GetPersonneFromDTO(persDTO);
			try
			{
				Personne res = await _service.AjouterPersonne(codeEquipe, pers);

				// Renvoie une réponse de code 201 avec l'en-tête 
				// "location: <url d'accès à l’équipe de la personne>" et un corps contenant l’équipe
				return CreatedAtAction(nameof(GetEquipe), new { codeFiliere, codeEquipe }, res);
			}
			catch (Exception e)
			{
				// Journalise des détails sur l'erreur et renvoie la réponse HTTP 
				return this.CustomResponseForError(e, pers, _logger);
			}
		}

		// PUT: api/Filieres/BIOV/Equipes/BIOV_MKT?manager=AFERRAND
		[HttpPut("{codeEquipe}")]
		public async Task<ActionResult> PutPersonne(string codeEquipe, [FromQuery] string manager)
		{
			try
			{
				int nbMaj = await _service.ChangerManagerEquipe(codeEquipe, manager);
				return Ok(nbMaj + " personnes modifiées");
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}
	}
}
