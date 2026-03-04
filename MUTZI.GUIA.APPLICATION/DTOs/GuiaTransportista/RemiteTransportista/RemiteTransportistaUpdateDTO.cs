using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista
{
    [SwaggerSchema("Recibe un objeto remite_transportista para actualizar", Nullable = false)]
    public class RemiteTransportistaUpdateDTO
    {
        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el tipo de documento del REMITE_TRANSPORTISTA. Solo recibe los siguientes valores: DNI, CE, RUC y PASAPORTE", Nullable = false)]
        [MaxLength(20)]
        [Required]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el número de documento del REMITE_TRANSPORTISTA.", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del REMITE_TRANSPORTISTA es obligatorio")]
        [MaxLength(15)]
        [MinLength(8)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("razon_social")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la razón social del remitente_transportista", Nullable = false)]
        [MaxLength(200)]
        [Required]
        public string RazonSocial { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del remitente_transportista, ejm: 150131", Nullable = false)]
        [Required(ErrorMessage = "EL ubigeo es obligatorio")]
        //EL ubigeo debe tener 6 caracteres
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [MinLength(6)]
        //El ubigeo no puede ser 000000
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string UbigeoID { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la direccion fiscal del remitente_transportista", Nullable = false)]
        [MaxLength(250)]
        [Required]
        public string Direccion { get; set; }

        //[JsonPropertyName("email")]
        //[SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del remite_transportista", Nullable = false)]
        //[MaxLength(50)]
        //[Required]
        //[EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        //public string Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el codigo de PAIS referente, ejm: PE", Nullable = false)]
        [MaxLength(3)]
        [Required]
        public string Pais { get; set; }

        [JsonPropertyName("empleado_id_edicion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = false)]
        [Required]
        public int EmpleadoIDEdicion { get; set; }

        [JsonPropertyName("empleado_nombre_edicion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha editado la transacción en el sistema", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string EmpleadoNombreEdicion { get; set; }
    }
}
