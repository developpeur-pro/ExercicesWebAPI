using JobOverview_v158.Entities;
using JobOverview_v158.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using JobOverview_v158.Tools;
using Version = JobOverview_v158.Entities.Version;

namespace JobOverview_v158.Controllers
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
			var res = await _serviceLogi.ObtenirLogiciel(code);
			return res.ConvertToObjectResult();
		}

		// GET: api/Logiciels/GENOMICA/Versions?millesime=2023
		[HttpGet("{codeLogiciel}/versions")]
		public async Task<ActionResult<IEnumerable<Version>>> GetVersions(string codeLogiciel, [FromQuery] int? millesime)
		{
			var res = await _serviceLogi.ObtenirVersionsLogiciel(codeLogiciel, millesime);
			return res.ConvertToObjectResult();
		}

		// POST: api/Logiciels/GENOMICA/Versions
		[HttpPost("{codeLogiciel}/versions")]
		public async Task<ActionResult<Version>> PostVersion(string codeLogiciel, Version vers)
		{
			var res = await _serviceLogi.AjouterVersion(codeLogiciel, vers);
			string uri = Url.Action(nameof(GetVersions), new { codeLogiciel, res.Data?.Millesime }) ?? "";
			return res.ConvertToObjectResult(uri);

		}

		// PATCH: api/Logiciels/GENOMICA/Versions/6
		[HttpPatch("{codeLogiciel}/versions/{numVersion}")]
		public async Task<ActionResult<Version>> PatchVersion(string codeLogiciel,
						float numVersion, JsonPatchDocument<Version> patch)
		{
			var res = await _serviceLogi.ModifierVersion(codeLogiciel, numVersion, patch);
			return res.ConvertToObjectResult();
		}

		// GET : api/Logiciels/GENOMICA/Versions/1.00/Releases/30
		[HttpGet("{codeLogiciel}/Versions/{numVersion}/Releases/{numRelease}")]
		public async Task<ActionResult<IEnumerable<Version>>> GetRelease(string codeLogiciel, float numVersion, short numRelease)
		{
			var res = await _serviceLogi.ObtenirRelease(codeLogiciel, numVersion, numRelease);
			return res.ConvertToObjectResult();
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

			var res = await _serviceLogi.AjouterRelease(codeLogiciel, numVersion, rel);

			object clé = new {
				codeLogiciel = res.Data?.CodeLogiciel,
				numVersion = res.Data?.NumeroVersion,
				numRelease = res.Data?.Numero
			};
			string uri = Url.Action(nameof(GetRelease), clé) ?? "";

			return res.ConvertToObjectResult(uri);
		}
	}
}
