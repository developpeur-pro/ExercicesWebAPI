namespace JobOverview_v77.Services
{
	public class ValidationRulesException : Exception
	{
		public Dictionary<string, string[]> Errors { get; } = new();

		public ValidationRulesException()
		{

		}

		public ValidationRulesException(string message)
			 : base(message)
		{
		}

		public ValidationRulesException(string message, Exception inner)
			 : base(message, inner)
		{
		}
	}
}
