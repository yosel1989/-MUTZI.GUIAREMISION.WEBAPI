using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino
{
    [SwaggerSchema("Devuelve el objeto destino")]
    public class DestinoDTO
    {
        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Devuelve en texto el ubigeo de la dirección fiscal del proveedor, ejm: 150131", Nullable = false)]
        public string UbigeoId { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Devuelve en texto la dirección de destino o del punto de llegada", Nullable = false)]
        public string Direccion { get; set; }
        //public string Ciudad { get; set; }

        [JsonPropertyName("distrito")]
        [SwaggerSchema("Devuelve en texto el distrito de destino o del punto de llegada", Nullable = false)]
        public string Distrito { get; set; }

        [JsonPropertyName("provincia")]
        [SwaggerSchema("Devuelve en texto la provincia de destino o del punto de llegada", Nullable = false)]
        public string Provincia { get; set; }

        [JsonPropertyName("departamento")]
        [SwaggerSchema("Devuelve en texto el departamento de destino o del punto de llegada", Nullable = false)]
        public string Departamento { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve en texto el codigo de PAIS referente, ejm: PE", Nullable = false)]
        public string Pais { get; set; }
    }
}
