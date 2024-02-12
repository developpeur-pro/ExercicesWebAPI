namespace JobOverview_v128.Entities
{
	public class Filiere
	{
		public string Code { get; set; } = "";
		public string Nom { get; set; } = "";

		// Propriété de navigation
		public virtual List<Equipe> Equipes { get; set; } = new();
	}

	public class Logiciel
	{
		public string Code { get; set; } = "";
		public string CodeFiliere { get; set; } = "";
		public string Nom { get; set; } = "";

		// Propriétés de navigation
		public virtual List<Module> Modules { get; set; } = new();
	}

	public class Module
	{
		public string Code { get; set; } = "";
		public string CodeLogiciel { get; set; } = "";
		public string Nom { get; set; } = "";

		public string? CodeModuleParent { get; set; }
		public string? CodeLogicielParent { get; set; }

		// Propriété de navigation
		public virtual List<Module> SousModules { get; set; } = new();
	}

	public class Version
	{
		public float Numero { get; set; }
		public string CodeLogiciel { get; set; } = "";
		public short Millesime { get; set; }
		public DateTime DateOuverture { get; set; }
		public DateTime DateSortiePrevue { get; set; }
		public DateTime? DateSortieReelle { get; set; }
		public string? Notes { get; set; }

		// Propriété de navigation
		public virtual List<Release> Releases { get; set; } = new();
	}

	public class Release
	{
		public short Numero { get; set; }
		public float NumeroVersion { get; set; }
		public string CodeLogiciel { get; set; } = "";
		public DateTime DatePubli { get; set; }
		public string? Notes { get; set; }
	}
}
