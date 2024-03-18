using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JobOverview_v158.Tools
{
	public static class ServiceResultExtensions
	{
		/// <summary>
		/// Traduit un résultat de méthode de service métier en réponse HTTP, et la renvoie
		/// </summary>
		/// <typeparam name="T">type de la donnée métier traitée</typeparam>
		/// <param name="res">résultat métier</param>
		/// <param name="uri">uri d'obtention de la donnée créée</param>
		/// <returns>ObjectResult représentant la réponse HTTP</returns>
		/// <exception cref="InvalidOperationException">Si la réponse de service est d'un type innatendu</exception>
		public static ObjectResult ConvertToObjectResult<T>(this ServiceResult<T> res, string? uri = null)
		{
			switch (res.ResultKind)
			{
				case ResultKinds.Ok:
					return new OkObjectResult(res.Data);

				case ResultKinds.Created:
					if (uri != null)
						return new CreatedResult(uri, res.Data);
					else
						return new CreatedResult();

				case ResultKinds.InvalidData:
					return new ObjectResult(new ValidationProblemDetails
					{
						Title = "Données non valides",
						Status = (int)HttpStatusCode.BadRequest,
						Errors = res.Errors.ToDictionary(),
					});

				case ResultKinds.NotFound:
					return new ObjectResult(new ProblemDetails
					{
						Title = "Enregistrement non trouvé",
						Status = (int)HttpStatusCode.NotFound,
						Detail = res.Errors.ToString()
					});

				case ResultKinds.Conflict:
					return new ObjectResult(new ProblemDetails
					{
						Title = "Enregistrement déjà existant",
						Status = (int)HttpStatusCode.Conflict,
						Detail = res.Errors.ToString()
					});

				case ResultKinds.Concurrency:
					return new ObjectResult(new ProblemDetails
					{
						Title = "Enregistrement modifié par un autre utilisateur",
						Status = (int)HttpStatusCode.Conflict,
						Detail = res.Errors.ToString()
					});

				// Pour la valeur ResultKinds.Unexpected ou autre
				default:
					throw new InvalidOperationException("Résultat innatendu." + res.Errors);
			}
		}
	}
}
