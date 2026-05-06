using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.Conductor
{
    public class ConductorCreateGuiaDTO
    {
        [JsonPropertyName("conductor_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del conductor que se va a asociar a los datos_envio de la guía", Nullable = true)]
        [Required]
        public int ConductorId { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el tipo de documento del conductor. Solo recibe los siguientes valores: DNI, CE y PASAPORTE", Nullable = false)]
        [MaxLength(21)]
        [Required]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, se coloca el número de documento del conductor. En caso de DNI, solo 8 dígitos, en otros casos acepta hasta 15 digitos", Nullable = false)]
        [MaxLength(15)]
        [MinLength(8)]
        [Required]
        public string NumeroDocumento { get; set; }
    }
}
