using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.DataTable
{
    public class PagedResult<T>
    {
        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; }
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("data")]
        public List<T> Data { get; set; }
    }
}
