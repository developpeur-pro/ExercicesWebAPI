namespace JobOverview_v123.Entities;

public class Travail
{
	public DateTime DateTravail { get; set; }
	public int IdTache { get; set; }
	public decimal Heures { get; set; }
	public decimal TauxProductivite { get; set; }
}

public class Tache
{
	public int Id { get; set; }
	public string Titre { get; set; } = "";
	public decimal DureePrevue { get; set; }
	public decimal DureeRestante { get; set; }
	public string CodeActivite { get; set; } = "";
	public string Personne { get; set; } = "";
	public string CodeLogiciel { get; set; } = "";
	public string CodeModule { get; set; } = "";
	public float NumVersion { get; set; }
	public string? Description { get; set; }

	public virtual List<Travail> Travaux { get; set; } = new();
}

public class Activite
{
	public string Code { get; set; } = "";
	public string Titre { get; set; } = "";
}

public class ActiviteMetier
{
	public string CodeActivite { get; set; } = "";
	public string CodeMetier { get; set; } = "";
}