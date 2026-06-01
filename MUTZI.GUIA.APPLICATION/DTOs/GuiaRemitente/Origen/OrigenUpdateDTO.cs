using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen
{
    public class OrigenUpdateDTO
    {
        [JsonPropertyName("origen_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del origen que se va a actualizar", Nullable = false)]
        [Required]
        public int OrigenId { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del proveedor, ejm: 150131", Nullable = false)]
        [Required(ErrorMessage = "EL ubigeo es obligatorio")]
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string UbigeoId { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la dirección de origen o del punto de partida", Nullable = false)]
        [MaxLength(250)]
        [Required]
        public string Direccion { get; set; }
        //public string Ciudad { get; set; }
        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el CodigoSunat de PAIS referente, ejm: PE", Nullable = false)]
        [MaxLength(3)]
        [Required]
        public string Pais { get; set; }
    }
}
