using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.EmailRemiteTransportistaGuia
{
    [SwaggerSchema("Devuelve el objeto email asociado a la guía transportista.", Nullable = false)]
    public class EmailRemiteTransportistaGuiaDTO
    {
        [JsonPropertyName("email_remite_transportista_guia_id")]
        [SwaggerSchema("Devuelve un entero, aquí se muestra el ID del email asociado a la guía transportista", Nullable = false)]
        public int EmailRemiteTransportistaGuidID { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Devuelve un texto, aquí se muestra el correo electrónico del remitente asociado a la guía transportista", Nullable = false)]
        public string Email { get; set; }

        [JsonPropertyName("guia_id")]
        [SwaggerSchema("Devuelve un entero, aquí se muestra el ID de la guía transportista a la que está asociado el email", Nullable = false)]
        public int GuiaID { get; set; }

        [JsonPropertyName("remite_transportista_id")]
        [SwaggerSchema("Devuelve un entero, aquí se muestra el ID del remitente transportista al que está asociado el email", Nullable = false)]
        public int RemiteTransportistaID { get; set; }
    }
}
