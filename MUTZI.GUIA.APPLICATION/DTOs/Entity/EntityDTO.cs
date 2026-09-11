using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Entity
{
    public class EntityDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [JsonPropertyName("type")]
        [Description("Tipo de entidad: company o person")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        [Description("Razón social si es empresa")]
        public string? Name { get; set; }

        [JsonPropertyName("first_name")]
        [Description("Nombre si es persona")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [Description("Apellido si es persona")]
        public string? LastName { get; set; }

        [JsonPropertyName("document_type_id")]
        [Description("Id del tipo de documento (DNI, RUC, etc.) de la tabla master_data.document_type")]
        public int DocumentTypeId { get; set; }

        [JsonPropertyName("document_type")]
        [Description("Nombre del tipo de documento (DNI, RUC, etc.) de la tabla master_data.document_type")]
        public string DocumentType { get; set; }

        [JsonPropertyName("document_number")]
        [Description("Número de documento de identidad o RUC")]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [Description("Código de ubicación geográfica (Ubigeo)")]
        public string? UbigeoId { get; set; }

        [JsonPropertyName("address")]
        [Description("Dirección física de la entidad")]
        public string? Address { get; set; }

        [JsonPropertyName("country_id")]
        [Description("Identificador del país")]
        public int? CountryId { get; set; }

        [JsonPropertyName("country")]
        [Description("Nombre del país")]
        public string? Country { get; set; }

        [JsonPropertyName("is_internal")]
        [Description("Indica si la entidad pertenece al consorcio o no es empresa tercera.")]
        public bool IsInternal { get; set; } = true;

        [JsonPropertyName("active")]
        [Description("Estado de la entidad: activa o inactiva")]
        public bool Active { get; set; } = true;

        [JsonPropertyName("created_at")]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; }

        [JsonPropertyName("updated_at")]
        [Description("Fecha de última modificación")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("updated_at_user")]
        [Description("Usuario que modificó el registro")]
        public string? UpdatedAtUser { get; set; }

        [JsonPropertyName("updated_at_user_name")]
        [Description("Nombre del usuario que modificó el registro")]
        public string? UpdatedAtUserName { get; set; }

        // Condicionales

        [Column("mtc_code")]
        [Description("Número de registro MTC ")]
        public string? MtcCode { get; set; }
    }
}
