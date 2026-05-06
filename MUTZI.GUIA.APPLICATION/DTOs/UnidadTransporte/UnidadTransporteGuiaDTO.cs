using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte
{
    [SwaggerSchema("Devuelve un objeto transporte asociado a los datos de envío de una guía de remisión")]
    public class UnidadTransporteGuiaDTO
    {
        [JsonPropertyName("transporte_guia_id")]
        [SwaggerSchema("Devuelve en entero el ID del transporte asociado a los datos de envío de una guía de remisión.", Nullable = false)]
        public int TransporteGuidId { get; set; }

        [JsonPropertyName("transporte_id")]
        [SwaggerSchema("Devuelve en entero el ID del transporte en la base de datos", Nullable = false)]
        public int TransporteID { get; set; }

        [JsonPropertyName("datos_envio_id")]
        [SwaggerSchema("Devuelve en entero el ID de los datos de envío asociados a la guía de remisión", Nullable = false)]
        public int DatosEnvioID { get; set; }

        [JsonPropertyName("placa")]
        [SwaggerSchema("Devuelve en texto la placa de la unidad de transporte", Nullable = false)]
        public string Placa { get; set; }

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Devuelve en texto la tarjeta de la unidad de transporte", Nullable = false)]
        public string Tarjeta { get; set; }

        [JsonPropertyName("cod_emisor_vehicular")]
        [SwaggerSchema("Devuelve un texto, el código del emisor vehicular del transporte", Nullable = true)]
        public string? CodEmisorVehicular { get; set; }

        [JsonPropertyName("emisor_vehicular")]
        [SwaggerSchema("Devuelve un texto, el emisor vehicular del transporte", Nullable = true)]
        public string? EmisorVehicular { get; set; }

        [JsonPropertyName("nro_autorizacion")]
        [SwaggerSchema("Devuelve un texto, el número de autorización del transporte", Nullable = true)]
        public string? NroAutorizacion { get; set; }
    }
}
