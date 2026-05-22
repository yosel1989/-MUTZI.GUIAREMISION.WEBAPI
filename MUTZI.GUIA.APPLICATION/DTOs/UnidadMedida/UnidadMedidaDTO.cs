using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.UnidadMedida
{
    public class UnidadMedidaDTO
    {
        [JsonPropertyName("id")]
        public int UnidadMedidaId { get; set; }

        [JsonPropertyName("codigo_um")]
        [MaxLength(10)]
        public string CodigoUm { get; set; } = null!;

        [JsonPropertyName("descripcion")]
        [MaxLength(100)]
        public string Descripcion { get; set; } = null!;

        [JsonPropertyName("es_unidad_peso")]
        public bool EsUnidadPeso { get; set; } = false;

        [JsonPropertyName("es_unidad_volumen")]
        public bool EsUnidadVolumen { get; set; } = false;

        [JsonPropertyName("es_unidad_longitud")]
        public bool EsUnidadLongitud { get; set; } = false;

        [JsonPropertyName("es_unidad_conteo")]
        public bool EsUnidadConteo { get; set; } = false;

        [JsonPropertyName("activo")]
        public bool Activo { get; set; }
    }
}
