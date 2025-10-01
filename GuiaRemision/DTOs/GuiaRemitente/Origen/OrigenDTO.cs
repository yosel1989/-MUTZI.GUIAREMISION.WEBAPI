using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaRemitente.Origen
{
    [SwaggerSchema("Devuelve un objeto origen, aquí devuelve el punto de partida de la guía.", Nullable = false)]
    public class OrigenDTO
    {
        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Devuelve en texto el ubigeo de la dirección del punto de partida, ejm: 150131", Nullable = false)]
        public string UbigeoId { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Devuelve en texto la dirección de origen o del punto de partida", Nullable = false)]
        public string Direccion { get; set; }
        //public string Ciudad { get; set; }

        [JsonPropertyName("distrito")]
        [SwaggerSchema("Devuelve en texto el distrito de origen o del punto de partida", Nullable = false)]
        public string Distrito { get; set; }

        [JsonPropertyName("provincia")]
        [SwaggerSchema("Devuelve en texto la provincia de origen o del punto de partida", Nullable = false)]
        public string Provincia { get; set; }

        [JsonPropertyName("departamento")]
        [SwaggerSchema("Devuelve en texto el departamento de origen o del punto de partida", Nullable = false)]
        public string Departamento { get; set; }
        
        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve en texto el codigo de PAIS referente, ejm: PE", Nullable = false)]
        public string Pais { get; set; }
    }
}
