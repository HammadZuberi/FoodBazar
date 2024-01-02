using FoodBazar.Services.ShoppingCartApi.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodBazar.Services.ShoppingCartApi.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDeatailsId { get; set; }

        public int CartHeaderId { get; set; }

        [ForeignKey(nameof(CartHeaderId))]
        public CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto Product { get; set; }

        public int Count { get; set; }
    }
}
