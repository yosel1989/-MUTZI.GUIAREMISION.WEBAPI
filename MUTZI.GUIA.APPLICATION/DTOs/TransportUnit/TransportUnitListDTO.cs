using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte
{
    public class TransportUnitListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del transporte en la bbdd", Nullable = false)]
        public int UnidadTransporteId { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve un texto, la descripción del transporte", Nullable = false)]
        public string Descripcion { get; set; }

        [JsonPropertyName("marca")]
        [SwaggerSchema("Devuelve un texto, la marca del transporte", Nullable = false)]
        public string Marca { get; set; }

        [JsonPropertyName("modelo")]
        [SwaggerSchema("Devuelve un texto, el modelo del transporte", Nullable = false)]
        public string Modelo { get; set; }

        [JsonPropertyName("placa")]
        [SwaggerSchema("Devuelve un texto, la placa del transporte", Nullable = false)]
        public string Placa { get; set; }

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Devuelve un texto, la tarjeta de circulación del transporte", Nullable = false)]
        public string Tarjeta { get; set; }

        [JsonPropertyName("tipo")]
        [SwaggerSchema("Devuelve un texto, se coloca el tipo de la unidad de transporte, Ejm: interno, externo", Nullable = false)]
        public string Tipo { get; set; }
    }
}
