using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    public class GuiaRemisionIndicadorTrasladoDTO
    {
        [JsonPropertyName("id")]
        public int GuiaRemisionIndicadorTrasladoId { get; set; }

        [JsonPropertyName("name")]
        public string Descripcion { get; set; }

        [JsonPropertyName("code")]
        public string CodigoSunat { get; set; }

        [JsonPropertyName("active")]
        public bool Activo { get; set; } = true;
    }



    public class GuiaRemisionIndicadorTrasladoToSelectDTO
    {
        [JsonPropertyName("id")]
        public int GuiaRemisionIndicadorTrasladoId { get; set; }

        [JsonPropertyName("name")]
        public string Descripcion { get; set; }

        [JsonPropertyName("code")]
        public string CodigoSunat { get; set; }
    }
}
