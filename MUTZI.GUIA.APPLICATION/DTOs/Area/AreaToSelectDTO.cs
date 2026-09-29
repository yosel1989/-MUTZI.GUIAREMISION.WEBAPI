using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Area
{
    public class AreaToSelectDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único del área")]
        public int AreaId { get; set; }

        [JsonPropertyName("code")]
        [Required]
        [Description("Código del área: 2 dígitos si es principal (10), 4 dígitos si es subárea (1001)")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("name")]
        [Required]
        [Description("Nombre completo del área. En subáreas incluye al padre: 'Producción – Envasado'")]
        public string Name { get; set; } = null!;
    }
}
