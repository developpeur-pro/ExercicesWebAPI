using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JobOverview_v158.Tools
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

        // Traduit une Exception en objet de type ServiceResult<T>
        public static ServiceResult<T> ConvertToServiceResult<T>(this Exception ex)
        {
            if (ex is DbUpdateConcurrencyException ce)
                return ServiceResult<T>.CreateConcurrencyResult("Accès concurrentiel",
                   "L'enregistement a été modifié par un autre utilisateur depuis sa récupération");

            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
                return sqlEx.ConvertToServiceResult<T>();

            if (ex is SqlException sqlEx2) // Emise par ExecuteUpdate et ExecuteDelete
                return sqlEx2.ConvertToServiceResult<T>();

            if (ex is JsonPatchException)
                return ServiceResult<T>.CreateInvalidDataResult("PATCH non appliqué",
                    "Le patch contient des opérations non prises en charge pour cette ressource.");

            if (ex is DbUpdateException && ex.InnerException is not SqlException)
                throw new NotImplementedException("Traduction des erreurs en résultats de service non implémentée pour ce SGBD");

            throw ex;
        }

        // Traduit une SqlException en objet de type ServiceResult<T>
        public static ServiceResult<T> ConvertToServiceResult<T>(this SqlException sqlEx)
        {
            switch (sqlEx.Number)
            {
                case NotNullError:
                    return ServiceResult<T>.CreateInvalidDataResult("Contrainte NotNull",
                        "Impossible d'affecter la valeur Null à un champ non nullable.");

                case IdentityError:
                    return ServiceResult<T>.CreateInvalidDataResult("Contrainte Identity",
								"Impossible d'affecter explicitement une valeur à un identifiant auto-incrémenté.");

                case ForeignKeyError:
                    return ServiceResult<T>.CreateInvalidDataResult("Contrainte ForeignKey",
                       "La requête fait référence à un enregistrement qui n'existe pas dans la base\n" +
                         " ou bien tente de supprimer un enregistrement référencé ailleurs dans la base");

                case UniqueError:
                case PrimaryKeyError:
                    return ServiceResult<T>.CreateConflictResult("Contrainte Unique",
							  "Un enregistrement de même identifiant existe déjà.");

                case NumericValueError:
                    return ServiceResult<T>.CreateInvalidDataResult("Type numérique",
                        "Une valeur numérique incorrecte a été fournie.");

                case MaxLengthError:
                    return ServiceResult<T>.CreateInvalidDataResult("Contrainte MaxLength",
                        "Une chaîne trop longue a été fournie.");

                default:
                    return ServiceResult<T>.CreateUnexpectedResult("Erreur SQL Server",
                        "Erreur non gérée à l'enregistrement dans la base de données.");
            }
        }
    }
}
