using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte
{
    [SwaggerSchema("Devuelve un objeto de unidad de transporte")]
    public class UnidadTransporteDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del transporte en la bbdd", Nullable = false)]
        public int TransporteID { get; set; }

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

        [JsonPropertyName("numero_registro_mtc")]
        [SwaggerSchema("Devuelve un texto, el número de registro MTC del transporte", Nullable = false)]
        public string NumeroRegistroMTC { get; set; }

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Devuelve un texto, la tarjeta de circulación del transporte", Nullable = false)]
        public string Tarjeta { get; set; }

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
        [SwaggerSchema("Devuelve un texto, el estado del transporte solo puede ser: ACTIVO o INACTIVO", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado del transporte en la bbdd", Nullable = false)]
        public int IdEstado { get; set; }
    }
}
