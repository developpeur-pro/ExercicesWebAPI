using JobOverview_v145.Entities;

namespace JobOverview_v145.V1.Entities
{
	public class EquipeDTO
	{
		public string Code { get; set; } = string.Empty;
		public string Nom { get; set; } = string.Empty;
		public string CodeService { get; set; } = string.Empty;
		public string CodeFiliere { get; set; } = string.Empty;

		// Propriétés de navigation
		public virtual List<PersonneDTO> Personnes { get; set; } = new();
		public virtual Service Service { get; set; } = new();
	}

	public class PersonneDTO
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
}