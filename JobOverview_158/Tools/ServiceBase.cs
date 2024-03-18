using Microsoft.EntityFrameworkCore;
using JobOverview_v158.Data;

namespace JobOverview_v158.Tools
{
	public class ServiceBase
	{
		private readonly DbContext _dbContext;

		public ServiceBase(DbContext context)
		{
			_dbContext = context;
		}

		/// <summary>
		/// Renvoie un résultat de type Ok contenant la donnée souhaitée
		/// </summary>
		/// <typeparam name="T">Type de donnée du résultat</typeparam>
		/// <param name="data">Donnée à stocker dans le résultat</param>
		/// <returns></returns>
		public ServiceResult<T> ResultOk<T>(T data) =>
			 ServiceResult<T>.CreateOkResult(data);

		/// <summary>
		/// Renvoie un résultat de type NotFound
		/// </summary>
		/// <typeparam name="T">Type de la donnée non trouvée</typeparam>
		/// <param name="id">Id de la donnée non trouvée</param>
		/// <returns></returns>
		public ServiceResult<T?> ResultNotFound<T>(object id) =>
			 ServiceResult<T?>.CreateNotFoundResult(id);


		/// <summary>
		/// Renvoie un résultat de type Ok contenant la donnée souhaitée
		/// ou un résultat de type NotFound contenant l'id de la donnée si celle-ci vaut null
		/// </summary>
		/// <typeparam name="T">Type de donnée du résultat</typeparam>
		/// <param name="id">Id de la donnée</param>
		/// <param name="data">Donnée à stocker dans le résultat</param>
		/// <returns></returns>
		public ServiceResult<T?> ResultOkOrNotFound<T>(object id, T data)
		{
			if (data == null) return ResultNotFound<T>(id);

			return ResultOk<T?>(data);
		}

		/// <summary>
		/// Renvoie un résultat de type BadRequest avec le détail des erreurs.
		/// Les erreurs peuvent être liées à des règles de validation ou des contraintes d'intégrité.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="errors">Erreurs</param>
		/// <returns></returns>
		public ServiceResult<T?> ResultInvalidData<T>(ErrorsDictionary errors) =>
			 ServiceResult<T?>.CreateInvalidDataResult(errors);

		/// <summary>
		/// Renvoie un résultat de type BadRequest avec le détail de l'erreur, pour une erreur unique.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="title">titre</param>
		/// <param name="message">message d'erreur</param>
		/// <returns></returns>
		public ServiceResult<T?> ResultInvalidData<T>(string title = "", string? message = null) =>
			 ServiceResult<T?>.CreateInvalidDataResult(title, message);

		/// <summary>
		/// Enregistre les modifications du DbContext et renvoie un résultat Created
		/// contenant les données passées en paramètre, ou bien un résutat d'erreur 
		/// </summary>
		/// <typeparam name="T">Type des données du résultat</typeparam>
		/// <param name="data">Données à stocker dans le résultat</param>
		/// <returns></returns>
		public async Task<ServiceResult<T?>> SaveAndResultCreatedAsync<T>(T data)
		{
			try
			{
				await _dbContext.SaveChangesAsync();
				return ServiceResult<T?>.CreateCreatedResult(data);
			}
			catch (Exception e)
			{
				return e.ConvertToServiceResult<T?>();
			}
		}

		/// <summary>
		/// Enregistre les modifications du DbContext et renvoie un résultat Ok
		/// contenant les données passées en paramètre, ou bien un résutat d'erreur 
		/// </summary>
		/// <typeparam name="T">Type des données du résultat</typeparam>
		/// <param name="data">Données à stocker dans le résultat</param>
		/// <returns></returns>
		public async Task<ServiceResult<T?>> SaveAndResultOkAsync<T>(T data)
		{
			try
			{
				await _dbContext.SaveChangesAsync();
				return ServiceResult<T?>.CreateOkResult(data);
			}
			catch (Exception e)
			{
				return e.ConvertToServiceResult<T?>();
			}
		}

		/// <summary>
		/// Enregistre les modifications du DbContext et renvoie un résultat Ok
		/// contenant le nombre de modifications enregistrées, ou bien un résutat d'erreur 
		/// </summary>
		/// <returns></returns>
		public async Task<ServiceResult<int>> SaveAndResultOkAsync()
		{
			try
			{
				int cnt = await _dbContext.SaveChangesAsync();
				return ServiceResult<int>.CreateOkResult(cnt);
			}
			catch (Exception e)
			{
				return e.ConvertToServiceResult<int>();
			}
		}
	}
}
