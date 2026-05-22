using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente
{
    public class GuiaRemisionListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, es el identificador público de la guía.", Nullable = false)]
        public int GuiaRemisionId { get; set; }

        [JsonPropertyName("uuid")]
        [SwaggerSchema("Devuelve un identificador privado de la guía.", Nullable = false)]
        public Guid GuiaRemisionUuid { get; set; }

        [JsonPropertyName("entidad_remitente")]
        [SwaggerSchema("Devuelve un texto, es el nombre o razón social a la que pertenece la guía de remisión", Nullable = false)]
        public string EntidadRemitente { get; set; }

        [JsonPropertyName("numero_documento_remitente")]
        [SwaggerSchema("Devuelve un texto, es el número de documento de la empresa a la que pertenece la guía de remisión", Nullable = false)]
        public string NumeroDocumentoRemitente { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Devuelve un texto, es la seríe a la que pertenece la guía", Nullable = false)]
        public string Serie { get; set; }

        [JsonPropertyName("numero")]
        [SwaggerSchema("Devuelve un texto, es el número a la que pertenece la guía", Nullable = false)]
        public string Correlativo { get; set; }

        [JsonPropertyName("establecimiento_remitente")]
        [SwaggerSchema("Devuelve un texto, es la razón social del remitente de la guía de remisión", Nullable = false)]
        public string EstablecimientoRemitente { get; set; }

        [JsonPropertyName("tipo_guia")]
        [SwaggerSchema("Devuelve en texto el tipo de guía de remisión, solo puede ser: REMITENTE o TRANSPORTISTA", Nullable = false)]
        public string TipoGuia { get; set; }

        [JsonPropertyName("numero_guia")]
        [SwaggerSchema("Devuelve en texto el número de guía de remisión", Nullable = false)]
        public string NumeroGuia { get; set; }

        //[JsonPropertyName("tipo_traslado")]
        //[SwaggerSchema("Devuelve un texto y solo pueden ser: VENTA, TRASLADO ó COMPRA", Nullable = false)]
        //public string TipoTraslado { get; set; }

        [JsonPropertyName("tipo_transporte")]
        [SwaggerSchema("Devuelve un texto y solo pueden ser: PUBLICO ó PRIVADO", Nullable = false)]
        public string TipoTransporte { get; set; }

        [JsonPropertyName("fecha_emision")]
        [SwaggerSchema("Devuelve en texto la fecha de emisión de la guía en formato: yyyy-MM-dd; por ejemplo: 2025-09-23", Nullable = false)]
        public string FechaEmision { get; set; }

        [JsonPropertyName("hora_emision")]
        [SwaggerSchema("Devuelve en texto la hora de emisión de la guía en formato de 24 horas(hh:mm:ss); por ejemplo: 14:35:00", Nullable = false)]
        public string HoraEmision { get; set; }

        [JsonPropertyName("respuesta_ticket")]
        [SwaggerSchema("Devuelve en texto el número de ticket de respuesta del facturador", Nullable = true)]
        public string? RespuestaTicket { get; set; }

        [JsonPropertyName("establecimiento_destinatario")]
        [SwaggerSchema("Devuelve un texto, es la razón social del destinatario de la guía de remisión", Nullable = false)]
        public string EstablecimientoDestinatario { get; set; }

        [JsonPropertyName("entidad_destinatario")]
        [SwaggerSchema("Devuelve un texto, es nombre o razón social del destinatario.", Nullable = false)]
        public string EntidadDestinatario { get; set; }

        [JsonPropertyName("numero_documento_destinatario")]
        [SwaggerSchema("Devuelve un texto, es el número de documento del destinatario de la guía de remisión", Nullable = false)]
        public string NumeroDocumentoDestinatario { get; set; }

        [JsonPropertyName("direccion_destino")]
        [SwaggerSchema("Devuelve un texto, es la dirección destino de la guía de remisión", Nullable = false)]
        public string DireccionDestino{ get; set; }

        [JsonPropertyName("distrito_origen")]
        [SwaggerSchema("Devuelve en texto el distrito de origen o del punto de partida", Nullable = false)]
        public string DistritoOrigen { get; set; }

        [JsonPropertyName("direccion_origen")]
        [SwaggerSchema("Devuelve en texto la dirección de origen", Nullable = false)]
        public string DireccionOrigen { get; set; }

        [JsonPropertyName("distrito_destino")]
        [SwaggerSchema("Devuelve en texto el distrito de destino o del punto de llegada", Nullable = false)]
        public string DistritoDestino { get; set; }

        [JsonPropertyName("fecha_registro")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00", Nullable = false)]
        public DateTime FechaRegistro { get; set; }

        [JsonPropertyName("usuario_registro")]
        [SwaggerSchema("Devuelve un texto, el usuario del empleado que ha registrado la transacción en el sistema", Nullable = false)]
        public string UsuarioRegistro { get; set; }

        [JsonPropertyName("usuario_registro_nombre")]
        [SwaggerSchema("Devuelve un texto, el nombre del empleado que ha registrado la transacción en el sistema", Nullable = false)]
        public string UsuarioRegistroNombre { get; set; }

        [JsonPropertyName("fecha_modifico")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00, es la fecha de la última edición realizada en la guía", Nullable = true)]
        public DateTime? FechaModifico { get; set; }

        [JsonPropertyName("usuario_modifico")]
        [SwaggerSchema("Devuelve en texto el usuario del empleado que ha realizado la última edición en la guía", Nullable = true)]
        public string? UsuarioModifico { get; set; }

        [JsonPropertyName("usuario_modifico_nombre")]
        [SwaggerSchema("Devuelve en texto el nombre del empleado que ha realizado la última edición en la guía", Nullable = true)]
        public string? UsuarioModificoNombre { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, el estado de la guía en la bbdd.", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado de la guía en la bbdd", Nullable = false)]
        public int EstadoId { get; set; }

        [JsonPropertyName("estado_sunat")]
        [SwaggerSchema("Devuelve un texto, el estado de la guía en sunat.", Nullable = false)]
        public string EstadoSunat { get; set; } = null!;

        [JsonPropertyName("id_estado_sunat")]
        [SwaggerSchema("Devuelve un entero, el ID del estado de la guía en sunat", Nullable = false)]
        public sbyte EstadoSunatId { get; set; }

        [JsonPropertyName("area")]
        [SwaggerSchema("Devuelve un texto, el área que genero la guía", Nullable = false)]
        public string AreaNombre { get; set; }

        [JsonPropertyName("area_id")]
        [SwaggerSchema("Devuelve un número, el id del área que genero la guía.", Nullable = false)]
        public int AreaId { get; set; }

        [JsonPropertyName("motivo_traslado_id")]
        [SwaggerSchema("Devuelve un número, el id del motivo de traslado de la guía.", Nullable = false)]
        public int MotivoTrasladoId { get; set; }

        [JsonPropertyName("motivo_traslado")]
        [SwaggerSchema("Devuelve un texto, el motivo de traslado de la guía.", Nullable = false)]
        public string MotivoTrasladoNombre { get; set; }

    }
}
