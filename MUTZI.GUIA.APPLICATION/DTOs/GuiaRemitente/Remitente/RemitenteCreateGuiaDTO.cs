using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente
{
    public class RemitenteCreateGuiaDTO
    {
        [JsonPropertyName("remitente_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del remitente que se va a asociar a la guía de remisión.", Nullable = false)]
        [Required]
        public int RemitenteId { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el número de documento del remitente.", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del remitente es obligatorio")]
        [MaxLength(21)]
        [MinLength(8)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("email_remitente")]
        [SwaggerSchema("Recibe un arreglo de textos, aquí se coloca el correo electrónico del destinatario al cual le llegara la notificación de la guía emitida", Nullable = true)]
        public List<string>? EmailDestinatarioGuia { get; set; }
    }
}
