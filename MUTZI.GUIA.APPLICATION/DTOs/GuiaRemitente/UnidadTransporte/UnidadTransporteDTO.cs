using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte
{
    [SwaggerSchema("Devuelve un objeto de unidad de transporte")]
    public class UnidadTransporteDTO
    {
        [JsonPropertyName("transporte_id")]
        [SwaggerSchema("Devuelve un entero, el ID del transporte en la bbdd", Nullable = false)]
        public int TransporteID { get; set; }

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

        [JsonPropertyName("numero_registro_mtc")]
        [SwaggerSchema("Devuelve un texto, el número de registro MTC del transporte", Nullable = false)]
        public string NumeroRegistroMTC { get; set; }

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Devuelve un texto, la tarjeta de circulación del transporte", Nullable = false)]
        public string Tarjeta { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
