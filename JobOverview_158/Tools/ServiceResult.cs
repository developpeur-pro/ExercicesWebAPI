namespace JobOverview_v158.Tools
{
    public enum ResultKinds
	{
		Ok,
		Created,
		InvalidData,
		NotFound,
		Conflict,
		Concurrency,
		Unexpected
	}

	/// <summary>
	/// Classe décrivant un résultat d'appel de méthode de service métier
	/// </summary>
	public class ServiceResult<T>
	{
		private const string ID = "Identifiant";

		// Type de résultat
		public ResultKinds ResultKind { get; private set; }

		// Données métier ou nombre d'enregistrements affectés
		public T Data { get; private set; } = default!;

		// Dictionnaire d'erreurs
		public ErrorsDictionary Errors { get; private set; } = new();

		#region Méthodes statiques pour créer facilement des résultats des différetns types

		/// <summary>
		/// Renvoie un résultat Ok contenant les données spécifiées
		/// </summary>
		/// <param name="data">Données métier ou nombre d'enregistrements affectés</param>
		/// <returns></returns>
		public static ServiceResult<T> CreateOkResult(T data)
		{
			return new ServiceResult<T>
			{
				ResultKind = ResultKinds.Ok,
				Data = data,
			};
		}

		/// <summary>
		/// Renvoie un résultat Created contenant les données créées
		/// </summary>
		/// <param name="data">Données créées</param>
		/// <returns></returns>
		public static ServiceResult<T> CreateCreatedResult(T data)
		{
			return new ServiceResult<T>
			{
				ResultKind = ResultKinds.Created,
				Data = data,
			};
		}

		/// <summary>
		/// Renvoie un résultat invalide avec le message d'erreur spécifié
		/// </summary>
		/// <param name="title">titre du message</param>
		/// <param name="message">message d'erreur</param>
		/// <returns></returns>
		public static ServiceResult<T> CreateInvalidDataResult(string title = "", string? message = null)
		{
			var res = new ServiceResult<T> { ResultKind = ResultKinds.InvalidData };
			if (message != null) res.Errors.Add(title, message);

			return res;
		}

		/// <summary>
		/// Renvoie un résultat invalide avec le dictionnaire d'erreurs spécifié
		/// </summary>
		/// <param name="errors">Dictionnaire d'erreurs</param>
		/// <returns></returns>
		public static ServiceResult<T> CreateInvalidDataResult(ErrorsDictionary errors)
		{
			return new ServiceResult<T>
			{
				ResultKind = ResultKinds.InvalidData,
				Errors = errors
			};
		}

		/// <summary>
		/// Renvoie un résultat NotFound avec un message d'erreur contenant l'éventuel id spécifié
		/// </summary>
		/// <param name="id">identifiant le l'enregistrement non trouvé</param>
		/// <returns></returns>
		public static ServiceResult<T> CreateNotFoundResult(object? id = null)
		{
			var res = new ServiceResult<T> { ResultKind = ResultKinds.NotFound };
			if (id != null)
				res.Errors.Add(ID, id.ToString() ?? "");

			return res;
		}

		/// <summary>
		/// Renvoie un résultat Conflict avec le message d'erreur spécifié
		/// </summary>
		/// <param name="title">titre du message</param>
		/// <param name="message">message d'erreur</param>
		/// <returns></returns>
		public static ServiceResult<T> CreateConflictResult(string title, string message)
		{
			var res = new ServiceResult<T> { ResultKind = ResultKinds.Conflict };
			res.Errors.Add(title, message);

			return res;
		}

		/// <summary>
		/// Renvoie un résultat Concurrency avec le message d'erreur spécifié
		/// </summary>
		/// <param name="title">titre du message</param>
		/// <param name="message">message d'erreur</param>
		/// <returns></returns>
		public static ServiceResult<T> CreateConcurrencyResult(string title, string message)
		{
			var res = new ServiceResult<T> { ResultKind = ResultKinds.Concurrency };
			res.Errors.Add(title, message);

			return res;
		}

		/// <summary>
		/// Renvoie un résultat Unexpected avec le message d'erreur spécifié
		/// </summary>
		/// <param name="message">message d'erreur</param>
		/// <param name="title">titre du message</param>
		/// <returns></returns>
		public static ServiceResult<T> CreateUnexpectedResult(string title = "", string? message = null)
		{
			var res = new ServiceResult<T> { ResultKind = ResultKinds.Unexpected };
			res.Errors.Add(title, message??"");
			return res;
		}

		#endregion
	}
}
