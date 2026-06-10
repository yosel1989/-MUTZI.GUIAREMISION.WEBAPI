using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio
{
    public class DatosEnvioGT_DTO
    {
        [JsonPropertyName("datos_envio_id")]
        [SwaggerSchema("Devuelve en entero el ID de datos_envio asociado a la guía de remisión.", Nullable = false)]
        public int DatosEnvioId { get; set; }

        [JsonPropertyName("fecha_envio")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23, solo si es PRIVADA", Nullable = false)]
        public string? FechaEnvio { get; set; }

        [JsonPropertyName("fecha_entrega_transportista")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23, solo si es PUBLICA", Nullable = false)]
        public string? FechaEntregaTransportista { get; set; }

        [JsonPropertyName("peso_bruto")]
        [SwaggerSchema("Devuelve un decimal, el peso bruto de todo el envío, Ejm: 20, 50, etc.", Nullable = false)]
        public decimal PesoBruto { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Devuelve un texto, la unidad de medida del peso bruto. Ejm: KGM ó TNE", Nullable = false)]
        public string CodigoUM { get; set; } = "KGM";
        //EN caso de subcontratista

        [JsonPropertyName("ruc_empresa_currier")]
        [SwaggerSchema("Devuelve un texto y es el ruc de la empresa de Currier, este campo solo se muestra si el pagador del flete es un subcontratador", Nullable = false)]
        public string? RucEmpresaCurrier { get; set; }

        [JsonPropertyName("razon_social_currier")]
        [SwaggerSchema("Devuelve un texto y es la razón social de la empresa de Currier, este campo solo se llena si el pagador del flete es un subcontratador", Nullable = false)]
        public string? RazonSocialCurrier { get; set; }

        [JsonPropertyName("registro_mtc_currier")]
        [SwaggerSchema("Devuelve un texto y es el registro MTC de la empresa de Currier, este campo solo devuelve si el pagador del flete es un subcontratador", Nullable = false)]
        public string? RegistroMtcCurrier { get; set; }

        // Conductor
        [JsonPropertyName("conductor")]
        [SwaggerSchema("Devuelve el conductor asociado a los datos de envío.", Nullable = false)]
        public List<ConductorGuiaDTO> ConductorGuia { get; set; }

        // Unidad de transporte
        [JsonPropertyName("unidad_transporte")]
        [SwaggerSchema("Devuelve el transporte asociado a los datos de envío.", Nullable = false)]
        public List<UnidadTransporteGuiaDTO> TransporteGuia { get; set; }
    }
}
