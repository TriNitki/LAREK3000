using static OrderAPI.Utility.SD;

namespace OrderAPI.Models.DTO.AuthDTO
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object? Data { get; set; }
        public string? AccessToken { get; set; }
        public Dictionary<string, string?>? Queries { get; set; }
    }
}
