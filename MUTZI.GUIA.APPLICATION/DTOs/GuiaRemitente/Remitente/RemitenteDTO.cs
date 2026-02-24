using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente
{
    [SwaggerSchema("Devuelve un objeto del remitente", Nullable = false)]
    public class RemitenteDTO
    {
        [JsonPropertyName("remitente_id")]
        [SwaggerSchema("Devuelve un entero, el ID del remitente en la bbdd", Nullable = false)]
        public int RemitenteID { get; set; }

        [JsonPropertyName("nombre_empresa")]
        [SwaggerSchema("Devuelve un texto, el nombre de la empresa asociada al remitente/sucursal.", Nullable = false)]
        public string NombreEmpresa { get; set; }

        [JsonPropertyName("ruc")]
        [SwaggerSchema("Devuelve en texto el ruc del remitente. Asimismo, ese ruc está asociado a la empresa.", Nullable = false)]
        public string Ruc { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve en texto la descripción o nombre del remitente/sucursal.", Nullable = false)]
        public string Descripcion { get; set; }

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

        [JsonPropertyName("emailFacturador")]
        [SwaggerSchema("Devuelve un texto, el correo electrónico del facturador del remitente", Nullable = false)]
        public string EmailFacturador { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve un texto, el codigo de PAIS referente, ejm: PE", Nullable = false)]
        public string Pais { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Devuelve un texto, la serie que posteriormente se usa en las guías de remisión, ejm: 001, 002", Nullable = true)]
        public string Serie { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("fecha_edicion")]
        public DateTime? FechaEdicion { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        public int EmpleadoIdCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        public string EmpleadoNombreCreacion { get; set; }

        [JsonPropertyName("empleado_id_edicion")]
        public int? EmpleadoIdEdicion { get; set; }

        [JsonPropertyName("empleado_nombre_edicion")]
        public string? EmpleadoNombreEdicion { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }
    }
}
