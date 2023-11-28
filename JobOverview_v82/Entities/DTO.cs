namespace JobOverview_v82.Entities
{
	public class FormRelease
	{
		public short Numero { get; set; }
		public float NumeroVersion { get; set; }
		public string CodeLogiciel { get; set; } = "";
		public DateTime DatePubli { get; set; }
		public IFormFile? Notes { get; set; }
	}
}
