using System.Text.Json.Serialization;

namespace SalesService.Application.Dtos
{
    public record ApiResponse
    {
        public bool Success { get; set; } = true;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; }
    }
}
