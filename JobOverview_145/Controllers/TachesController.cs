using Asp.Versioning;
using JobOverview_v145.Entities;
using JobOverview_v145.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobOverview_v145.Controllers
{
	[Route("api/[controller]")]
	[ApiVersion(1.0)]
	[ApiVersion(2.0)]
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
				Personne? pers = await _serviceTaches.ObtenirPersonne(personne);
				if (pers == null || pers.Manager != userId)
					return Forbid();
			}

			List<Tache> taches = await _serviceTaches.ObtenirTaches(personne, logiciel, version);
			return Ok(taches);
		}

		// GET: api/Taches/45
		[HttpGet("{id}")]
		public async Task<ActionResult<Tache>> GetTache(int id)
		{
			Tache? tache = await _serviceTaches.ObtenirTache(id);
			if (tache == null) return NotFound();

			return Ok(tache);
		}

		// GET: api/Personnes/RBEAUMONT
		[HttpGet("/api/Personnes/{pseudo}")]
		public async Task<ActionResult<Personne>> GetPersonne(string pseudo)
		{
			var pers = await _serviceTaches.ObtenirPersonne(pseudo);

			if (pers == null) return NotFound($"La personne {pseudo} n'existe pas");

			return Ok(pers);
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
			try
			{
				// Modifie la tâche si elle existe ou bien la crée
				// et récupère avec son Id généré automatiquement
				Tache res = await _serviceTaches.ModifierAjouterTache(tache);

				// Renvoie la réponse appropriée selon que la tâche a été créée ou mise à jour
				if (tache.Id == 0)
					return CreatedAtAction(nameof(GetTache), new { res.Id }, res);
				else
					return Ok(res);
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// POST: api/Taches/45/Travaux
		[HttpPost("{idTache}/Travaux")]
		public async Task<IActionResult> PostTravail([FromRoute] int idTache, Travail travail)
		{
			try
			{
				Travail res = await _serviceTaches.AjouterTravail(idTache, travail);
				return CreatedAtAction(nameof(GetTache), new { id = res.IdTache }, res);
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// DELETE: api/Taches/45/Travaux/2023-11-23
		[HttpDelete("{idTache}/Travaux/{date}")]
		public async Task<IActionResult> DeleteTravail(int idTache, DateTime date)
		{
			try
			{
				await _serviceTaches.SupprimerTravail(idTache, date);
				return NoContent();
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// DELETE: api/Taches?personne=RBEAUMONT&logiciel=ANATOMIA&version=6
		[HttpDelete]
		[Authorize(Policy = "GérerTaches")]
		public async Task<IActionResult> DeleteTaches(
			[FromQuery] string? personne, [FromQuery] string? logiciel, [FromQuery] float? version)
		{
			try
			{
				int nbSuppr = await _serviceTaches.SupprimerTaches(personne, logiciel, version);
				return Ok(nbSuppr + " tâches supprimées");
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

	}
}
