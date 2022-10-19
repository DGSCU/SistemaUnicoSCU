using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class LengthAttribute : ValidationAttribute
	{
		private readonly int minLength;
		private readonly int maxLength;
		private readonly int lowerErrorMessage;
		private readonly int higerErrorMessage;
		private const string DEFAULT_ERROR= "Il vaore non può superare i {0} caratteri";
		public LengthAttribute(int maxLength, string errorMessage = "Il vaore non può superare i {0} caratteri") : base(errorMessage)
		{
			this.maxLength = maxLength;
		}
		public int MaxLength { get{ return maxLength; } }
		public override bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			if (!(value is string text)){
				return false;
			}
			if (text.Length>maxLength)
			{
				ErrorMessage = string.Format(ErrorMessage?? DEFAULT_ERROR, maxLength);
				return false;
			}
			return true;
		}
	}
}
