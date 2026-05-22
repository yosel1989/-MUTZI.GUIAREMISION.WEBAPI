using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    [SwaggerSchema("Devuelve lista de los tipos de establecimiento según SUNAT", Nullable = false)]
    public class TipoEstablecimientoDTO
    {
        [JsonPropertyName("tipo")]
        [SwaggerSchema("Devuelve en texto el nombre del tipo.", Nullable = false)]
        public string Tipo { get; set; }

        [JsonPropertyName("Codigo")]
        [SwaggerSchema("Devuelve en texto el código del establecimiento", Nullable = false)]
        public string Codigo { get; set; }
    }
}
