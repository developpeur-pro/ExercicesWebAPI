using JobOverview_v94.Entities;
using JobOverview_v94.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Version = JobOverview_v94.Entities.Version;

namespace JobOverview_v94.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LogicielsController : ControllerBase
	{
		private readonly IServiceLogiciels _serviceLogi;
		private readonly ILogger<LogicielsController> _logger;

		public LogicielsController(IServiceLogiciels service, ILogger<LogicielsController> logger)
		{
			_serviceLogi = service;
			_logger = logger;
		}

		// GET: api/Logiciels
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Logiciel>>> GetLogiciels([FromQuery] string codeFiliere)
		{
			var logiciels = await _serviceLogi.ObtenirLogiciels(codeFiliere);
			return Ok(logiciels);
		}

		// GET: api/Logiciels/GENOMICA
		[HttpGet("{code}")]
		public async Task<ActionResult<Logiciel>> GetLogiciel(string code)
		{
			var logiciel = await _serviceLogi.ObtenirLogiciel(code);

			if (logiciel == null)
			{
				return NotFound();
			}

			return logiciel;
		}

		// GET: api/Logiciels/GENOMICA/Versions?millesime=2023
		[HttpGet("{codeLogiciel}/versions")]
		public async Task<ActionResult<IEnumerable<Version>>> GetVersions(string codeLogiciel, [FromQuery] int? millesime)
		{
			var versions = await _serviceLogi.ObtenirVersionsLogiciel(codeLogiciel, millesime);

			if (versions == null) return NotFound();

			return Ok(versions);
		}

		// POST: api/Logiciels/GENOMICA/Versions
		[HttpPost("{codeLogiciel}/versions")]
		public async Task<ActionResult<Version>> PostVersion(string codeLogiciel, Version vers)
		{
			try
			{
				Version res = await _serviceLogi.AjouterVersion(codeLogiciel, vers);
				return CreatedAtAction(nameof(GetVersions), new { codeLogiciel, res.Millesime }, res);
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// PATCH: api/Logiciels/GENOMICA/Versions/6
		[HttpPatch("{codeLogiciel}/versions/{numVersion}")]
		public async Task<ActionResult<Version>> PatchVersion(string codeLogiciel,
						float numVersion, JsonPatchDocument<Version> patch)
		{
			try
			{
				int nbMaj = await _serviceLogi.ModifierVersion(codeLogiciel, numVersion, patch);
				if (nbMaj == 0) return BadRequest($"Aucune mise à jour réalisée");
				return NoContent();
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// GET : api/Logiciels/GENOMICA/Versions/1.00/Releases/30
		[HttpGet("{codeLogiciel}/Versions/{numVersion}/Releases/{numRelease}")]
		public async Task<ActionResult<IEnumerable<Version>>> GetRelease(string codeLogiciel, float numVersion, short numRelease)
		{
			var release = await _serviceLogi.ObtenirRelease(codeLogiciel, numVersion, numRelease);

			if (release == null) return NotFound();

			return Ok(release);
		}

		// POST : api/Logiciels/GENOMICA/Versions/1.00/Releases
		[HttpPost("{codeLogiciel}/Versions/{numVersion}/Releases")]
		public async Task<ActionResult<Release>> PostRelease(string codeLogiciel, float numVersion, [FromForm] FormRelease fr)
		{
			// Crée une entité du modèle à partir de l'entité DTO
			Release rel = new Release()
			{
				CodeLogiciel = codeLogiciel,
				NumeroVersion = numVersion,
				Numero = fr.Numero,
				DatePubli = fr.DatePubli
			};

			if (fr.Notes != null)
			{
				using StreamReader reader = new(fr.Notes.OpenReadStream());
				rel.Notes = await reader.ReadToEndAsync();
			}

			try
			{
				Release res = await _serviceLogi.AjouterRelease(codeLogiciel, numVersion, rel);

				object clé = new { codeLogiciel = res.CodeLogiciel, numVersion = res.NumeroVersion, numRelease = res.Numero };
				string uri = Url.Action(nameof(GetRelease), clé) ?? "";
				return Created(uri, res);
			}
			catch (Exception e)
			{
				// Journalise des détails sur l'erreur et renvoie la réponse HTTP 
				return this.CustomResponseForError(e, rel, _logger);
			}
		}
	}
}
