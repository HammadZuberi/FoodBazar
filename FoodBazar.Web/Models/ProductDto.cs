using FoodBazar.Web.Utilities;
using System.ComponentModel.DataAnnotations;

namespace FoodBazar.Web.Models
{
	public class ProductDto
	{
		[Key]
		public int ProductId { get; set; }

		[Required]
		public string Name { get; set; }
		[Required]
		[Range(0, 1000)]
		public double Price { get; set; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
		public string? ImageUrl { get; set; }
		public string? ImageLocalPath { get; set; }
		public int Count { get; set; } = 1;
		[AllowedFileSizeExtension(new string[] { ".jpg", ".png" }, 1)]
		public IFormFile? Image { get; set; }
	}
}
