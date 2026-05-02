using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario
{
    public class DestinatarioCreateGuiaDTO
    {
        [JsonPropertyName("destinatario_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del destinatario que se va a asociar a la guía de remisión.", Nullable = false)]
        [Required]
        public int DestinatarioId { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el número de documento del destinatario.", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del destinatario es obligatorio")]
        [MaxLength(21)]
        [MinLength(8)]
        public string NumeroDocumento { get; set; }

        //[JsonPropertyName("email_destinatario")]
        //[SwaggerSchema("Recibe un texto, aquí se coloca hasta máximo 3 correos del destinatario", Nullable = true)]
        //public List<EmailDestinatarioGuiaCreateDTO>? EmailDestinatarioGuia { get; set; }


        [JsonPropertyName("email_destinatario")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del destinatario", Nullable = true)]
        [MaxLength(50)]
        public List<string>? EmailDestinatarioGuia { get; set; }
    }
}
