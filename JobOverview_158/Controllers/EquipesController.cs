using Microsoft.AspNetCore.Mvc;
using JobOverview_v158.Entities;
using JobOverview_v158.Services;
using Microsoft.AspNetCore.Authorization;
using JobOverview_v158.Tools;

namespace JobOverview_v158.Controllers
{
	[Route("api/Filieres/{codeFiliere}/[controller]")]
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
			var res = await _service.ObtenirEquipes(codeFiliere);
			return res.ConvertToObjectResult();
		}

		// GET: api/Filieres/BIOH/Equipes/BIOH_DEV
		[HttpGet("{codeEquipe}")]
		public async Task<ActionResult<Equipe?>> GetEquipe(string codeFiliere, string codeEquipe)
		{
			var res = await _service.ObtenirEquipe(codeFiliere, codeEquipe);
			return res.ConvertToObjectResult();
		}

		// POST: api/Filieres/BIOV/Equipes
		[HttpPost]
		public async Task<ActionResult<Equipe>> PostEquipe(string codeFiliere, Equipe eq)
		{
			var res = await _service.AjouterEquipe(codeFiliere, eq);

			// Renvoie une réponse de code 201 avec l'en-tête 
			// "location: <url d'accès à l’équipe>" et un corps contenant l’équipe
			string uri = Url.Action(nameof(GetEquipe),
					new { codeFiliere = res.Data?.CodeFiliere, codeEquipe = res.Data?.Code }) ?? "";

			return res.ConvertToObjectResult(uri);
		}

		// POST: api/Filieres/BIOV/Equipes/BIOV_MKT
		[HttpPost("{codeEquipe}")]
		public async Task<ActionResult<Equipe>> PostPersonne(string codeFiliere, string codeEquipe, Personne pers)
		{
			var res = await _service.AjouterPersonne(codeEquipe, pers);

			// Renvoie une réponse de code 201 avec l'en-tête 
			// "location: <url d'accès à l’équipe de la personne>" et un corps contenant l’équipe
			string uri = Url.Action(nameof(GetEquipe), new { codeFiliere, codeEquipe }) ?? "";
			return res.ConvertToObjectResult(uri);
		}

		// PUT: api/Filieres/BIOV/Equipes/BIOV_MKT?manager=AFERRAND
		[HttpPut("{codeEquipe}")]
		public async Task<ActionResult<string>> PutPersonne(string codeEquipe, [FromQuery] string manager)
		{
			var res = await _service.ChangerManagerEquipe(codeEquipe, manager);
			return res.ConvertToObjectResult();
		}
	}
}
