using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente
{
    public class GuiaRemitenteUpdateDTO
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

        [JsonPropertyName("area")]
        [SwaggerSchema("Recibe un texto, se coloca el área o departamento de la empresa", Nullable = true)]
        [MaxLength(100)]
        public string? Area { get; set; }

        [JsonPropertyName("registro_mtc")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el registro MTC del vehículo, este campo solo se llena si el tipo_guia es 'TRANSPORTISTA'", Nullable = true)]
        [MaxLength(20)]
        public string? RegistroMTC { get; set; }

        [JsonPropertyName("empleado_id_edicion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = false)]
        [Required]
        public int EmpleadoIDEdicion { get; set; }

        [JsonPropertyName("empleado_nombre_edicion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha editado la transacción en el sistema", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string EmpleadoNombreEdicion { get; set; }

        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoUpdateDTO>? DocRelacionado { get; set; }

        //[JsonPropertyName("remitente_id")]
        //[Required]
        //public int RemitenteId { get; set; }
        [JsonPropertyName("destinatario_id")]
        [Required]
        public int? DestinatarioId { get; set; }

        [JsonPropertyName("proveedor_id")]
        public int? ProveedorId { get; set; }

        [JsonPropertyName("datos_envio")]
        [Required]
        public DatosEnvioUpdateDTO DatosEnvio { get; set; }

        [JsonPropertyName("origen")]
        [Required]
        public OrigenUpdateDTO Origen { get; set; }

        [JsonPropertyName("destino")]
        [Required]
        public List<DestinoUpdateDTO> Destino { get; set; }

        [JsonPropertyName("productos")]
        [Required]
        public List<ProductosUpdateDTO> Productos { get; set; }

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

        public string getTipoCodigoDocumento(string tipo, string numeroDocumento)
        {
            if (tipo == "DNI")
            {
                if (numeroDocumento.Length != 8)
                {
                    throw new Exception("El DNI: '" + numeroDocumento + "' debe tener 8 digitos.");
                }
                return EspacioNombreGuiaRemision._DocumentType01;
            }
            if (tipo == "CE")
            {
                if (numeroDocumento.Length < 8 || numeroDocumento.Length > 15)
                {
                    throw new Exception("El número de CE o Pasaporte: '" + numeroDocumento + "' debe tener entre 8 y 15 dígitos.");
                }
                return EspacioNombreGuiaRemision._DocumentType04;
            }
            if (tipo == "RUC")
            {
                if (numeroDocumento.Length != 11)
                {
                    throw new Exception("El RUC: '" + numeroDocumento + "' debe tener 11 digitos.");
                }
                return EspacioNombreGuiaRemision._DocumentType06;
            }
            if (tipo == "PASAPORTE")
                return EspacioNombreGuiaRemision._DocumentType07;
            throw new System.Exception("Tipo de documento de identidad no especificado, coloque entre: DNI, CE, RUC, PASAPORTE");
        }
    }
}
