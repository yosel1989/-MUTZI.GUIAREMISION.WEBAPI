using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos
{
    [SwaggerSchema("Recibe un objeto productos, aquí se detalla los productos o recursos que son enviados", Nullable = false)]
    public class ProductosCreateDTO
    {
        [JsonPropertyName("codigo")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el código del producto.", Nullable = false)]
        [MaxLength(10)]
        [Required]
        public string codigo { get; set; }

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
