using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DatosEnvio
{
    public class DatosEnvioDTO
    {
        [JsonPropertyName("motivo_envio")]
        [SwaggerSchema("Devuelve en texto el motivo del envío. Ejm: Venta de mercadería, etc.", Nullable = false)]
        public string MotivoEnvio { get; set; }

        [JsonPropertyName("fecha_envio")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23", Nullable = false)]
        public string FechaEnvio { get; set; }

        [JsonPropertyName("peso_bruto")]
        [SwaggerSchema("Devuelve en decimal el peso bruto de todo el envío, Ejm: 20, 50, etc.", Nullable = false)]
        public decimal PesoBruto { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Devuelve en texto la unidad de medida del peso bruto. Ejm: KGM ó TNE", Nullable = false)]
        public string CodigoUM { get; set; } = "KGM";

        [JsonPropertyName("ruc_empresa_currier")]
        [SwaggerSchema("Devuelve en texto el ruc de la empresa de Currier, este campo solo se llena si el tipo_transporte es 'PUBLICO'", Nullable = true)]
        public string? RucEmpresaCurrier { get; set; }

        [JsonPropertyName("razon_social_currier")]
        [SwaggerSchema("Devuelve en texto la razón social de la empresa de Currier, este campo solo se llena si el tipo_transporte es 'PUBLICO'", Nullable = true)]
        public string? RazonSocialCurrier { get; set; }

        [JsonPropertyName("registro_mtc_currier")]
        [SwaggerSchema("Devuelve en texto el registro MTC de la empresa de Currier, este campo solo se llena si el tipo_transporte es 'PUBLICO'", Nullable = true)]
        public string? RegistroMtcCurrier { get; set; }

        [JsonPropertyName("indicador_vehiculo_conductor")]
        [SwaggerSchema("Devuelve en booleano el indicador de vehículo y conductor: true o false", Nullable = true)]
        public bool IndicadorVehiculoConductor { get; set; }

        [JsonPropertyName("conductor")]
        public List<ConductorGuiaDTO?> ConductorGuia { get; set; }

        [JsonPropertyName("unidad_transporte")]
        public List<TransporteGuiaDTO?> TransporteGuia { get; set; }
    }
}
