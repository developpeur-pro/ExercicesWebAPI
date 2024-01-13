using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorWasm.Backend.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class InfosController : ControllerBase
	{
		[HttpGet("EncodedToken")]
		public async Task<ActionResult<string?>> GetJetonEncodé()
		{
			UserTokenRequestParameters? p = null;
			UserToken jeton = await HttpContext.GetUserAccessTokenAsync(p);
			return jeton.AccessToken;
		}

		[HttpGet("DecodedToken")]
		public async Task<ActionResult<string>> GetJetonDécodé()
		{
			// Récupère le jeton d'accès de l'utilisateur courant
			UserTokenRequestParameters? p = null;
			UserToken jeton = await HttpContext.GetUserAccessTokenAsync(p);

			string jetonDécodé = string.Empty;
			// Décode le jeton et renvoie le résultat sous forme de texte JSON indenté
			if (!string.IsNullOrEmpty(jeton.AccessToken))
			{
				var jwt = new JwtSecurityToken(jeton.AccessToken);
				var doc = JsonDocument.Parse(jwt.Payload.SerializeToJson());
				jetonDécodé = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
			}

			return jetonDécodé;
		}
	}
}
