using System.Collections;

namespace JobOverview_v158.Tools
{
	/// <summary>
	/// Modélise un dictionnaire d'erreurs
	/// </summary>
	public class ErrorsDictionary : IEnumerable<KeyValuePair<string, string[]>>
	{
		private Dictionary<string, string[]> _dic = new();

		/// <summary>
		/// Ajoute une erreur dans le tableau associé à une clé donnée du dictionnaire
		/// Si la clé spécifiée n'existe pas, crée une nouvelle entrée de dictionnaire
		/// </summary>
		/// <param name="key">clé à rechercher ou à créer</param>
		/// <param name="message">item à ajouter dans le tableau d'erreurs associé à cette clé</param>
		public void Add(string key, string message)
		{
			if (_dic.ContainsKey(key))
				_dic[key] = _dic[key].Append(message).ToArray();
			else
				_dic.Add(key, new string[] { message });
		}

		/// <summary>
		/// Fusionne le dictionnaire avec celui passé en paramètre.
		/// Pour les clés communes, fusionne les listes d'erreurs associées.
		/// </summary>
		/// <param name="otherDic">Dictionnaire à fusionner</param>
		public void Merge(ErrorsDictionary otherDic)
		{
			foreach (var item in otherDic)
			{
				if (!_dic.TryAdd(item.Key, item.Value))
				{
					_dic[item.Key] = _dic[item.Key].Union(item.Value).ToArray();
				}
			}
		}

		/// <summary>
		/// Obtient le tableau d'erreurs associé à une clé donnée
		/// </summary>
		/// <param name="key">Clé recherchée</param>
		/// <param name="errors">Tableau d'erreurs associé à cette clé</param>
		/// <returns>true si la clé existe, false sinon</returns>
		public bool TryGetErrors(string key, out string[]? errors) =>
			 _dic.TryGetValue(key, out errors);

		// Enumérateurs (pour parcours avec foreach)
		IEnumerator IEnumerable.GetEnumerator() => _dic.GetEnumerator();
		IEnumerator<KeyValuePair<string, string[]>> IEnumerable<KeyValuePair<string, string[]>>.GetEnumerator() => _dic.GetEnumerator();

		/// <summary>
		/// Renvoie vrai si le dictionnaire contient au moins une entrée
		/// </summary>
		/// <returns></returns>
		public bool Any() => _dic.Any();

		/// <summary>
		/// Renvoie le nombre total d'erreurs dans le dictionnaire, toutes entrées confondues
		/// </summary>
		public int Count
		{
			get
			{
				int cnt = 0;
				foreach (var item in _dic)
				{
					cnt += item.Value.Length;
				}
				return cnt;
			}
		}

		/// <summary>
		/// Renvoie le contenu du dictionnaire sous forme de texte indenté
		/// </summary>
		public override string ToString()
		{
			StringWriter sw = new();
			foreach (string key in _dic.Keys)
			{
				// S'il n'y a qu'une erreur pour cette clé, on l'écrit sur la même ligne
				if (_dic[key].Length == 1)
					sw.WriteLine(key + " : " + _dic[key][0]);

				// Sinon, on écrit une erreur par ligne
				else
				{
					sw.WriteLine(key + " :");
					foreach (string val in _dic[key])
						sw.WriteLine("\t" + val);
				}
			}
			return sw.ToString();
		}
	}
}
