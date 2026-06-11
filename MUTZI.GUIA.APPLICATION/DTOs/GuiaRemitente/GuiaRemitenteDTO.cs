using MUTZI.GUIA.APPLICATION.DTOs.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.EmailDestinatarioGuia;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.VentaConfig;
using MUTZI.GUIA.APPLICATION.Enums;
using MUTZI.GUIA.APPLICATION.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente
{
    public class GuiaRemitenteDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, es el número ID de la guía.", Nullable = false)]
        public int GuiaRemisionId { get; set; }

        [JsonPropertyName("tipo_guia")]
        [SwaggerSchema("Devuelve en texto el tipo de guía: REMITENTE ó TRANSPORTISTA", Nullable = false)]
        public string TipoGuia { get; set; }

        [JsonPropertyName("ruc")]
        [SwaggerSchema("Devuelve en texto el ruc del facturador asociado", Nullable = false)]
        public string Ruc { get; set; }

        [JsonPropertyName("empresa")]
        [SwaggerSchema("Devuelve un objeto de tipo empresa", Nullable = false)]
        public EmpresaDTO Empresa { get; set; }

        [JsonPropertyName("email_facturador")]
        [SwaggerSchema("Devuelve en texto el correo electrónico del facturador asociado", Nullable = false)]
        public string EmailFacturador { get; set; }

        [JsonPropertyName("tipo_transporte")]
        [SwaggerSchema("Devuelve un texto y solo pueden ser: PUBLICO ó PRIVADO", Nullable = false)]
        public string TipoTransporte { get; set; }

        [JsonPropertyName("motivo_traslado_id")]
        [SwaggerSchema("Devuelve un número y solo puede ser uno registrado en el catalogo de motivos de traslado", Nullable = false)]
        public int MotivoTrasladoId { get; set; }

        [JsonPropertyName("motivo_traslado")]
        [SwaggerSchema("Devuelve un texto, la descripción del motivo de traslado", Nullable = false)]
        public string MotivoTrasladoDescripcion { get; set; }

        [JsonPropertyName("motivo_traslado_codigo")]
        [SwaggerSchema("Devuelve un texto, el código del catalogo de sunat en relación al motivo de traslado", Nullable = false)]
        public string MotivoTrasladoCodigo { get; set; }

        [JsonPropertyName("serie_correlativo")]
        [SwaggerSchema("Devuelve un texto", Nullable = false)]
        public string SerieCorrelativo { get; set; }

        [JsonPropertyName("fecha_emision")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23", Nullable = false)]
        public string Fecha { get; set; }

        [JsonPropertyName("hora")]
        [SwaggerSchema("Devuelve un texto de hora en formato de 24 horas(hh:mm:ss); por ejemplo: 14:35:00", Nullable = false)]
        public string Hora { get; set; }

        [JsonPropertyName("observacion")]
        [SwaggerSchema("Devuelve un texto, donde se detalla una observación o comentario sobre la guía de remisión", Nullable = true)]
        public string? Observacion { get; set; }

        [JsonPropertyName("area")]
        [SwaggerSchema("Devuelve en texto el área.", Nullable = false)]
        public string AreaNombre { get; set; } = null!;

        [JsonPropertyName("registro_mtc")]
        [SwaggerSchema("Devuelve en texto el número de registro MTC, este campo tiene valor si tipo_guia es 'TRANSPORTISTA'")]
        public string? RegistroMTC { get; set; }

        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoDTO>? DocRelacionados { get; set; }

        [JsonPropertyName("remitente")]
        public EstablecimientoDTO Remitente { get; set; }

        [JsonPropertyName("destinatario")]
        public EstablecimientoDTO Destinatario { get; set; }

        [JsonPropertyName("destinatario_correo")]
        public List<EmailDestinatarioGuiaDTO> DestinatarioCorreo { get; set; } = new List<EmailDestinatarioGuiaDTO>();

        [JsonPropertyName("proveedor")]
        public ProveedorDTO? Proveedor { get; set; }

        [JsonPropertyName("datos_envio")]
        public GuiaRemisionDatosEnvioDTO DatosEnvio { get; set; }

        [JsonPropertyName("origen")]
        public OrigenDTO Origen { get; set; }

        [JsonPropertyName("destino")]
        public List<DestinoDTO> Destino { get; set; }

        [JsonPropertyName("productos")]
        public List<GuiaRemisionDetalleDTO> Productos { get; set; }

        [JsonPropertyName("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [JsonPropertyName("usuario_registro")]
        [SwaggerSchema("Devuelve un texto, el usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string UsuarioRegistro { get; set; }

        [JsonPropertyName("usuario_registro_nombre")]
        [SwaggerSchema("Devuelve un texto, el nombre del usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string UsuarioRegistroNombre { get; set; }

        [JsonPropertyName("fecha_modifico")]
        public DateTime? FechaModifico { get; set; }

        [JsonPropertyName("usuario_modifico")]
        [SwaggerSchema("Devuelve un texto, el usuario que ha modificado el registro.", Nullable = false)]
        public string? UsuarioModifico { get; set; }

        [JsonPropertyName("usuario_modifico_nombre")]
        [SwaggerSchema("Devuelve un texto, el nombre del usuario que ha modificado el registro.", Nullable = false)]
        public string? UsuarioModificoNombre { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, el estado de la guía de remisión.", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado de la guía de remisión en la bbdd", Nullable = false)]
        public int EstadoId { get; set; }

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
            if (tipo == "PASAPORTE" || tipo == "PP")
                return EspacioNombreGuiaRemision._DocumentType07;
            throw new System.Exception("Tipo de documento de identidad no especificado, coloque entre: DNI, CE, RUC, PASAPORTE dependiendo del caso");
        }
    }
}
