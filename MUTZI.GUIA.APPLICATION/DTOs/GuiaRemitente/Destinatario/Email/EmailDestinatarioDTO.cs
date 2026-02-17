using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario.Email
{
    [SwaggerSchema("Devuelve el objeto email asociado al destinatario.", Nullable = false)]
    public class EmailDestinatarioDTO
    {
        [JsonPropertyName("emailDestinatarioID")]
        [SwaggerSchema("Devuelve un entero, aquí se muestra el ID del email asociado al destinatario", Nullable = false)]
        public int EmailDestinatarioID { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Devuelve un texto, aquí se muestra el correo electrónico del destinatario", Nullable = false)]
        public string Email { get; set; }

        [JsonPropertyName("destinatarioID")]
        [SwaggerSchema("Devuelve un entero, aquí se muestra el ID del destinatario al que está asociado el email", Nullable = false)]
        public int DestinatarioID { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, aquí se muestra el estado del email asociado al destinatario.", Nullable = false)]
        public string Estado { get; set; }
    }
}
