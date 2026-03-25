using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente
{
    public class GuiaListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, es el número ID de la guía.", Nullable = false)]
        public int GuiaID { get; set; }

        [JsonPropertyName("empresa")]
        [SwaggerSchema("Devuelve un texto, es el nombre de la empresa a la que pertenece la guía de remisión", Nullable = false)]
        public string Empresa { get; set; }

        [JsonPropertyName("ruc")]
        [SwaggerSchema("Devuelve un texto, es el número de RUC de la empresa a la que pertenece la guía de remisión", Nullable = false)]
        public string Ruc { get; set; }

        [JsonPropertyName("razon_remitente")]
        [SwaggerSchema("Devuelve un texto, es la razón social del remitente de la guía de remisión", Nullable = false)]
        public string RazonRemitente { get; set; }

        [JsonPropertyName("tipo_guia")]
        [SwaggerSchema("Devuelve en texto el tipo de guía de remisión, solo puede ser: REMITENTE o TRANSPORTISTA", Nullable = false)]
        public string TipoGuia { get; set; }

        [JsonPropertyName("numero_guia")]
        [SwaggerSchema("Devuelve en texto el número de guía de remisión", Nullable = false)]
        public string NumeroGuia { get; set; }

        [JsonPropertyName("tipo_traslado")]
        [SwaggerSchema("Devuelve un texto y solo pueden ser: VENTA, TRASLADO ó COMPRA", Nullable = false)]
        public string TipoTraslado { get; set; }

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

        [JsonPropertyName("razon_destinatario")]
        [SwaggerSchema("Devuelve un texto, es la razón social del destinatario de la guía de remisión", Nullable = false)]
        public string RazonDestinatario { get; set; }

        [JsonPropertyName("nro_documento_destinatario")]
        [SwaggerSchema("Devuelve un texto, es el número de documento del destinatario de la guía de remisión", Nullable = false)]
        public string NroDocumentoDestinatario { get; set; }

        [JsonPropertyName("distrito_origen")]
        [SwaggerSchema("Devuelve en texto el distrito de origen o del punto de partida", Nullable = false)]
        public string DistritoOrigen { get; set; }

        [JsonPropertyName("distrito_destino")]
        [SwaggerSchema("Devuelve en texto el distrito de destino o del punto de llegada", Nullable = false)]
        public string DistritoDestino { get; set; }

        [JsonPropertyName("fecha_creacion")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00", Nullable = false)]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Devuelve un texto, el nombre del empleado que ha registrado la transacción en el sistema", Nullable = false)]
        public string EmpleadoNombreCreacion { get; set; }

        [JsonPropertyName("fecha_ultima_edicion")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00, es la fecha de la última edición realizada en la guía", Nullable = true)]
        public DateTime? FechaEdicion { get; set; }

        [JsonPropertyName("empleado_nombre_edicion")]
        [SwaggerSchema("Devuelve en texto el nombre del empleado que ha realizado la última edición en la guía", Nullable = true)]
        public string? EmpleadoNombreEdicion { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, el estado de la guía.", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado de la guía en la bbdd", Nullable = false)]
        public int IdEstado { get; set; }

    }
}
