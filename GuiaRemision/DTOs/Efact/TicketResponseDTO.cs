using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.Efact
{
    public class TicketResponseDTO
    {
        [JsonProperty("tickets")]
        public List<string> Tickets { get; set; }
    }
    [SwaggerSchema("Respuesta del facturador (cuando la guía fue enviada correctamente).", Nullable = false)]
    public class EfactResponseDto
    {
        [JsonPropertyName("codigo")]
        [SwaggerSchema("Devuelve en texto un código cuando la guía fue enviada correctamente. Ejm => código: 0; indica que se envió la guía correctamente", Nullable = false)]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve en texto la descripción cuando la guía es enviada correctamente, normalmente retorna el ticket creado en la plataforma de facturación. Ejm => descripcion: 33baf32d-39f8-4151-bd48-cfadcfb9549c", Nullable = false)]
        public string Description { get; set; } = string.Empty;
    }
    [SwaggerSchema("Respuesta exitosa del endpoint enviar_guia_remision")]
    public class EnviarGuiaResponseDto
    {
        [JsonPropertyName("success")]
        [SwaggerSchema("Devuelve un booleando 'true' cuando se envía la guía de remisión", Nullable = false)]
        public bool Success { get; set; }
        [JsonPropertyName("respuesta_facturador")]
        [SwaggerSchema("Devuelve el objeto de salida cuando se envía la guía de remisión", Nullable = false)]
        public EfactResponseDto EfactResponse { get; set; }
    }

    [SwaggerSchema("Respuesta de error estándar para bad request (400). Todos los errores que salen aquí son antes de interactuar con el facturador de guías de remisión. Es decir, el error pudo haber ocurrido en las validaciones antes de crear la info en la bbdd o después de crear la info en la bbdd, empero siempre serán antes de enviar aL FACTURADOR, ya sea, EFACT u otro facturador.", Nullable = false)]
    public class BadRequestResponseDto
    {
        [JsonPropertyName("error")]
        [SwaggerSchema("Devuelve un texto corto del error ocurrido.", Nullable = false)]
        public string Error { get; set; } = string.Empty;
        [JsonPropertyName("detalle")]
        [SwaggerSchema("Devuelve un texto largo detallando el error.", Nullable = false)]
        public string Detalle { get; set; } = string.Empty;
    }

    [SwaggerSchema("Respuesta de error interno (500). El error 500 puede salir cuando hay errores al procesar la información en el servidor o con el facturador de guías de remisión, puede ser por ejm: errores de token, errores de creación de guías en la plataforma del facturador de guías, etc.", Nullable = false)]
    public class InternalErrorResponseDto
    {
        [JsonPropertyName("error")]
        [SwaggerSchema("Devuelve un texto corto del error ocurrido.", Nullable = false)]
        public string Error { get; set; } = string.Empty;
        [JsonPropertyName("detalle")]
        [SwaggerSchema("Devuelve un texto largo de 500 caracteres detallando el error.", Nullable = false)]
        public string Detalle { get; set; } = string.Empty;
    }

}
