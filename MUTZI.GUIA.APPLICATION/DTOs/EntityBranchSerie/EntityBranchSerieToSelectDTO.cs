using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.EntityBranchSerie
{
    public class EntityBranchSerieToSelectDTO
    {
        [JsonPropertyName("id")]
        public int EntityBranchSerieId { get; set; }

        [JsonPropertyName("serie")]
        public string Serie { get; set; }

        [JsonPropertyName("entity_name")]
        public string EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        public string EntityDocumentNumber { get; set; }

        [JsonPropertyName("entity_branch_alias")]
        public string EntityBranchAlias { get; set; }

        [JsonPropertyName("entity_branch_address")]
        public string EntityBranchAddress { get; set; }
    }
}
