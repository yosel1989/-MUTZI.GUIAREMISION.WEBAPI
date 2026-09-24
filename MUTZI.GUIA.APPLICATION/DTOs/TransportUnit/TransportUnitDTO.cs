using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte
{
    [SwaggerSchema("Devuelve un objeto de unidad de transporte")]
    public class TransportUnitDTO
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

        [JsonPropertyName("entidad_reguladora_vehicular_id")]
        [SwaggerSchema("Devuelve un número, el id de la entidad reguladora vehicular", Nullable = true)]
        public int? EntidadReguladoraVehicularId { get; set; }

        [JsonPropertyName("entidad_reguladora_vehicular")]
        [SwaggerSchema("Devuelve un texto, el nombre de la entidad reguladora vehicular", Nullable = true)]
        public string? EntidadReguladoraVehicularNombre { get; set; }

        [JsonPropertyName("emisor_vehicular")]
        [SwaggerSchema("Devuelve un texto, el emisor vehicular del transporte", Nullable = true)]
        public string? EmisorVehicular { get; set; }

        [JsonPropertyName("nro_autorizacion")]
        [SwaggerSchema("Devuelve un texto, el número de autorización del transporte", Nullable = true)]
        public string? NroAutorizacion { get; set; }

        [JsonPropertyName("created_at")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00", Nullable = false)]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        [SwaggerSchema("Devuelve un texto, el usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        [SwaggerSchema("Devuelve un texto, el nombre del usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string CreatedAtUserName { get; set; }

        [JsonPropertyName("updated_at")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00, es la fecha de la última edición realizada en la guía", Nullable = true)]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("updated_at_user")]
        [SwaggerSchema("Devuelve en texto, el usuario que ha realizado un cambio en el registro", Nullable = true)]
        public string? UpdatedAtUser { get; set; }

        [JsonPropertyName("updated_at_user_name")]
        [SwaggerSchema("Devuelve en texto, el nombre del usuario que ha realizado un cambio en el registro", Nullable = true)]
        public string? UpdatedAtUserName { get; set; }

        [JsonPropertyName("active")]
        [SwaggerSchema("Devuelve un bool, el estado del registro activo = true, inactivo = false", Nullable = false)]
        public bool Active { get; set; }

        [JsonPropertyName("tipo")]
        [SwaggerSchema("Devuelve un texto, el tipo del transporte solo puede ser: interno, externo", Nullable = false)]
        public string Tipo { get; set; }
    }
}
