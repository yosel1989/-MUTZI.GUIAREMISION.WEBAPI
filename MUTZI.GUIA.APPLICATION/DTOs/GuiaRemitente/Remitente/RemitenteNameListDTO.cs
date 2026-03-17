using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente
{
    public class RemitenteNameListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del remitente en la bbdd", Nullable = false)]
        public int RemitenteID { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve en texto la descripción o nombre del remitente/sucursal.", Nullable = false)]
        public string Descripcion { get; set; }
    }
}
