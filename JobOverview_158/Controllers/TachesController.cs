using JobOverview_v158.Entities;
using JobOverview_v158.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using JobOverview_v158.Tools;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JobOverview_v158.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TachesController : ControllerBase
	{
		private readonly IServiceTaches _serviceTaches;

		public TachesController(IServiceTaches serviceTaches)
		{
			_serviceTaches = serviceTaches;
		}

		// GET: api/Taches?personne=x&Logiciel=y&version=z
		/// <summary>Liste de tâches filtrable</summary>
		/// <remarks>
		/// Obtient les tâches de l'utilisateur courant si aucun filtre de personne n'est appliqué,  
		/// ou bien les tâches de la personne définie en filtre, si l'utilisateur courant est son manager.  
		/// 
		/// La liste est filtrable sur un logiciel et une version de logiciel.
		/// </remarks>
		/// <param name="personne">Pseudo de la personne dont on souhaite obtenir les tâches</param>
		/// <param name="logiciel">Code du logciel concerné</param>
		/// <param name="version">N° de version du logiciel</param>
		/// <response code="200">Liste des tâches</response>
		/// <response code="403">L'utilisateur courant n'est pas autorisé à obtenir la liste des tâches, car il n'est pas le manager de la personne spécifiée</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<IEnumerable<Tache>>> GetTaches(
			[FromQuery] string? personne, [FromQuery] string? logiciel, [FromQuery] float? version)
		{
			string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Si aucun filtre sur la personne, on filtre sur l'utilisateur courant
			if (string.IsNullOrEmpty(personne))
			{
				personne = userId;
			}
			// Sinon, on vérifie que l'utilisateur courant est le manager de la personne
			// Si ce n'est pas le cas, on renvoie une réponse de code 403
			else
			{
				var resp = await _serviceTaches.ObtenirPersonne(personne);
				if (resp.Data == null || resp.Data.Manager != userId)
					return Forbid();
			}

			var res = await _serviceTaches.ObtenirTaches(personne, logiciel, version);
			return res.ConvertToObjectResult();
		}

		// GET: api/Taches/45
		[HttpGet("{id}")]
		public async Task<ActionResult<Tache>> GetTache(int id)
		{
			var res = await _serviceTaches.ObtenirTache(id);
			return res.ConvertToObjectResult();
		}

		// GET: api/Personnes/RBEAUMONT
		[HttpGet("/api/Personnes/{pseudo}")]
		public async Task<ActionResult<Personne>> GetPersonne(string pseudo)
		{
			var res = await _serviceTaches.ObtenirPersonne(pseudo);
			return res.ConvertToObjectResult();
		}

		// PUT: api/Taches
		/// <summary>Crée ou modifie une tâche</summary>
		/// <remarks>
		/// Si la tâche passée en paramètre à un id = 0, ajoute cette tâche dans la base,  
		/// sinon, modifie la tâche existante en base 
		/// </remarks>
		/// <param name="tache">Tâche à créer ou à modifier</param>
		/// <response code="200">Renvoie la tâche modifiée</response>
		/// <response code="201">Renvoie la tâche créée</response>
		/// <response code="400">Erreur si la personne spécifiée n'existe pas, ou si l'activité de la tâche ne fait pas partie de celles de la personne</response>
		/// <response code="403">L'utilisateur courant n'est pas autorisé à créer ou modifier des tâches</response>
		[HttpPut]
		[Authorize(Policy = "GérerTaches")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<Tache>> PutTache(Tache tache)
		{
			// Modifie la tâche si elle existe ou bien la crée
			// et récupère avec son Id généré automatiquement
			var res = await _serviceTaches.ModifierAjouterTache(tache);

			string uri = Url.Action(nameof(GetTache), new { res.Data?.Id }) ?? "";
			return res.ConvertToObjectResult(uri);
		}

		// POST: api/Taches/45/Travaux
		[HttpPost("{idTache}/Travaux")]
		public async Task<IActionResult> PostTravail([FromRoute] int idTache, Travail travail)
		{

			var res = await _serviceTaches.AjouterTravail(idTache, travail);

			string uri = Url.Action(nameof(GetTache), new { id = res.Data?.IdTache }) ?? "";
			return res.ConvertToObjectResult();
		}

		// DELETE: api/Taches/45/Travaux/2023-11-23
		[HttpDelete("{idTache}/Travaux/{date}")]
		public async Task<IActionResult> DeleteTravail(int idTache, DateTime date)
		{
			var res = await _serviceTaches.SupprimerTravail(idTache, date);
			return res.ConvertToObjectResult();
		}

		// DELETE: api/Taches?personne=RBEAUMONT&logiciel=ANATOMIA&version=6
		[HttpDelete]
		[Authorize(Policy = "GérerTaches")]
		public async Task<IActionResult> DeleteTaches(
			[FromQuery] string? personne, [FromQuery] string? logiciel, [FromQuery] float? version)
		{
			var res = await _serviceTaches.SupprimerTaches(personne, logiciel, version);
			return res.ConvertToObjectResult();
		}
	}
}
