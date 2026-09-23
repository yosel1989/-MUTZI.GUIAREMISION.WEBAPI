using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranchSerie
{
    public class SecurityPersonEntityBranchSerieDTO
    {
        [JsonPropertyName("security_person_id")]
        [Description("Identificador único del registro de la persona que utilizará el sistema")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("entity_branch_serie_id")]
        [Description("Identificador único del registro de la serie.")]
        public int EntityBranchSerieId { get; set; }

        [JsonPropertyName("serie")]
        [Description("Serie del establecimiento.")]
        public string Serie { get; set; }

        [JsonPropertyName("entity_name")]
        [Description("Nombre de la entidad")]
        public string EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        [Description("Número de documento de la entidad")]
        public string EntityDocumentNumber { get; set; }

        [JsonPropertyName("created_at")]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; }

    }
}
