using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static FoodBazar.Web.Utilities.SD;

namespace FoodBazar.Web.Services
{
    public class BaseService : IBaseService
    {

        public readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;

        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("FoodBazarAPI");

                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data != null)
                { //for put and post 

                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;


                switch (requestDto.ApiType)
                {

                    case (ApiType.POST):
                        message.Method = HttpMethod.Post;
                        break;

                    case (ApiType.PUT):
                        message.Method = HttpMethod.Put;
                        break;

                    case (ApiType.DELETE):
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }
                //send request
                apiResponse = await client.SendAsync(message);

                //chk status
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not found" };
                        break;

                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Not found" };
                        break;

                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Not found" };
                        break;

                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Not found" };
                        break;
                    case HttpStatusCode.NetworkAuthenticationRequired:
                        return new() { IsSuccess = false, Message = "Not found" };
                        break;
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message.ToString()
                };
                return dto;
            }



        }
    }

}
