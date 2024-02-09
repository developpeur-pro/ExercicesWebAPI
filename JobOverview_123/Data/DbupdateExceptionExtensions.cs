using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobOverview_v123.Data
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
			(HttpStatusCode StatusCode, string Message) err;

			if (ex is DbUpdateException && ex.InnerException is not SqlException)
				throw new NotImplementedException("Traduction des erreurs en réponses HTTP non implémentée pour ce SGBD");

			if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
				err = GetProblemDetailFromSqlException(sqlEx);
			else if (ex is SqlException sqlEx2)
				err = GetProblemDetailFromSqlException(sqlEx2);
			else
				err = (HttpStatusCode.BadRequest, ex.Message);

			return new ProblemDetails
			{
				Title = err.StatusCode.ToString(),
				Status = (int)err.StatusCode,
				Detail = err.Message
			};
		}

		private static (HttpStatusCode StatusCode, string Message) GetProblemDetailFromSqlException(SqlException sqlEx)
		{
			(HttpStatusCode StatusCode, string Message) err;

			switch (sqlEx.Number)
			{
				case NotNullError:
					err = (HttpStatusCode.BadRequest, "Impossible d'affecter la valeur Null à un champ non nullable.");
					break;

				case IdentityError:
					err = (HttpStatusCode.Conflict, "Impossible d'affecter une valeur à un identifiant auto-incrémenté.");
					break;

				case ForeignKeyError:
					err = (HttpStatusCode.BadRequest, "La requête fait référence à un enregistrement qui n'existe pas dans la base\n" +
						" ou bien tente de supprimer un enregistrement référencé ailleurs dans la base");
					break;

				case UniqueError:
				case PrimaryKeyError:
					err = (HttpStatusCode.Conflict, "Un enregistrement de même identifiant existe déjà dans la base.");
					break;

				case NumericValueError:
					err = (HttpStatusCode.BadRequest, "Une valeur numérique incorrecte a été fournie.");
					break;

				case MaxLengthError:
					err = (HttpStatusCode.BadRequest, "Une chaîne trop longue a été fournie.");
					break;

				default:
					err = (HttpStatusCode.InternalServerError, "Erreur non gérée à l'enregistrement dans la base de données.");
					break;
			}

			return err;
		}
	}
}
