using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.EmailRemiteTransportistaGuia;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Transportista
{
    public class TransportistaDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, es el número ID del transportista.", Nullable = false)]
        public int TransportistaId { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve en texto el tipo de documento del transportista, solo pueden tener los siguientes valores: DNI, CE, RUC y PASAPORTE", Nullable = false)]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Devuelve en texto el número documento del transportista.", Nullable = false)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("razon_social")]
        public string RazonSocial { get; set; }

        [JsonPropertyName("ubigeo_id")]
        public string UbigeoId { get; set; }

        [JsonPropertyName("departamento")]
        public string Departamento { get; set; }

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; }

        [JsonPropertyName("distrito")]
        public string Distrito { get; set; }

        [JsonPropertyName("direccion")]
        public string Direccion { get; set; }

        [JsonPropertyName("pais")]
        public string Pais { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Devuelve en texto el Código de establecimiento o de la Sucursal", Nullable = true)]
        public string? CodigoSunat { get; set; }

        [JsonPropertyName("email_remite_transportista")]
        public List<EmailRemiteTransportistaGuiaDTO>? EmailRemiteTransportista { get; set; }

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
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00, es la fecha de la última edición realizada", Nullable = true)]
        public DateTime? FechaModifico { get; set; }

        [JsonPropertyName("usuario_modifico")]
        [SwaggerSchema("Devuelve en texto, el usuario que ha realizado la última edición", Nullable = true)]
        public string? UsuarioModifico { get; set; }

        [JsonPropertyName("usuario_modifico_nombre")]
        [SwaggerSchema("Devuelve en texto, el nombre del usuario que ha realizado la última edición", Nullable = true)]
        public string? UsuarioModificoNombre { get; set; }

        [JsonPropertyName("estado")]
        [SwaggerSchema("Devuelve un texto, el estado del transportista solo puede ser: ACTIVO o INACTIVO", Nullable = false)]
        public string Estado { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Devuelve un entero, el ID del estado del transportista en la bbdd", Nullable = false)]
        public int EstadoId { get; set; }

        [JsonPropertyName("registro_mtc")]
        [SwaggerSchema("Devuelve un texto, el CodigoSunat de registro MTC del transportista", Nullable = true)]
        public string? RegistroMTC { get; set; }

        [JsonPropertyName("email_contacto")]
        [SwaggerSchema("Devuelve un texto, el email de contacto del transportista", Nullable = true)]
        public string? EmailContacto { get; set; }
    }
}
