using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor
{
    [SwaggerSchema("Devuelve un objeto de proveedor", Nullable = false)]
    public class ProveedorDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del proveedor en la bbdd", Nullable = false)]
        public int ProveedorID { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve en texto el tipo de documento del proveedor. Valores como: DNI, CE, RUC y PASAPORTE", Nullable = false)]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Devuelve en texto el número de documento", Nullable=false)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("razon_social")]
        [SwaggerSchema("Devuelve en texto la razón social", Nullable = false)]
        public string RazonSocial { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Devuelve en texto el ubigeo de la dirección fiscal del proveedor, ejm: 150131", Nullable = false)]
        public string UbigeoId { get; set; }

        [JsonPropertyName("departamento")]
        [SwaggerSchema("Devuelve en texto el departamento de la dirección fiscal del proveedor", Nullable = false)]
        public string Departamento { get; set; }

        [JsonPropertyName("provincia")]
        [SwaggerSchema("Devuelve en texto la provincia de la dirección fiscal del proveedor", Nullable = false)]
        public string Provincia { get; set; }

        [JsonPropertyName("distrito")]
        [SwaggerSchema("Devuelve en texto el distrito de la dirección fiscal del proveedor", Nullable = false)]
        public string Distrito { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Devuelve en texto la dirección fiscal del proveedor", Nullable = false)]
        public string Direccion { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Devuelve en texto el correo electrónico del proveedor", Nullable = false)]
        public string Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve en texto el codigo de PAIS referente, ejm: PE", Nullable = false)]
        public string Pais { get; set; }

        [JsonPropertyName("codigoSunat")]
        [SwaggerSchema("Devuelve en texto el Código de establecimiento o de la Sucursal", Nullable = true)]
        public string? CodigoSunat { get; set; }

        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve en texto el estado del proveedor solo puede ser: ACTIVO o INACTIVO", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado del proveedor en la bbdd", Nullable = false)]
        public int IdEstado { get; set; }
    }
}
