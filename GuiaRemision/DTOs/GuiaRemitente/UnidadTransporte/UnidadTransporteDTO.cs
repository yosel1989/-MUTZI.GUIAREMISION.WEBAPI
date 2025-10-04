using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaRemitente.UnidadTransporte
{
    [SwaggerSchema("Devuelve un objeto de unidad de transporte")]
    public class UnidadTransporteDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del transporte en la bbdd", Nullable = false)]
        public int TransporteID { get; set; }

        [JsonPropertyName("placa")]
        [SwaggerSchema("Recibe un texto, se coloca la placa de la unidad_transporte", Nullable = false)]
        public string Placa { get; set; }

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Recibe un texto, se coloca la tarjeta de la unidad_transporte", Nullable = false)]
        public string Tarjeta { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
