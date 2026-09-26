using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson
{
    public class SecurityPersonDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único del registro de la persona de seguridad del local.")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("person_id")]
        [Description("Identificador único del registro de la tabla persona.")]
        public int PersonId { get; set; }

        [JsonPropertyName("person_document_number")]
        [Description("Número de documento de identidad de la persona")]
        public string? PersonDocumentNumber { get; set; }

        [JsonPropertyName("person_full_name")]
        [Description("Nombre completo de la persona.")]
        public string PersonFullName { get; set; }

        [JsonPropertyName("person_role")]
        [Description("Cargo de la persona en la empresa.")]
        public string PersonRole { get; set; }

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

        [JsonPropertyName("series")]
        public List<SecurityPersonSeriesDTO> Series { get; set; } = new List<SecurityPersonSeriesDTO>();

        [JsonPropertyName("entity_branchs")]
        public List<SecurityPersonEntityBranchsDTO> EntityBranches { get; set; } = new List<SecurityPersonEntityBranchsDTO>();

        [JsonPropertyName("entity_branchs")]
        public List<SecurityPersonReasonForTransferDTO> ReasonForTransfers { get; set; } = new List<SecurityPersonReasonForTransferDTO>();
    }

    public class SecurityPersonSeriesDTO
    {
        [JsonPropertyName("serie")]
        public string Serie { get; set; }

        [JsonPropertyName("entity_name")]
        public string EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        public string EntityDocumentNumber { get; set; }
    }

    public class SecurityPersonEntityBranchsDTO
    {
        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("entity_name")]
        public string EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        public string EntityDocumentNumber { get; set; }
    }

    public class SecurityPersonReasonForTransfersDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code_sunat")]
        public string CodeSunat { get; set; }

    }
}
