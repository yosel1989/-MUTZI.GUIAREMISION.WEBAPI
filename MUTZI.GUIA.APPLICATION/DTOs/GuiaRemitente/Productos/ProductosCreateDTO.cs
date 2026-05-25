using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos
{
    [SwaggerSchema("Recibe un objeto productos, aquí se detalla los productos o recursos que son enviados", Nullable = false)]
    public class ProductosCreateDTO
    {
        [JsonPropertyName("Cantidad")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la Cantidad del producto.", Nullable = false)]
        [Required]
        public string Cantidad { get; set; }

        [JsonPropertyName("unidad_medida_id")]
        [SwaggerSchema("Recibe un múmero, aquí se coloca el id de la unidad de medida.", Nullable = false)]
        [Required]
        public int UnidadMedidaId { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la unidad de medida del producto. Ejm: KGM, PK, NIU, etc.", Nullable = false)]
        [Required]
        public string CodigoUm { get; set; }

        [JsonPropertyName("codigo")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el código del producto.", Nullable = true)]
        [MaxLength(10)]
        public string? Codigo { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la descripción del producto.", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string Descripcion { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Recibe un texto, solo sí existe el código del producto en la sunat.", Nullable = true)]
        public string? CodigoSunat { get; set; }

        [JsonPropertyName("gtin")]
        [SwaggerSchema("Recibe un texto, Codigo de barras del producto", Nullable = true)]
        public string? Gtin { get; set; }

        [JsonPropertyName("codigo_subnacional")]
        [SwaggerSchema("Recibe un texto, Codigo de la clasificación arancelaria de mercancias asignado al producto", Nullable = true)]
        public string? CodigoSubnacional { get; set; }

        [JsonPropertyName("bien_normalizado")]
        [SwaggerSchema("Recibe un boolean, indica si el producto es un bien normalizado o no", Nullable = true)]
        [Required]
        public bool BienNormalizado { get; set; }

    }
}
