using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EstablecimientoListToModalDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del remitente en la bbdd", Nullable = false)]
        public int EstablecimientoId { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve el nombre asociada al establecimiento.", Nullable = false)]
        public string Descripcion { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Devuelve el código de establecimiento registrado en SUNAT. Ejem: 0001, 0012", Nullable = false)]
        public string CodigoSunat { get; set; }

        [JsonPropertyName("entidad")]
        [SwaggerSchema("Devuelve el nombre de la entidad a la que pertenece.", Nullable = false)]
        public string Entidad { get; set; }
    }
}
