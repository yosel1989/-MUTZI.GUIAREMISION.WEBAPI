using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Efact
{
    public class TicketResponseDTO
    {
        [JsonProperty("tickets")]
        public List<string> Tickets { get; set; }
    }
    [SwaggerSchema("Respuesta del facturador. Ejemplos de uso: 'cuando la guía fue enviada correctamente', 'cuando se consulta el pdf u otro documento al facturador.", Nullable = false)]
    public class EfactResponseDTO
    {
        [JsonPropertyName("codigo")]
        [SwaggerSchema("Devuelve un texto, es el código de respuesta cuando hubo una correcta interacción con el facturador. Ejm => código: 0; indica que se envió la guía correctamente. Ej.2 => código: -9998; indica que se ha generado la guía, pero que el documento pdf u otro aún está en proceso.", Nullable = false)]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve en texto la descripción cuando hubo una correcta interacción con el facturador, normalmente retorna el ticket creado en la plataforma del facturador. Ejm cuando se crea una guía => descripcion: 33baf32d-39f8-4151-bd48-cfadcfb9549c ó cuando se consulta un pdf => Ejm: descripcion: En proceso.", Nullable = false)]
        public string Description { get; set; } = string.Empty;
    }
    [SwaggerSchema("Respuesta exitosa del endpoint enviar_guia_remision")]
    public class SendGuiaResponseDTO
    {
        [JsonPropertyName("success")]
        [SwaggerSchema("Devuelve un booleando 'true' cuando se envía la guía de remisión", Nullable = false)]
        public bool Success { get; set; }
        [JsonPropertyName("respuesta_facturador")]
        [SwaggerSchema("Devuelve el objeto de salida cuando se envía la guía de remisión", Nullable = false)]
        public EfactResponseDTO EfactResponse { get; set; }
    }

    [SwaggerSchema("Respuesta de error estándar para bad request (400). Este tipo de error pudo haber ocurrido en las validaciones antes de crear la info en la bbdd o después de crear la info en la bbdd, incluso al buscar o listar la información desde la bbdd.", Nullable = false)]
    public class BadRequestResponseDTO
    {
        [JsonPropertyName("error")]
        [SwaggerSchema("Devuelve un texto corto del error ocurrido.", Nullable = false)]
        public string Error { get; set; } = string.Empty;
        [JsonPropertyName("detalle")]
        [SwaggerSchema("Devuelve un texto largo detallando el error.", Nullable = false)]
        public string Detalle { get; set; } = string.Empty;
    }

    [SwaggerSchema("Respuesta estándar de error interno (500). El error 500 puede salir cuando hay errores al procesar la información en el servidor o con el facturador de guías de remisión, puede ser por ejm: errores de token, errores de creación de guías en la plataforma del facturador de guías, etc. Estos errores normalmente son capturados después de las validaciones y ocurren en pleno envío de datos o respuesta desde el servidor, donde la transacción no ha sido completada y retorna el mensaje de error.", Nullable = false)]
    public class InternalErrorResponseDTO
    {
        [JsonPropertyName("error")]
        [SwaggerSchema("Devuelve un texto corto del error ocurrido.", Nullable = false)]
        public string Error { get; set; } = string.Empty;
        [JsonPropertyName("detalle")]
        [SwaggerSchema("Devuelve un texto largo de 500 caracteres detallando el error.", Nullable = false)]
        public string Detalle { get; set; } = string.Empty;
    }
    [SwaggerSchema("Respuesta para un registro exitoso en la bbdd")]
    public class CreatedDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del nuevo registro en la bbdd.", Nullable = false)]
        public int Id { get; set; }

        [JsonPropertyName("detalle")]
        [SwaggerSchema("Devuelve en texto el detalle de la transacción en la bbdd.", Nullable = false)]
        public string Detalle { get; set; } = "Registro creado exitosamente.";
    }
    [SwaggerSchema("Respuesta para un registro eliminado de la bbdd")]
    public class DeletedDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID afectado por la transacción en la bbdd.", Nullable = false)]
        public int Id { get; set; }

        [JsonPropertyName("eliminado")]
        [SwaggerSchema("Devuelve un booleano true, en caso el registro haya sido eliminado.", Nullable = false)]
        public bool Eliminado { get; set; } = true;

        [JsonPropertyName("detalle")]
        [SwaggerSchema("Devuelve en texto el detalle de la transacción en la bbdd.", Nullable = false)]
        public string Detalle { get; set; } = "Registro eliminado exitosamente.";
    }
    [SwaggerSchema("Respuesta para una actualización de estado de un registro en la bbdd")]
    public class UpdatedStatus
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID afectado por la transacción en la bbdd.", Nullable = false)]
        public int Id { get; set; }

        [JsonPropertyName("new_status")]
        [SwaggerSchema("Devuelve en entero el nuevo estado del registro.", Nullable = false)]
        public int NewStatus { get; set; }

        [JsonPropertyName("detalle")]
        [SwaggerSchema("Devuelve en texto el detalle de la transacción en la bbdd.", Nullable = false)]
        public string Detalle { get; set; } = "Estado de registro actualizado correctamente.";
    }
}
