using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranchSerie
{
    public class SecurityPersonEntityBranchSerieDeleteDTO
    {
        [JsonPropertyName("security_person_id")]
        [Description("Identificador único del registro de la persona que utilizará el sistema")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("entity_branch_serie_id")]
        [Description("Identificador único del registro de la persona de seguridad del local.")]
        public int EntityBranchSerieId { get; set; }
    }
}
