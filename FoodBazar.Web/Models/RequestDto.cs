using FoodBazar.Web.Utilities;
using static FoodBazar.Web.Utilities.SD;
using ContentType = FoodBazar.Web.Utilities.SD.ContentType;

namespace FoodBazar.Web.Models
{
    public class RequestDto
    {

        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public string AccessToken { get; set; }
        public object Data { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
