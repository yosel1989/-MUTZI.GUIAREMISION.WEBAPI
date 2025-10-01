using GuiaRemision.Constantes;
using GuiaRemision.DTOs.GuiaRemitente.Destinatario;
using GuiaRemision.DTOs.GuiaRemitente.DatosEnvio;
using GuiaRemision.DTOs.GuiaRemitente.Destino;
using GuiaRemision.DTOs.GuiaRemitente.Origen;
using GuiaRemision.DTOs.GuiaRemitente.Productos;
using GuiaRemision.DTOs.GuiaRemitente.Proveedor;
using GuiaRemision.DTOs.GuiaRemitente.Remitente;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace GuiaRemision.DTOs.GuiaRemitente
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
        [SwaggerSchema("Devuelve un texto, donde se detalla una observación o comentario sobre la guía de remisión", Nullable = false)]
        public string Observacion { get; set; }

        // Documento de referencia (factura/boleta vinculada)
        [JsonPropertyName("tipo_doc_ref")]
        [SwaggerSchema("Devuelve un texto, es el tipo de documento vinculado como referencia en la guía de remisión, los tipos definidos son: FACTURA, BOLETA, NOTA DE CREDITO Y NOTA DE DEBITO", Nullable = false)]
        public string TipoDocRef { get; set; }

        [JsonPropertyName("numero_doc_ref")]
        [SwaggerSchema("Devuelve un texto, aquí se hace referencia al numero del documento vinculado en la guía de remisión. Ejm: F025-00000003", Nullable = false)]
        public string NumeroDocRef { get; set; }

        [JsonPropertyName("ruc_doc_ref")]
        [SwaggerSchema("Devuelve un texto, aquí se coloca el ruc del documento vinculado como referencia en la guía de remisión.", Nullable = false)]
        public string RucDocRef { get; set; }
        public RemitenteDTO Remitente { get; set; }
        public DestinatarioDTO Destinatario { get; set; }
        public ProveedorDTO? Proveedor { get; set; }
        public DatosEnvioDTO DatosEnvio { get; set; }
        public OrigenDTO Origen { get; set; }
        public List<DestinoDTO> Destino { get; set; }
        public List<ProductosDTO> Productos { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string DocumentoAdicinalTipo()
        {
            if (string.IsNullOrEmpty(this.TipoDocRef))
                throw new System.Exception("tipo_doc_ref no puede estar vacío al generar el tipo de documento adicional.");

            switch (this.TipoDocRef.ToUpper())
            {
                case "FACTURA":
                    return "01"; // Factura
                case "BOLETA":
                    return "03"; // Boleta
                case "NOTA DE CREDITO":
                    return "07"; // Nota de crédito
                case "NOTA DE DEBITO":
                    return "08"; // Nota de débito
                default:
                    throw new System.Exception("Catálogo No. 01: (tipo_doc_ref)Tipo de documento vinculado como referencia no definido.");
            }
        }

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
