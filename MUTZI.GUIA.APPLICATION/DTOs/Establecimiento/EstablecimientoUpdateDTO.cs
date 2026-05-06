using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    [SwaggerSchema("Recibe un objeto establecimiento para actualizar", Nullable = false)]
    public class EstablecimientoUpdateDTO
    {
        [JsonPropertyName("establecimiento_id")]
        [SwaggerSchema("Recibe un número, es el id del establecimiento", Nullable = false)]
        [Required(ErrorMessage = "El id del establecimiento es requerido")]
        public int EstablecimientoId { get; set; }

        [JsonPropertyName("ruc")]
        [SwaggerSchema("Recibe un texto, es el ruc del establecimiento", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del establecimiento es obligatoria")]
        [StringLength(11, ErrorMessage = "El número de documento del establecimiento no puede exceder los 11 caracteres")]
        [MinLength(11)]
        public string Ruc { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la descripción o nombre del establecimiento", Nullable = false)]
        [MaxLength(200)]
        [Required]
        public string Descripcion { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del establecimiento, ejm: 150131", Nullable = false)]
        [Required(ErrorMessage = "EL ubigeo es obligatorio")]
        //EL ubigeo debe tener 6 caracteres
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [MinLength(6)]
        //El ubigeo no puede ser 000000
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string UbigeoID { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la direccion fiscal del establecimiento", Nullable = false)]
        [MaxLength(250)]
        [Required]
        public string Direccion { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del establecimiento", Nullable = true)]
        //[MaxLength(50)]
        //[Required]
        //[EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string? Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el codigo de PAIS referente, ejm: PE", Nullable = false)]
        [MaxLength(3)]
        [Required]
        public string Pais { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la serie que posteriormente se usa en las guías de remisión, ejm: 001, 002", Nullable = false)]
        [MaxLength(4)]
        public string Serie { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el Código de establecimiento o de la Sucursal", Nullable = true)]
        [StringLength(4, ErrorMessage = "El código de establecimiento requiere 4 caracteres")]
        public string? CodigoSunat { get; set; }
    }
}
