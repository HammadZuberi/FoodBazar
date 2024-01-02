namespace FoodBazar.Services.ShoppingCartApi.Models.Dto
{

    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public object? Result { get; set; }
    }
}
