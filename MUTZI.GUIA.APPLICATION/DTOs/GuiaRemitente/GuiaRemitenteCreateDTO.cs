using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Proveedor;
using MUTZI.GUIA.APPLICATION.Enums;
using MUTZI.GUIA.APPLICATION.DTOs.DatosEnvio;

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

        [JsonPropertyName("motivo_traslado_id")]
        [SwaggerSchema("Recibe un número, el id debe existir en el catalogo de motivos de traslado", Nullable = false)]
        [Required]
        public int MotivoTrasladoId { get; set; }

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

        [JsonPropertyName("registro_mtc")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el registro MTC del vehículo, este campo solo se llena si el tipo_guia es 'TRANSPORTISTA'", Nullable = true)]
        [MaxLength(20)]
        public string? RegistroMTC { get; set; }

        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoCreateDTO>? DocRelacionado { get; set; }

        [JsonPropertyName("remitente_id")]
        [Required]
        public int RemitenteId { get; set; }

        [JsonPropertyName("remitente")]
        [Required]
        public EntityBranchDTO Remitente { get; set; }

        [JsonPropertyName("destinatario_id")]
        [Required]
        public int DestinatarioId { get; set; }

        [JsonPropertyName("destinatario")]
        [Required]
        public EntityBranchDTO Destinatario { get; set; }

        [JsonPropertyName("destinatario_email")]
        [Required]
        public List<string> DestinatarioEmail { get; set; } = new List<string>();

        [JsonPropertyName("proveedor_id")]
        [SwaggerSchema("Recibe un objeto con el ID del proveedor que va a ser asociado a la guía", Nullable = true)]
        public int? ProveedorId { get; set; }

        [JsonPropertyName("proveedor")]
        public ProveedorCreateGuiaDTO? Proveedor { get; set; }


        [JsonPropertyName("datos_envio")]
        [Required]
        public GuiaRemisionDatosEnvioCreateDTO DatosEnvio { get; set; }

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
            switch (this.MotivoTrasladoId)
            {
                case (int)SunatCatalogoEnum.MotivoTraslado.VENTA:
                    if (this.Remitente.Ruc == this.Destinatario.Ruc)
                        throw new System.Exception("Si el tipo de traslado es VENTA, el 'Tipo y Número de documento de identidad del destinatario' no puede ser igual al 'Tipo y Número de documento de identidad del remitente'.");
                    return "01";
                case (int)SunatCatalogoEnum.MotivoTraslado.COMPRA:
                    if (this.Remitente.Ruc != this.Remitente.Ruc)
                        throw new System.Exception("Si el tipo de traslado es COMPRA, el 'Tipo y Número de documento de identidad del destinatario' debe ser igual al 'Tipo y Número de documento de identidad del remitente'.");
                    return "02";
                case (int)SunatCatalogoEnum.MotivoTraslado.TRASLADO_ENTRE_ESTABLECIMIENTOS_DE_LA_MISMA_EMPRESA:
                    if (this.Remitente.Ruc != this.Remitente.Ruc)
                        throw new System.Exception("Si el tipo de traslado es TRASLADO, el 'Tipo y Número de documento de identidad del destinatario' debe ser igual al 'Tipo y Número de documento de identidad del remitente'.");
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
                    throw new System.Exception("Catálogo No. 18: Códigos – Tipo de UnidadTransporte no definido. Debe elegir entre: ´PRIVADO ó PUBLICO");
            }
        }

        public string getTipoCodigoDocumento(string tipo, string numeroDocumento)
        {
            if (tipo == "DNI")
            {
                if (numeroDocumento.Length != 8)
                {
                    throw new Exception("El DNI: '"+ numeroDocumento + "' debe tener 8 digitos.");
                }
                return EspacioNombreGuiaRemision._DocumentType01;
            }
            if (tipo == "CE")
            {
                if (numeroDocumento.Length < 8 || numeroDocumento.Length > 15)
                {
                    throw new Exception("El número de CE o Pasaporte: '"+numeroDocumento+"' debe tener entre 8 y 15 dígitos.");
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
            if (tipo == "PASAPORTE" || tipo == "PAS")
                return EspacioNombreGuiaRemision._DocumentType07;
            throw new System.Exception("Tipo de documento de identidad no especificado, coloque entre: DNI, CE, RUC, PASAPORTE");
        }


        public void ValidarBienesNormalizados()
        {
            var tieneBienesNormalizados = this.Productos.Any(x => x.IndicadorBienNormalizado == true);
            if (tieneBienesNormalizados)
            {
                if(this.DocRelacionado is null)
                {
                    throw new System.Exception("Debe añadir el documento relacionado para el bien normalizado.");
                }
                else
                {
                    if (!this.DocRelacionado.Any(x => x.TipoDocumentoReferenciaCodigo == "49" || x.TipoDocumentoReferenciaCodigo == "80"))
                    {
                        throw new System.Exception("Debe añadir un documento de sustento para el bien normalizado.");
                    }
                }
            }
        }
    }
}
