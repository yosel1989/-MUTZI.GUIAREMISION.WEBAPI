using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos
{
    [SwaggerSchema("Devuelve un objeto de productos", Nullable = false)]
    public class ProductosDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve en entero el ID de la guía de detalle.", Nullable = false)]
        public int GuiaRemisionDetalleId { get; set; }

        [JsonPropertyName("guia_remision_id")]
        [SwaggerSchema("Devuelve en entero el ID de la guía a la que pertenece.", Nullable = false)]
        public int GuiaRemisionId { get; set; }

        [JsonPropertyName("Cantidad")]
        [SwaggerSchema("Devuelve en texto la Cantidad del producto.", Nullable = false)]
        public string Cantidad { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Devuelve en texto la unidad de medida del producto. Ejm: KGM, PK, NIU, etc.", Nullable = false)]
        public string CodigoUm { get; set; }

        [JsonPropertyName("Codigo")]
        [SwaggerSchema("Devuelve en texto el código del producto.", Nullable = true)]
        public string? Codigo { get; set; }

        [JsonPropertyName("Descripcion")]
        [SwaggerSchema("Devuelve en texto la descripción del producto.", Nullable = false)]
        public string Descripcion { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Devuelve en texto, el código de sunat solo sí se registró.", Nullable = true)]
        public string? CodigoSunat { get; set; }

        [JsonPropertyName("gtin")]
        [SwaggerSchema("Devuelve en texto, el gtin del producto.", Nullable = true)]
        public string? Gtin { get; set; }

        [JsonPropertyName("codigo_subnacional")]
        [SwaggerSchema("Devuelve en texto, el código subnacional del producto solo sí se registró.", Nullable = true)]
        public string? CodigoSubnacional { get; set; }

        [JsonPropertyName("bien_normalizado")]
        [SwaggerSchema("Devuelve un boolean, true si se registro como un bien normalizado, false sino.", Nullable = false)]
        public bool BienNormalizado { get; set; } = false;

        [JsonPropertyName("descripcion_um")]
        [SwaggerSchema("Devuelve en texto la descripción de la unidad de medida del producto. Ejm: Kilogramo, Paquete, Unidad, etc.", Nullable = false)]
        public string DescripcionUM { get; set; }
    }
}
