using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista
{
    public class RemiteTransportistaCreateGuiaDTO
    {
        [JsonPropertyName("remite_transportista_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del remite_transportista que se va a asociar a la guía de remisión TRANSPORTISTA.", Nullable = false)]
        [Required]
        public int RemiteTransportistaId { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el número de documento del REMITE_TRANSPORTISTA.", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del REMITE_TRANSPORTISTA es obligatorio")]
        [MaxLength(15)]
        [MinLength(8)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("email_remite_transportista")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del REMITE_TRANSPORTISTA", Nullable = true)]
        [MaxLength(50)]
        public List<string>? EmailRemiteTransportista { get; set; }
    }
}
