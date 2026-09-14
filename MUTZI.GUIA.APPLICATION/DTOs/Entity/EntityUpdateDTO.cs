using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Entity
{
    public class EntityUpdateDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [JsonPropertyName("type")]
        [Description("Tipo de entidad: company o person")]
        public string Type { get; set; } = "company";

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

        [JsonPropertyName("is_internal")]
        [Description("Indica si la entidad pertenece al consorcio o no es empresa tercera.")]
        public bool IsInternal { get; set; } = false;


        // Condicionales

        [JsonPropertyName("mtc_code")]
        [Description("Número de registro MTC ")]
        public string? MtcCode { get; set; }
    }
}
