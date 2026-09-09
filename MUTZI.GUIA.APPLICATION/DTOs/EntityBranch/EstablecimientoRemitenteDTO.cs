using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EstablecimientoRemitenteDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del establecimiento que emitira la guía o comprobante en la bbdd", Nullable = false)]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("entity_id")]
        public int? EntityId { get; set; }

        [JsonPropertyName("entity_name")]
        public string? EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        public string? EntityDocumentNumber { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Devuelve un texto, aquí devuelve el ubigeo de la dirección fiscal del remitente, ejm: 150131", Nullable = false)]
        public string UbigeoID { get; set; }

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
        [SwaggerSchema("Devuelve un texto, el correo electrónico del remitente", Nullable = false)]
        public string Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve un texto, el CodeSunat de PAIS referente, ejm: PE", Nullable = false)]
        public string Pais { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Devuelve un texto, la serie que posteriormente se usa en las guías de remisión, ejm: 001, 002", Nullable = true)]
        public string Serie { get; set; }

        [JsonPropertyName("nueva_serie")]
        [SwaggerSchema("Devuelve un texto, la nueva serie que se usará en las guías de remisión, ejm: T001, V001", Nullable = true)]
        public string NuevaSerie { get; set; }

        [JsonPropertyName("nuevo_correlativo")]
        [SwaggerSchema("Devuelve un entero, el nuevo correlativo que se usará en las guías de remisión, ejm: 1, 2, 3", Nullable = true)]
        public int? NuevoCorrelativo { get; set; }

        [JsonPropertyName("nuevo_numero_guia")]
        [SwaggerSchema("Devuelve un texto, el nuevo número de guía que se usará en las guías de remisión, ejm: T001-00000001", Nullable = true)]
        public string NuevoNumeroGuia { get; set; }

        [JsonPropertyName("area")]
        [SwaggerSchema("Devuelve un texto, el area al que pertenece la serie , ejm: VENTAS, LOGISTICA", Nullable = true)]
        public string? Area { get; set; }
    }
}
