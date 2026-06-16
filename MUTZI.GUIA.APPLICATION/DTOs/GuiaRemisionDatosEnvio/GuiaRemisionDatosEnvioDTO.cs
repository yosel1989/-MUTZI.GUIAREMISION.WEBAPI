using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.Transportista;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.DatosEnvio
{
    public class GuiaRemisionDatosEnvioDTO
    {
        [JsonPropertyName("datos_envio_id")]
        [SwaggerSchema("Devuelve en entero el ID de datos_envio asociado a la guía de remisión.", Nullable = false)]
        public int GuiaRemisionDatoEnvioId { get; set; }

        [JsonPropertyName("motivo_envio")]
        [SwaggerSchema("Devuelve en texto el motivo del envío. Ejm: Venta de mercadería, etc.", Nullable = false)]
        public string MotivoEnvio { get; set; }

        [JsonPropertyName("fecha_envio")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23, solo si es PRIVADO", Nullable = true)]
        public string? FechaEnvio { get; set; }

        [JsonPropertyName("fecha_entrega_transportista")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23, solo si es PUBLICO", Nullable = true)]
        public string? FechaEntregaTransportista { get; set; }

        [JsonPropertyName("peso_bruto")]
        [SwaggerSchema("Devuelve en decimal el peso bruto de todo el envío, Ejm: 20, 50, etc.", Nullable = false)]
        public decimal PesoBruto { get; set; }

        [JsonPropertyName("unidad_medida_id")]
        [SwaggerSchema("Devuelve en número, el id de la unidad de medida del peso bruto", Nullable = false)]
        public int UnidadMedidaId { get; set; }

        [JsonPropertyName("unidad_medida")]
        [SwaggerSchema("Devuelve en texto, la abreviatura de la unidad de medida del peso bruto", Nullable = false)]
        public string UnidadMedida { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Devuelve en texto la unidad de medida del peso bruto. Ejm: KGM ó TNE", Nullable = false)]
        public string CodigoUm { get; set; } = "KGM";

        [JsonPropertyName("traslado_vehiculo_categoria")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la placa del vehiculo pesado, solo sí se selecciono el traslado_vehiculo_categoria", Nullable = false)]
        public string? TrasladoVehiculoCategoriaPlacaVehiculo { get; set; } = null;

        [JsonPropertyName("indicador_registro_vehiculo_conductor")]
        [SwaggerSchema("Devuelve en booleano el indicador de vehículo y conductor: true o false", Nullable = true)]
        public bool IndicadorRegistroVehiculoConductor { get; set; }

        [JsonPropertyName("transportista_id")]
        public int? TransportistaId { get; set; }

        [JsonPropertyName("indicador_traslado_vehiculo_categoria")]
        [SwaggerSchema("Recibe un boolean, aquí se coloca si el traslado se realizara en vehiculos pesados", Nullable = false)]
        public bool IndicadorTrasladoVehiculoCategoria { get; set; } = false;

        [JsonPropertyName("indicador_transbordo_programado")]
        public bool IndicadorTransbordoProgramado { get; set; } = false;

        [JsonPropertyName("indicador_retorno_vehiculo_envases_vacios")]
        public bool IndicadorRetornoVehiculoEnvasesVacios { get; set; } = false;

        [JsonPropertyName("indicador_retorno_vehiculo_vacio")]
        public bool IndicadorRetornoVehiculoVacio { get; set; } = false;

        [JsonPropertyName("indicador_traslado_total_dam")]
        public bool IndicadorTrasladoTotalDam { get; set; } = false;

        [JsonPropertyName("transportista")]
        public TransportistaDTO? Transportista { get; set; }

        [JsonPropertyName("conductor")]
        public List<ConductorGuiaDTO?> GuiaRemisionConductores { get; set; } = new List<ConductorGuiaDTO?>();

        [JsonPropertyName("unidad_transporte")]
        public List<UnidadTransporteGuiaDTO?> GuiaRemisionUnidadesTransporte { get; set; } = new List<UnidadTransporteGuiaDTO?>();
    }
}
