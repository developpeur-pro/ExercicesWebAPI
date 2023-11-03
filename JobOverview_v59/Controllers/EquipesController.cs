using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobOverview_v59.Entities;
using JobOverview_v59.Services;

namespace JobOverview_v59.Controllers
{
	[Route("api/Filieres/{codeFiliere}/[controller]")]
	[ApiController]
	public class EquipesController : ControllerBase
	{
		private readonly IServiceEquipes _service;

		public EquipesController(IServiceEquipes service)
		{
			_service = service;
		}

		// GET: api/Filieres/BIOH/Equipes
		[HttpGet]
		public async Task<ActionResult<List<Equipe>>> GetEquipes(string codeFiliere)
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
	}
}
