using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.EmailDestinatarioGuia
{
    [SwaggerSchema("Devuelve el objeto email asociado a la guía remitente.", Nullable = false)]
    public class EmailDestinatarioGuiaDTO
    {
        [JsonPropertyName("email_destinatario_guia_id")]
        [SwaggerSchema("Devuelve un entero, aquí se muestra el ID del email asociado a la guía", Nullable = false)]
        public int EmailDestinatarioGuidID { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Devuelve un texto, aquí se muestra el correo electrónico del destinatario", Nullable = false)]
        public string Email { get; set; }

        [JsonPropertyName("guia_id")]
        [SwaggerSchema("Devuelve un entero, aquí se muestra el ID de la guía remitente a la que está asociado el email", Nullable = false)]
        public int GuiaID { get; set; }

        [JsonPropertyName("destinatario_id")]
        [SwaggerSchema("Devuelve un entero, aquí se muestra el ID del destinatario al que está asociado el email", Nullable = false)]
        public int DestinatarioID { get; set; }
    }
}
