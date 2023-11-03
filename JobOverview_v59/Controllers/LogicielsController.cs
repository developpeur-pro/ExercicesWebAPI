using JobOverview_v59.Entities;
using JobOverview_v59.Services;
using Microsoft.AspNetCore.Mvc;
using Version = JobOverview_v59.Entities.Version;

namespace JobOverview_v59.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LogicielsController : ControllerBase
	{
		private readonly IServiceLogiciels _serviceLogi;

		public LogicielsController(IServiceLogiciels service)
		{
			_serviceLogi = service;
		}

		// GET: api/Logiciels
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Logiciel>>> GetLogiciels([FromQuery] string codeFiliere)
		{
			var logiciels = await _serviceLogi.ObtenirLogiciels(codeFiliere);
			return Ok(logiciels);
		}

		// GET: api/Logiciels/ABC
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

		// GET: Logiciels/GENOMICA/versions?millesime=2023
		[HttpGet("{codeLogiciel}/versions")]
		public async Task<ActionResult<IEnumerable<Version>>> GetVersions(string codeLogiciel, [FromQuery] int? millesime)
		{
			var versions = await _serviceLogi.ObtenirVersionsLogiciel(codeLogiciel, millesime);

			if (versions == null) return NotFound();

			return Ok(versions);
		}
	}
}
