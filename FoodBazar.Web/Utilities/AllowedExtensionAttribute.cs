using System.ComponentModel.DataAnnotations;

namespace FoodBazar.Web.Utilities
{
	public class AllowedFileSizeExtensionAttribute : ValidationAttribute
	{
		private readonly string[] _extensions;
		private readonly int _maxFileSize;
		public AllowedFileSizeExtensionAttribute(string[] extensions, int maxFileSize = 1)
		{
			_extensions = extensions;
			_maxFileSize = maxFileSize;

		}
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var file = value as IFormFile;

			if (file != null)
			{
				var extension = Path.GetExtension(file.FileName);
				if (!extension.Contains(extension.ToLower()))
				{
					return new ValidationResult("this file extension is not allowed!");
				}

				if (file.Length > (_maxFileSize * 1024 * 1024))
				{
					return new ValidationResult($"Maximum allowed file size {_maxFileSize} MB, file is not allowed!");
				}
			}

			//return base.IsValid(value, validationContext);
			return ValidationResult.Success;
		}
	}
}
