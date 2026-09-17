using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.EntityBranchSerie
{
    public class EntityBranchSerieDTO
    {
        [JsonPropertyName("id")]
        public int EntityBranchSerieId { get; set; }

        [JsonPropertyName("entity_branch_id")]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("invoice_type_id")]
        public int InvoiceTypeId { get; set; }

        [JsonPropertyName("invoice_type")]
        public string InvoiceType { get; set; }

        [JsonPropertyName("serie")]
        public string Serie { get; set; }

        [JsonPropertyName("area")]
        public string Area { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        public string CreatedAtUserName { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("updated_at_user")]
        public string? UpdatedAtUser { get; set; }

        [JsonPropertyName("updated_at_user_name")]
        public string? UpdatedAtUserName { get; set; }
    }
}
