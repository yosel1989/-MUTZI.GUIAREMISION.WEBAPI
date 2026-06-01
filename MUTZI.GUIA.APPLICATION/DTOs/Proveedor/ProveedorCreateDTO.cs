using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Proveedor
{
    [SwaggerSchema("Recibe un objeto proveedor, solo si el tipo_traslado es 'COMPRA', entonces se completa el proveedor, de lo contrario, no se define el objeto proveedor", Nullable = true)]
    public class ProveedorCreateDTO
    {
        [JsonPropertyName("tipo_documento_id")]
        [SwaggerSchema("Recibe un número, aquí se coloca el id del tipo de documento del proveedor.", Nullable = false)]
        [Required]
        public int TipoDocumentoId { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el número de documento del proveedor.", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del proveedor es obligatorio")]
        [MaxLength(21)]
        [MinLength(8)]
        public string NumeroDocumento { get; set; }


        [JsonPropertyName("razon_social")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la razón social del proveedor.", Nullable = false)]
        [MaxLength(200)]
        [Required]
        public string RazonSocial { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del proveedor, ejm: 150131", Nullable = true)]
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string? UbigeoId { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la dirección fiscal del proveedor", Nullable = true)]
        [MaxLength(250)]
        public string? Direccion { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del proveedor", Nullable = true)]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string? Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el CodigoSunat de PAIS referente, ejm: PE", Nullable = true)]
        [MaxLength(3)]
        public string? Pais { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el Código de establecimiento o de la Sucursal", Nullable = true)]
        [StringLength(4, ErrorMessage = "El código de establecimiento requiere 4 caracteres")]
        public string? CodigoSunat { get; set; }

    }
}
