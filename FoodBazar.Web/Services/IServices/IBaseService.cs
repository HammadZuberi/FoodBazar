using FoodBazar.Web.Models;

namespace FoodBazar.Web.Services.IServices
{
	public interface IBaseService
	{
		Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBeaer = true);
	}
}
