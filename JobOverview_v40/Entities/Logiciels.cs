namespace JobOverview_v40.Entities
{
	public class Filiere
	{
		public string Code { get; set; } = "";
		public string Nom { get; set; } = "";
	}

	public class Logiciel
	{
		public string Code { get; set; } = "";
		public string CodeFiliere { get; set; } = "";
		public string Nom { get; set; } = "";
	}

	public class Module
	{
		public string Code { get; set; } = "";
		public string CodeLogiciel { get; set; } = "";
		public string Nom { get; set; } = "";

		public string? CodeModuleParent { get; set; }
		public string? CodeLogicielParent { get; set; }
	}

	public class Version
	{
		public float Numero { get; set; }
		public string CodeLogiciel { get; set; } = "";
		public short Millesime { get; set; }
		public DateTime DateOuverture { get; set; }
		public DateTime DateSortiePrevue { get; set; }
		public DateTime? DateSortieReelle { get; set; }
	}

	public class Release
	{
		public short Numero { get; set; }
		public float NumeroVersion { get; set; }
		public string CodeLogiciel { get; set; } = "";
		public DateTime DatePubli { get; set; }
	}
}
