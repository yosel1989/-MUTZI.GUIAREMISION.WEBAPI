using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente
{
    [SwaggerSchema("Devuelve un objeto del remitente", Nullable = false)]
    public class RemitenteDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del remitente en la bbdd", Nullable = false)]
        public int RemitenteID { get; set; }

        [JsonPropertyName("ruc")]
        [SwaggerSchema("Devuelve un texto, es el ruc del remitente", Nullable = false)]
        public string Ruc { get; set; }

        [JsonPropertyName("razon_social")]
        [SwaggerSchema("Devuelve un texto, la razón social del remitente", Nullable = false)]
        public string RazonSocial { get; set; }

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

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Devuelve un texto, la direccion fiscal del remitente", Nullable = false)]
        public string Direccion { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Devuelve un texto, el correo electrónico del remitente", Nullable = false)]
        public string Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve un texto, el codigo de PAIS referente, ejm: PE", Nullable = false)]
        public string Pais { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Devuelve un texto, la serie que posteriormente se usa en las guías de remisión, ejm: 001, 002", Nullable = true)]
        public DateTime FechaCreacion { get; set; }
    }
}
