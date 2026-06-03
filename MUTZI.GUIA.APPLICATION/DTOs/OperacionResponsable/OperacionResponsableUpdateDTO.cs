using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.OperacionResponsable
{
    public class OperacionResponsableUpdateDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Recibe un número, es el id de la operación", Nullable = false)]
        [Required(ErrorMessage = "El id de la operación es requerida")]
        public int OperacionResponsableId { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Recibe un texto, es el nombre de la operación", Nullable = false)]
        [Required(ErrorMessage = "El nombre de la operación es obligatoria")]
        [StringLength(75, ErrorMessage = "El nombre de la operación no puede exceder los 75 caracteres")]
        public string Descripcion { get; set; }

        [JsonPropertyName("guia_remision_estado_id")]
        [SwaggerSchema("Recibe un número, aquí se coloca el id del estado de la guía de remisión a relacionar", Nullable = false)]
        [Required]
        public int GuiaRemisionEstadoId { get; set; }
    }
}
