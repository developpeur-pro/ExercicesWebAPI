using JobOverview_v100.Entities;
using JobOverview_v100.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JobOverview_v100.Controllers
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
		[HttpGet]
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
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut]
		[Authorize(Policy = "GérerTaches")]
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
