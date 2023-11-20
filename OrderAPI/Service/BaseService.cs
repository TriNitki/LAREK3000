using OrderAPI.Models.DTO.AuthDTO;
using OrderAPI.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static OrderAPI.Utility.SD;
using System.Web;

namespace OrderAPI.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto<T>?> SendAsync<T>(RequestDto request)
        {
            try
            {
                HttpClient client = httpClientFactory.CreateClient("OrderAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                // Set authorization
                if (request.AccessToken != null)
                {
                    message.Headers.Add("Authorization", $"Bearer {request.AccessToken}");
                }
                
                var uri = new UriBuilder(request.Url);

                // Add query parameters to URL
                if (request.Queries != null)
                {
                    var query = HttpUtility.ParseQueryString(uri.Query);
                    foreach (KeyValuePair<string, string?> param in request.Queries)
                    {
                        query[param.Key] = param.Value;
                    }
                    uri.Query = query.ToString();
                }
                
                message.RequestUri = uri.Uri;

                // Add request body
                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                // Set request type
                switch (request.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                // Process status code
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Forbidden" };
                    case HttpStatusCode.BadRequest:
                        return new() { IsSuccess = false, Message = "BadRequest" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = new ResponseDto<T>()
                        {
                            IsSuccess = true,
                            Result = JsonConvert.DeserializeObject<T>(apiContent)
                        };

                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto<T>()
                {
                    IsSuccess = false,
                    Message = ex.Message.ToString(),
                };
                return dto;
            }
        }
    }
}
