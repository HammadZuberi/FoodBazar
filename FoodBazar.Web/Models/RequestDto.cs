using FoodBazar.Web.Utilities;
using static FoodBazar.Web.Utilities.SD;

namespace FoodBazar.Web.Models
{
    public class RequestDto
    {

        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public string AccessToken { get; set; }
        public object Data { get; set; }
    }
}
