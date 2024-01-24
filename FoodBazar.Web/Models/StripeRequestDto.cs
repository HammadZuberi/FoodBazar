namespace FoodBazar.Web.Models
{
	public class StripeRequestDto
	{
        public string StripeSessionId { get; set; }
        public string StripeSessionUrl { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; set; }
        public OrderHeaderDto orderHeader { get; set; }
    }
}
