using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EntityBranchDTO
    {
        [JsonPropertyName("id")]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("entity_id")]
        public int EntityId { get; set; }

        [JsonPropertyName("entity_name")]
        public string EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        public string EntityDocumentNumber { get; set; }

        [JsonPropertyName("entity_address")]
        public string EntityAddress { get; set; }

        [JsonPropertyName("entity_department")]
        public string EntityDepartment { get; set; }

        [JsonPropertyName("entity_province")]
        public string EntityProvince { get; set; }

        [JsonPropertyName("entity_district")]
        public string EntityDistrict { get; set; }

        [JsonPropertyName("entity_country")]
        public string? EntityCountry { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("ubigeo_id")]
        public string UbigeoId { get; set; } = null!;

        [JsonPropertyName("department")]
        public string Department { get; set; } = null!;

        [JsonPropertyName("province")]
        public string Province { get; set; } = null!;

        [JsonPropertyName("district")]
        public string District { get; set; } = null!;

        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("serie")]
        public string Serie { get; set; } = null!;

        [JsonPropertyName("code_sunat")]
        public string CodeSunat { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        public string CreatedAtUserName { get; set; }

        [JsonPropertyName("updated_at_user")]
        public string? UpdatedAtUser { get; set; }

        [JsonPropertyName("updated_at_user_name")]
        public string? UpdatedAtUserName { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("area")]
        public string? Area { get; set; }

        [JsonPropertyName("is_main")]
        public bool IsMain { get; set; } = false;
    }
}
