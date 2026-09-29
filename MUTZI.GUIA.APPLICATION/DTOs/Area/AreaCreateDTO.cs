using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Area
{
    public class AreaCreateDTO
    {
        [JsonPropertyName("code")]
        [Required]
        [Description("Código del área: 2 dígitos si es principal (10), 4 dígitos si es subárea (1001)")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("name")]
        [Required]
        [Description("Nombre completo del área. En subáreas incluye al padre: 'Producción – Envasado'")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("parent_area_id")]
        [Description("Id del área padre. NULL si es un área principal")]
        public int? ParentAreaId { get; set; }

        [JsonPropertyName("level")]
        [Description("Nivel jerárquico: 1 = área principal, 2 = subárea")]
        public byte Level { get; set; }
    }
}
