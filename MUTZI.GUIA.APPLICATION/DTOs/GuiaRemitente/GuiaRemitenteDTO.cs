using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente
{
    public class GuiaRemitenteDTO
    {
        [JsonPropertyName("guia_id")]
        [SwaggerSchema("Devuelve un entero, es el número ID de la guía.", Nullable = false)]
        public int GuiaID { get; set; }

        [JsonPropertyName("tipo_transporte")]
        [SwaggerSchema("Devuelve un texto y solo pueden ser: PUBLICO ó PRIVADO", Nullable = false)]
        public string TipoTransporte { get; set; }

        [JsonPropertyName("tipo_traslado")]
        [SwaggerSchema("Devuelve un texto y solo pueden ser: VENTA, TRASLADO ó COMPRA", Nullable = false)]
        public string TipoTraslado { get; set; }

        [JsonPropertyName("serie_correlativo")]
        [SwaggerSchema("Devuelve un texto", Nullable = false)]
        public string SerieCorrelativo { get; set; }

        [JsonPropertyName("fecha")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23", Nullable = false)]
        public string Fecha { get; set; }

        [JsonPropertyName("hora")]
        [SwaggerSchema("Devuelve un texto de hora en formato de 24 horas(hh:mm:ss); por ejemplo: 14:35:00", Nullable = false)]
        public string Hora { get; set; }

        [JsonPropertyName("observacion")]
        [SwaggerSchema("Devuelve un texto, donde se detalla una observación o comentario sobre la guía de remisión", Nullable = true)]
        public string? Observacion { get; set; }

        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoDTO> DocRelacionados { get; set; }
        public RemitenteDTO Remitente { get; set; }
        public DestinatarioDTO Destinatario { get; set; }
        public ProveedorDTO? Proveedor { get; set; }
        public DatosEnvioDTO DatosEnvio { get; set; }
        public OrigenDTO Origen { get; set; }
        public List<DestinoDTO> Destino { get; set; }
        public List<ProductosDTO> Productos { get; set; }
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        [SwaggerSchema("Devuelve un entero, ID del empleado que ha realizado la transacción", Nullable = false)]
        public int EmpleadoIDCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Devuelve un texto, el nombre del empleado que ha registrado la transacción en el sistema", Nullable = false)]
        public string EmpleadoNombreCreacion { get; set; }

        public string TipoTrasladoCode20()
        {
            switch (this.TipoTraslado)
            {
                case "VENTA":
                    return "01";
                case "COMPRA":
                    return "02";
                case "TRASLADO":
                    return "04";
                default:
                    throw new System.Exception("Catálogo No. 20: Códigos – Motivos de traslado no definido. Debe elegir entre: VENTA, COMPRA ó TRASLADO");
            }
        }

        public string TipoTRansporteCode18()
        {
            switch (this.TipoTransporte)
            {
                case "PUBLICO":
                    return "01";
                case "PRIVADO":
                    return "02";
                default:
                    throw new System.Exception("Catálogo No. 18: Códigos – Tipo de Transporte no definido. Debe elegir entre: ´PRIVADO ó PUBLICO");
            }
        }

        public string getTipoCodigoDocumento(string tipo)
        {
            if (tipo == "DNI")
                return EspacioNombreGuiaRemision._DocumentType01;
            if (tipo == "CE")
                return EspacioNombreGuiaRemision._DocumentType04;
            if (tipo == "RUC")
                return EspacioNombreGuiaRemision._DocumentType06;
            if (tipo == "PASAPORTE")
                return EspacioNombreGuiaRemision._DocumentType07;
            throw new System.Exception("Tipo de documento de identidad no especificado, coloque entre: DNI, CE, RUC, PASAPORTE");
        }
    }
}
