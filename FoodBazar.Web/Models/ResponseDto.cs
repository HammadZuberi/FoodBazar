namespace FoodBazar.Web.Models
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public object? Result { get; set; }
    }
}
