using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.UnidadMedida
{
    public class UnidadMedidaToSelectDTO
    {
        [JsonPropertyName("id")]
        public int UnidadMedidaId { get; set; }

        [JsonPropertyName("codigo_um")]
        [MaxLength(10)]
        public string CodigoUm { get; set; } = null!;

        [JsonPropertyName("descripcion")]
        [MaxLength(100)]
        public string Descripcion { get; set; } = null!;
    }
}
