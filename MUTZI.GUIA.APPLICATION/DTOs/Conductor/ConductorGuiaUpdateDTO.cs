using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.Conductor
{
    public class ConductorGuiaUpdateDTO
    {
        [JsonPropertyName("conductor_guia_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del conductor_guia, OJO: Colocar el mismo ID si el conductor asociado sigue siendo el mismo, en caso de ser nuevo conductor_guia se debe dejar como valor cero '0'.", Nullable = false)]
        [Required]
        public int ConductorGuidID { get; set; }

        [JsonPropertyName("conductor_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del conductor asociado a la guía.", Nullable = false)]
        [Required]
        public int ConductorID { get; set; }
    }
}
