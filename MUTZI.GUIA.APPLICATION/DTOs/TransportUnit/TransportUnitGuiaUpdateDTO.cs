using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte
{
    public class TransportUnitGuiaUpdateDTO
    {
        [JsonPropertyName("transporte_guia_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del transporte_guia, OJO: Colocar el mismo ID si el transporte asociado sigue siendo el mismo, en caso de ser nuevo transporte_guia se debe dejar como valor cero '0'.", Nullable = false)]
        [Required]
        public int TransporteGuidID { get; set; }

        [JsonPropertyName("transporte_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del transporte asociado a la guía de remisión.", Nullable = false)]
        [Required]
        public int TransporteId { get; set; }
    }
}
