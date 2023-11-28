using JobOverview_v83.Entities;
using JobOverview_v83.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobOverview_v83.Controllers
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
			List<Tache> taches = await _serviceTaches.ObtenirTaches(personne, logiciel, version);

			return Ok(taches);
		}

		// GET: api/Taches/5
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

		// POST: api/Taches
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Tache>> PostTache(Tache tache)
		{
			try
			{
				// Enregistre la tâche dans la base et le récupère avec son Id généré automatiquement
				Tache res = await _serviceTaches.AjouterTache(tache);
				return CreatedAtAction(nameof(GetTache), new { res.Id }, res);
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// POST: api/Taches/5/Travaux
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
	}
}
