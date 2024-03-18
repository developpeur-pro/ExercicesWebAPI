using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobOverview_v154.Data
{
	public static class ExceptionExtensions
	{
		private const int NotNullError = 515;
		private const int IdentityError = 544;
		private const int ForeignKeyError = 547;
		private const int UniqueError = 2601;
		private const int PrimaryKeyError = 2627;
		private const int MaxLengthError = 2628;
		private const int NumericValueError = 8115;

		// Traduit une Exception en un objet de type ProblemDetails
		// utilisable pour construire une réponse HTTP
		public static ProblemDetails ConvertToProblemDetails(this Exception ex)
		{
			if (ex is DbUpdateConcurrencyException)
			{
				return new ProblemDetails
				{
					Title = "Accès concurrentiel",
					Status = (int)HttpStatusCode.Conflict,
					Detail = "Cet enregistrement a été modifié en base par un autre utilisateur depuis son obtention"
				};
			}

			if (ex is DbUpdateException)
			{
				if (ex.InnerException is SqlException sqlEx)
					return sqlEx.ConvertToProblemDetails();
				else
					throw new NotImplementedException("Traduction des erreurs en réponses HTTP non implémentée pour ce SGBD");
			}

			if (ex is SqlException sqlEx2)
				return sqlEx2.ConvertToProblemDetails();

			if (ex is JsonPatchException)
			{
				return new ProblemDetails
				{
					Title = "PATCH non appliqué",
					Status = (int)HttpStatusCode.BadRequest,
					Detail = "Le patch contient des opérations non prises en charge pour cette ressource."
				};
			}

			throw ex;
		}

		private static ProblemDetails ConvertToProblemDetails(this SqlException sqlEx)
		{
			var err = new ProblemDetails
			{
				Title = HttpStatusCode.BadRequest.ToString(),
				Status = (int)HttpStatusCode.BadRequest,
			};

			switch (sqlEx.Number)
			{
				case NotNullError:
					err.Detail = "Impossible d'affecter la valeur Null à un champ non nullable.";
					break;

				case IdentityError:
					err.Detail = "Impossible d'affecter une valeur à un identifiant auto-incrémenté.";
					break;

				case ForeignKeyError:
					err.Detail = "La requête fait référence à un enregistrement qui n'existe pas dans la base\n" +
						" ou bien tente de supprimer un enregistrement référencé ailleurs dans la base";
					break;

				case UniqueError:
				case PrimaryKeyError:
					err.Title = HttpStatusCode.Conflict.ToString();
					err.Status = (int)HttpStatusCode.Conflict;
					err.Detail = "Un enregistrement de même identifiant existe déjà dans la base.";
					break;

				case NumericValueError:
					err.Detail = "Une valeur numérique incorrecte a été fournie.";
					break;

				case MaxLengthError:
					err.Detail = "Une chaîne trop longue a été fournie.";
					break;

				default:
					err.Title = HttpStatusCode.InternalServerError.ToString();
					err.Status = (int)HttpStatusCode.InternalServerError;
					err.Detail = "Erreur non gérée à l'enregistrement dans la base de données.";
					break;
			}

			return err;
		}
	}
}
