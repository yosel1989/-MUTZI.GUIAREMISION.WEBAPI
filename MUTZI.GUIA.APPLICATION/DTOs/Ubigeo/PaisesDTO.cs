using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.Ubigeo
{
    [SwaggerSchema("Devuelve lista de código de países", Nullable = false)]
    public class PaisesDTO
    {
        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve en texto el nombre del país.", Nullable = false)]
        public string Pais { get; set; }

        [JsonPropertyName("codigo")]
        [SwaggerSchema("Devuelve en texto el código del país.", Nullable = false)]
        public string Codigo { get; set; }
    }
}
