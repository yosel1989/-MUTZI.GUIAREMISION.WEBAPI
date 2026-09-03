using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EntityBranchCreateDTO
    {
        [JsonPropertyName("ruc")]
        [SwaggerSchema("Recibe un texto, es el ruc del remitente", Nullable = true)]
        //[Required(ErrorMessage = "El Nro documento del remitente es obligatoria")]
        //[StringLength(11, ErrorMessage = "El número de documento del remitente no puede exceder los 11 caracteres")]
        //[MinLength(11)]
        public string? Ruc { get; set; }

        [JsonPropertyName("entity_id")]
        [SwaggerSchema("Recibe un número, aquí se coloca el identificador de la entidad", Nullable = false)]
        [Required]
        public int? EntityId { get; set; }

        [JsonPropertyName("description")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la descripción o nombre del remitente", Nullable = false)]
        [MaxLength(200)]
        [Required]
        public string Description { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del remitente, ejm: 150131", Nullable = false)]
        [Required(ErrorMessage = "EL ubigeo es obligatorio")]
        //EL ubigeo debe tener 6 caracteres
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [MinLength(6)]
        //El ubigeo no puede ser 000000
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string UbigeoID { get; set; }

        [JsonPropertyName("address")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la direccion fiscal del remitente", Nullable = false)]
        [MaxLength(250)]
        [Required]
        public string Address { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del remitente", Nullable = true)]
        [MinLength(5)]
        [MaxLength(50)]
        //[EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string? Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el CodeSunat de PAIS referente, ejm: PE", Nullable = false)]
        [MaxLength(3)]
        [Required]
        public string Pais { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la serie que posteriormente se usa en las guías de remisión, ejm: 001, 002", Nullable = false)]
        [StringLength(3, ErrorMessage = "La serie requiere 3 caracteres")]
        public string Serie { get; set; }

        [JsonPropertyName("code_sunat")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el Código de establecimiento o de la Sucursal", Nullable = true)]
        [StringLength(4, ErrorMessage = "El código de establecimiento requiere 4 caracteres")]
        public string CodeSunat { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = true)]
        public int? EmpleadoIDCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha realizado la transacción en el sistema", Nullable = true)]
        [MaxLength(100)]
        public string? EmpleadoNombreCreacion { get; set; }

        [JsonPropertyName("tipo")]
        [SwaggerSchema("Recibe un texto, se coloca el tipo de establecimiento segpun sunat : 'domicilio_fiscal', 'casa_matriz', 'sucursal', 'agencia', 'local_comercial', 'sede_productiva', 'almacen', 'oficina_administrativa'", Nullable = true)]
        [MaxLength(100)]
        public string Tipo { get; set; }

        [JsonPropertyName("area")]
        [SwaggerSchema("Recibe un texto, el area al que pertenece la serie , ejm: VENTAS, LOGISTICA", Nullable = true)]
        [MaxLength(45)]
        public string? Area { get; set; }

        [JsonPropertyName("is_main")]
        [SwaggerSchema("Recibe un boolean, aquí recibe un indicador si es anexo principal", Nullable = false)]
        public bool IsMain { get; set; }
    }
}
