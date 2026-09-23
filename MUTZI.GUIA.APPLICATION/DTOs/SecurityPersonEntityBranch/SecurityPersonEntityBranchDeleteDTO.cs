using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranch
{
    public class SecurityPersonEntityBranchDeleteDTO
    {
        [JsonPropertyName("security_person_id")]
        [Description("Identificador único del registro de la persona que utilizará el sistema")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("entity_branch_id")]
        [Description("Identificador único del registro de la persona de seguridad del local.")]
        public int EntityBranchId { get; set; }
    }
}
