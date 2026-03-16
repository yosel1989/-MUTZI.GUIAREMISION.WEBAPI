using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente
{
    public class RemitenteCreateGuiaDTO
    {
        [JsonPropertyName("ruc")]
        [SwaggerSchema("Recibe un texto, es el ruc del remitente", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del remitente es obligatoria")]
        [StringLength(11, ErrorMessage = "El número de documento del remitente no puede exceder los 11 caracteres")]
        [MinLength(11)]
        public string Ruc { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la descripción o nombre del remitente", Nullable = false)]
        [MaxLength(200)]
        [Required]
        public string Descripcion { get; set; }
    }
}
