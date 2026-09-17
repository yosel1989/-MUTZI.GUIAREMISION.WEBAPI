using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    public class InvoiceTypeDTO 
    {
        [JsonPropertyName("id")]
        public int InvoiceTypeId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code_sunat")]
        public string CodeSunat { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }



    public class InvoiceTypeToSelectDTO
    {
        [JsonPropertyName("id")]
        public int InvoiceTypeId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code_sunat")]
        public string CodeSunat { get; set; }
    }
}
