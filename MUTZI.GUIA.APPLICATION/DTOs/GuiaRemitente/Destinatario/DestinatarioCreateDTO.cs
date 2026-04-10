using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.EmailDestinatarioGuia;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario
{
    [SwaggerSchema("Recibe un objeto destinatario, si es nuevo destinatario la api crea un nuevo destinatario en la bbdd", Nullable = false)]
    public class DestinatarioCreateDTO
    {
        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el tipo de documento del destinatario. Solo recibe los siguientes valores: DNI, CE, RUC y PASAPORTE", Nullable = false)]
        [MaxLength(21)]
        [Required]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el número de documento del destinatario.", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del destinatario es obligatorio")]
        [MaxLength(21)]
        [MinLength(8)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("razon_social")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la razón social del destinatario.", Nullable = false)]
        [MaxLength(200)]
        [Required]
        public string RazonSocial { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del destinatario, ejm: 150131", Nullable = true)]
        //EL ubigeo debe tener 6 caracteres
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        //El ubigeo no puede ser 000000
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string? UbigeoId { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la dirección fiscal del destinatario", Nullable = true)]
        [MaxLength(250)]
        public string? Direccion { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el codigo de PAIS referente, ejm: PE", Nullable = true)]
        [MaxLength(3)]
        public string? Pais { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el Código de establecimiento o de la Sucursal", Nullable = true)]
        [StringLength(4, ErrorMessage = "El código de establecimiento requiere 4 caracteres")]
        public string? CodigoSunat { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = true)]
        public int? EmpleadoIDCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha realizado la transacción en el sistema", Nullable = true)]
        [MaxLength(100)]
        public string? EmpleadoNombreCreacion { get; set; }

        //[JsonPropertyName("email_destinatario")]
        //[SwaggerSchema("Recibe un texto, aquí se coloca hasta máximo 3 correos del destinatario", Nullable = true)]
        //public List<EmailDestinatarioGuiaCreateDTO>? EmailDestinatarioGuia { get; set; }
    }
}
