using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos
{
    [SwaggerSchema("Devuelve un objeto de productos", Nullable = false)]
    public class ProductosDTO
    {
        [JsonPropertyName("codigo")]
        [SwaggerSchema("Devuelve en texto el código del producto.", Nullable = false)]
        public string codigo { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve en texto la descripción del producto.", Nullable = false)]
        public string descripcion { get; set; }

        [JsonPropertyName("cantidad")]
        [SwaggerSchema("Devuelve en texto la cantidad del producto.", Nullable = false)]
        public string cantidad { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Devuelve en texto la unidad de medida del producto. Ejm: KGM, PK, NIU, etc.", Nullable = false)]
        public string CodigoUM { get; set; }

        [JsonPropertyName("descripcion_um")]
        [SwaggerSchema("Devuelve en texto la descripción de la unidad de medida del producto. Ejm: Kilogramo, Paquete, Unidad, etc.", Nullable = false)]
        public string DescripcionUM { get; set; }
    }
}
