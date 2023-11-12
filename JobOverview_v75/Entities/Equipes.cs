namespace JobOverview_v75.Entities
{
	public class Service
	{
		public string Code { get; set; } = string.Empty;
		public string Nom { get; set; } = string.Empty;
	}

	public class Equipe
	{
		public string Code { get; set; } = string.Empty;
		public string Nom { get; set; } = string.Empty;
		public string CodeService { get; set; } = string.Empty;
		public string CodeFiliere { get; set; } = string.Empty;

		// Propriétés de navigation
		public virtual List<Personne> Personnes { get; set; } = new();
		public virtual Service Service { get; set; } = new();
	}

	public class Personne
	{
		public string Pseudo { get; set; } = string.Empty;
		public string Nom { get; set; } = string.Empty;
		public string Prenom { get; set; } = string.Empty;
		public decimal TauxProductivite { get; set; }

		public string CodeEquipe { get; set; } = string.Empty;
		public string CodeMetier { get; set; } = string.Empty;
		public string? Manager { get; set; }

		// Propriétés de navigation
		public virtual Metier Métier { get; set; } = new();
	}

	public class Metier
	{
		public string Code { get; set; } = string.Empty;
		public string Titre { get; set; } = string.Empty;
		public string CodeService { get; set; } = string.Empty;
	}
}
