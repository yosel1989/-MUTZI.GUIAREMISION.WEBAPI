using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EntityBranchListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del remitente en la bbdd", Nullable = false)]
        public int EstablecimientoId { get; set; }

        [JsonPropertyName("description")]
        [SwaggerSchema("Devuelve el nombre asociada al establecimiento.", Nullable = false)]
        public string Description { get; set; }

        [JsonPropertyName("ruc")]
        [SwaggerSchema("Devuelve en texto el ruc asociado a la empresa.", Nullable = false)]
        public string Ruc { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Devuelve un texto, aquí devuelve el ubigeo de la dirección fiscal del remitente, ejm: 150131", Nullable = false)]
        public string UbigeoId { get; set; }

        [JsonPropertyName("departamento")]
        [SwaggerSchema("Devuelve un texto, el departamento de la dirección fiscal del remitente", Nullable = false)]
        public string Departamento { get; set; }

        [JsonPropertyName("provincia")]
        [SwaggerSchema("Devuelve un texto, la provincia de la dirección fiscal del remitente", Nullable = false)]
        public string Provincia { get; set; }

        [JsonPropertyName("distrito")]
        [SwaggerSchema("Devuelve un texto, el distrito de la dirección fiscal del remitente", Nullable = false)]
        public string Distrito { get; set; }

        [JsonPropertyName("address")]
        [SwaggerSchema("Devuelve un texto, la direccion fiscal del remitente", Nullable = false)]
        public string Address { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Devuelve un texto, el correo electrónico del remitente", Nullable = true)]
        public string? Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve un texto, el CodeSunat de PAIS referente, ejm: PE", Nullable = false)]
        public string Pais { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Devuelve un texto, la serie que posteriormente se usa en las guías de remisión, ejm: 001, 002", Nullable = true)]
        public string Serie { get; set; }

        [JsonPropertyName("area")]
        [SwaggerSchema("Devuelve un texto, el area al que pertenece la serie , ejm: VENTAS, LOGISTICA", Nullable = true)]
        public string? Area { get; set; }
    }
}
