
using FoodBazar.Web.Models;

namespace FoodBazar.Web.Services.IServices
{
    public interface ICartService
    {

        public Task<ResponseDto?> GetCartbyUserIdAsync(string userId);
        public Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        public Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
        public Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        public Task<ResponseDto?> EmailCart(CartDto cartDto);

    }
}
