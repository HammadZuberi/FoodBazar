namespace FoodBazar.Web.Utilities
{
	public class SD
	{
		public static string CouponApiUri { get; set; }
		public static string AuthApiUri { get; set; }
		public static string ProductApiUri { get; set; }
		public static string RoleAdmin = "ADMIN";
		public static string RoleCustomer = "CUSTOMER";
		public static string TokenCookie = "Token";

		public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE

		}
	}
}
