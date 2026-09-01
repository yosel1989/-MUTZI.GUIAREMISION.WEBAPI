using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Entity
{
    public class EntityListDTO 
    {
        [JsonPropertyName("id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [JsonPropertyName("name")]
        [Description("Razón social si es empresa")]
        public string? Name { get; set; }

        [JsonPropertyName("first_name")]
        [Description("Nombre si es persona")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [Description("Apellido si es persona")]
        public string? LastName { get; set; }

        [JsonPropertyName("document_number")]
        [Description("Número de documento de identidad o RUC")]
        public string DocumentNumber { get; set; }


    }
}
