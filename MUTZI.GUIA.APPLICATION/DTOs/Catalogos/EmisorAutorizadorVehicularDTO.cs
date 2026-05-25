using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    [SwaggerSchema("Devuelve lista de Entidades emisoras de la autorización vehícular.", Nullable = false)]
    public class EmisorAutorizadorVehicularDTO
    {
        [JsonPropertyName("codigo")]
        [SwaggerSchema("Devuelve en texto el código de la entidad emisora.", Nullable = false)]
        public string Codigo { get; set; }

        [JsonPropertyName("nombre")]
        [SwaggerSchema("Devuelve en texto el nombre de la entidad emisora.", Nullable = false)]
        public string Nombre { get; set; }

        [JsonPropertyName("abreviatura")]
        [SwaggerSchema("Devuelve en texto la abreviatura de la entidad emisora.", Nullable = false)]
        public string Abreviatura { get; set; }
    }
}
