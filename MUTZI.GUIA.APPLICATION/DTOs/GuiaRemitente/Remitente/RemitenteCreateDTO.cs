using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente
{
    [SwaggerSchema("Recibe un objeto remitente, si es nuevo remitente la api crea un nuevo remitente en la bbdd", Nullable = false)]
    public class RemitenteCreateDTO
    {
        [JsonPropertyName("ruc")]
        [SwaggerSchema("Recibe un texto, es el ruc del remitente", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del remitente es obligatoria")]
        [StringLength(11, ErrorMessage = "El número de documento del remitente no puede exceder los 11 caracteres")]
        [MinLength(11)]
        public string Ruc { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la descripción o nombre del remitente", Nullable = false)]
        [MaxLength(200)]
        [Required]
        public string Descripcion { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del remitente, ejm: 150131", Nullable = false)]
        [Required(ErrorMessage = "EL ubigeo es obligatorio")]
        //EL ubigeo debe tener 6 caracteres
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [MinLength(6)]
        //El ubigeo no puede ser 000000
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string UbigeoID { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la direccion fiscal del remitente", Nullable = false)]
        [MaxLength(250)]
        [Required]
        public string Direccion { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del remitente", Nullable = true)]
        //[MaxLength(50)]
        //[EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string? Email { get; set; }
        
        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el codigo de PAIS referente, ejm: PE", Nullable = false)]
        [MaxLength(3)]
        [Required]
        public string Pais { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la serie que posteriormente se usa en las guías de remisión, ejm: 001, 002", Nullable = true)]
        [StringLength(3, ErrorMessage = "La serie requiere 3 caracteres")]
        public string? Serie { get; set; }

        [JsonPropertyName("codigoSunat")]
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
    }
}
