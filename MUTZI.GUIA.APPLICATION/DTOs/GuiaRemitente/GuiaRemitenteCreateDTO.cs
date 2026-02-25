using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente
{
    public class GuiaRemitenteCreateDTO
    {

        [JsonPropertyName("tipo_transporte")]
        [SwaggerSchema("Recibe un texto y solo pueden ser: PUBLICO ó PRIVADO", Nullable = false)]
        [StringLength(7)]
        [MinLength(7)]
        [Required]
        public string TipoTransporte { get; set; }//PUBLICO, PRIVADO

        [JsonPropertyName("tipo_traslado")]
        [SwaggerSchema("Recibe un texto y solo pueden ser: VENTA, TRASLADO ó COMPRA", Nullable = false)]
        [MaxLength(8)]
        [MinLength(5)]
        [Required]
        public string TipoTraslado { get; set; }//VENTA,TRASLADO,COMPRA

        [JsonPropertyName("fecha")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23", Nullable = false)]
        [StringLength(10)]
        [MinLength(10)]
        [Required]
        public string Fecha { get; set; }

        [JsonPropertyName("hora")]
        [SwaggerSchema("Recibe un texto de hora en formato de 24 horas(hh:mm:ss); por ejemplo: 14:35:00", Nullable = false)]
        [StringLength(8)]
        [MinLength(8)]
        [Required]
        public string Hora { get; set; }

        [JsonPropertyName("observacion")]
        [SwaggerSchema("Recibe un texto, donde se detalla una observación o comentario sobre la guía de remisión", Nullable = true)]
        [MaxLength(100)]
        public string? Observacion { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = false)]
        [Required]
        public int EmpleadoIDCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha registrado la transacción en el sistema", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string EmpleadoNombreCreacion { get; set; }

        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoCreateDTO>? DocRelacionado { get; set; }

        [JsonPropertyName("remitente")]
        [Required]
        public RemitenteCreateDTO Remitente { get; set; }

        [JsonPropertyName("destinatario")]
        [Required]
        public DestinatarioCreateDTO? Destinatario { get; set; }

        [JsonPropertyName("proveedor")]
        public ProveedorCreateDTO? Proveedor { get; set; }
        
        [JsonPropertyName("datos_envio")]
        [Required]
        public DatosEnvioCreateDTO DatosEnvio { get; set; }

        [JsonPropertyName("origen")]
        [Required]
        public OrigenCreateDTO Origen { get; set; }

        [JsonPropertyName("destino")]
        [Required]
        public List<DestinoCreateDTO> Destino { get; set; }

        [JsonPropertyName("productos")]
        [Required]
        public List<ProductosCreateDTO> Productos { get; set; }

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
