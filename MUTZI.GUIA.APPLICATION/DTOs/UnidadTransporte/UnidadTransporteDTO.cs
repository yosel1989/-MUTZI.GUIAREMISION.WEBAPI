using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte
{
    [SwaggerSchema("Devuelve un objeto de unidad de transporte")]
    public class UnidadTransporteDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del transporte en la bbdd", Nullable = false)]
        public int UnidadTransporteId { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve un texto, la descripción del transporte", Nullable = false)]
        public string Descripcion { get; set; }

        [JsonPropertyName("marca")]
        [SwaggerSchema("Devuelve un texto, la marca del transporte", Nullable = false)]
        public string Marca { get; set; }

        [JsonPropertyName("modelo")]
        [SwaggerSchema("Devuelve un texto, el modelo del transporte", Nullable = false)]
        public string Modelo { get; set; }

        [JsonPropertyName("placa")]
        [SwaggerSchema("Devuelve un texto, la placa del transporte", Nullable = false)]
        public string Placa { get; set; }

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Devuelve un texto, la tarjeta de circulación del transporte", Nullable = false)]
        public string Tarjeta { get; set; }

        [JsonPropertyName("cod_emisor_vehicular")]
        [SwaggerSchema("Devuelve un texto, el código del emisor vehicular del transporte", Nullable = true)]
        public string? CodEmisorVehicular { get; set; }

        [JsonPropertyName("emisor_vehicular")]
        [SwaggerSchema("Devuelve un texto, el emisor vehicular del transporte", Nullable = true)]
        public string? EmisorVehicular { get; set; }

        [JsonPropertyName("nro_autorizacion")]
        [SwaggerSchema("Devuelve un texto, el número de autorización del transporte", Nullable = true)]
        public string? NroAutorizacion { get; set; }

        [JsonPropertyName("fecha_registro")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00", Nullable = false)]
        public DateTime FechaRegistro { get; set; }

        [JsonPropertyName("usuario_registro")]
        [SwaggerSchema("Devuelve un texto, el usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string UsuarioRegistro { get; set; }

        [JsonPropertyName("usuario_registro_nombre")]
        [SwaggerSchema("Devuelve un texto, el nombre del usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string UsuarioRegistroNombre { get; set; }

        [JsonPropertyName("fecha_modifico")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00, es la fecha de la última edición realizada en la guía", Nullable = true)]
        public DateTime? FechaModifico { get; set; }

        [JsonPropertyName("usuario_modifico")]
        [SwaggerSchema("Devuelve en texto, el usuario que ha realizado un cambio en el registro", Nullable = true)]
        public string? UsuarioModifico { get; set; }

        [JsonPropertyName("usuario_modifico_nombre")]
        [SwaggerSchema("Devuelve en texto, el nombre del usuario que ha realizado un cambio en el registro", Nullable = true)]
        public string? UsuarioModificoNombre { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, el estado del transporte solo puede ser: activo o inactivo", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado del transporte en la bbdd", Nullable = false)]
        public int IdEstado { get; set; }

        [JsonPropertyName("tipo")]
        [SwaggerSchema("Devuelve un texto, el tipo del transporte solo puede ser: interno, externo", Nullable = false)]
        public string Tipo { get; set; }
    }
}
