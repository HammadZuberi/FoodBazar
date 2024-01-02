using System.ComponentModel.DataAnnotations;

namespace FoodBazar.Services.Product.Models.Dto
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
		public string ImageUrl { get; set; }
	}
}
