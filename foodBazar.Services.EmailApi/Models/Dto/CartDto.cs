using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodBazar.Services.EmailApi.Models.Dto
{

    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
    public class CartDetailsDto
    {
        public int CartDeatailsId { get; set; }

        public int CartHeaderId { get; set; }

        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }

        public ProductDto? Product { get; set; }

        public int Count { get; set; }
    }


    public class CartHeaderDto
    {

        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

        public double Discount { get; set; }
        public double CartTotal { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
