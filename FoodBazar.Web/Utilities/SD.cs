namespace FoodBazar.Web.Utilities
{
	public class SD
	{
		public static string CouponApiUri { get; set; }
		public static string AuthApiUri { get; set; }
		public static string ProductApiUri { get; set; }
		public static string OrderApiUri { get; set; }
		public static string CartApiUri { get; set; }

		//Roles
		public static string RoleAdmin = "ADMIN";
		public static string RoleCustomer = "CUSTOMER";
		public static string TokenCookie = "Token";


		public const string Status_Pending = "Pending";
		public const string Status_Approved = "Approved";
		public const string Status_ReadyForPickUp = "ReadyForPickup";
		public const string Status_Completed = "Completed";
		public const string Status_Refunded = "Refunded";
		public const string Status_Cancelled = "Cancelled";

		public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE

		}

		public enum ContentType
		{
			Json,
			MultipartFormData
		}
	}
}
