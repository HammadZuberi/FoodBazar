

namespace FoodBazar.Services.OrderApi.Models.Dto
{
	public class OrderHeaderDto
	{
		public int OrderHeaderId { get; set; }
		public string? UserId { get; set; }
		public string? CouponCode { get; set; }

		public double Discount { get; set; }
		public double OrderTotal { get; set; }

		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }

		public DateTime OrderTime { get; set; }
		public string? Status { get; set; }
		public string? PaymentIntentId { get; set; }
		public string? StripeSessionId { get; set; }

		public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
	}

	public class OrderDetailsDto
	{
		public int OrderDeatailsId { get; set; }

		public int OrderHeaderId { get; set; }
		public int ProductId { get; set; }
	
		public ProductDto? Product { get; set; }

		public int Count { get; set; }
		public string ProductName { get; set; }
		public double Price { get; set; }


	}
}
