using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs
{
    public class GuiaCreatedResponseDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, es el número ID de la guía.", Nullable = false)]
        public int GuiaID { get; set; }

        [JsonPropertyName("tipo_guia")]
        [SwaggerSchema("Devuelve en texto el tipo de guía: REMITENTE ó TRANSPORTISTA", Nullable = false)]
        public string TipoGuia { get; set; }

        [JsonPropertyName("numero_guia")]
        [SwaggerSchema("Devuelve en texto el número de la guía creada recientemente.", Nullable = false)]
        public string NumeroGuia { get; set; }
    }
}
