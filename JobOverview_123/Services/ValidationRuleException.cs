namespace JobOverview_v123.Services
{
	public class ValidationRulesException : Exception
	{
		public Dictionary<string, string[]> Errors { get; } = new();

		public ValidationRulesException()
		{

		}

		public ValidationRulesException(string propriété, string message)
			 : base(message)
		{
			Errors.Add(propriété, new string[] { message });
		}

		public ValidationRulesException(string propriété, string message, Exception inner)
			 : base(message, inner)
		{
			Errors.Add(propriété, new string[] { message });			
		}
	}
}
