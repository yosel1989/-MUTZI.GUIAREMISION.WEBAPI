using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos
{
    public class ProductosUpdateDTO
    {
        [JsonPropertyName("guia_detalle_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID de la guía de detalle, ojo: Colocar el mismo ID si el registro solo se está actualizando, en caso de ser nuevo registro, es decir una nueva línea que se está insertando, entonces se debe dejar este campo con valor de cero: '0'.", Nullable = false)]
        [Required]
        public int GuiaDetalleID { get; set; }

        [JsonPropertyName("codigo")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el código del producto.", Nullable = true)]
        [MaxLength(10)]
        public string? codigo { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la descripción del producto.", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string descripcion { get; set; }

        [JsonPropertyName("cantidad")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la cantidad del producto.", Nullable = false)]
        [Required]
        public string cantidad { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la unidad de medida del producto. Ejm: KGM, PK, NIU, etc.", Nullable = false)]
        [Required]
        public string CodigoUM { get; set; }
    }
}
