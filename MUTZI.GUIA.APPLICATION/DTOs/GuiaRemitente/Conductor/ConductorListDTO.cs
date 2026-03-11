using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor
{
    public class ConductorListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del conductor en la bbdd", Nullable = false)]
        public int ConductorID { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve un texto, el tipo de documento solo pueden tener los siguientes valores: DNI, CE y PASAPORTE", Nullable = false)]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Devuelve en texto el número de documento del conductor", Nullable = false)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("nombres")]
        [SwaggerSchema("Devuelve en texto los nombres del conductor", Nullable = false)]
        public string Nombres { get; set; }

        [JsonPropertyName("apellidos")]
        [SwaggerSchema("Devuelve en texto los apellidos del conductor", Nullable = false)]
        public string Apellidos { get; set; }

        [JsonPropertyName("licencia")]
        [SwaggerSchema("Devuelve el número de licencia del conductor", Nullable = false)]
        public string Licencia { get; set; }
    }
}
