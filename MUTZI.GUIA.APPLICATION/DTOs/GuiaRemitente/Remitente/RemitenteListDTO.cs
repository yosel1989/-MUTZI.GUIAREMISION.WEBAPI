using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente
{
    public class RemitenteListDTO
    {
        [JsonPropertyName("id")]
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

        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve un texto, el codigo de PAIS referente, ejm: PE", Nullable = false)]
        public string Pais { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Devuelve un texto, la serie que posteriormente se usa en las guías de remisión, ejm: 001, 002", Nullable = true)]
        public string Serie { get; set; }
    }
}
