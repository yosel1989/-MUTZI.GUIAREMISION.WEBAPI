using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Proveedor
{
    public class ProveedorListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del proveedor en la bbdd", Nullable = false)]
        public int ProveedorId { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve en texto el tipo de documento del proveedor. Valores como: DNI, CE, RUC y PASAPORTE", Nullable = false)]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Devuelve en texto el número de documento", Nullable = false)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("razon_social")]
        [SwaggerSchema("Devuelve en texto la razón social", Nullable = false)]
        public string RazonSocial { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Devuelve en texto la dirección fiscal del proveedor", Nullable = false)]
        public string Direccion { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Devuelve en texto el correo electrónico del proveedor", Nullable = false)]
        public string Email { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Devuelve en texto el Código de establecimiento o de la Sucursal", Nullable = true)]
        public string? CodigoSunat { get; set; }
    }
}
