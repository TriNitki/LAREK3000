using static CatalogAPI.Utility.SD;

namespace CatalogAPI.Models.DTO.AuthDTO
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
