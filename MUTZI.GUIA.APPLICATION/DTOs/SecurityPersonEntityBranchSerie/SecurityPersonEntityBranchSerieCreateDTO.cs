using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranchSerie
{
    public class SecurityPersonEntityBranchSerieCreateDTO
    {
        [JsonPropertyName("security_person_id")]
        [Description("Identificador único del registro de la persona que utilizará el sistema")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("entity_branch_serie_id")]
        [Description("Identificador único del registro de la serie del local.")]
        public int EntityBranchSerieId { get; set; }
    }
}
