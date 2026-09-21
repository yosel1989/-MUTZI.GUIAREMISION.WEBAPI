using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    [SwaggerSchema("Recibe un objeto establecimiento para actualizar", Nullable = false)]
    public class EntityBranchUpdateDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Recibe un número, es el id del establecimiento", Nullable = false)]
        [Required(ErrorMessage = "El id del establecimiento es requerido")]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("entity_id")]
        public int EntityId { get; set; }

        [JsonPropertyName("entity_name")]
        public string? EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        public string? EntityDocumentNumber { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del establecimiento, ejm: 150131", Nullable = false)]
        [Required(ErrorMessage = "EL ubigeo es obligatorio")]
        //EL ubigeo debe tener 6 caracteres
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [MinLength(6)]
        //El ubigeo no puede ser 000000
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string UbigeoId { get; set; }

        [JsonPropertyName("address")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la direccion fiscal del establecimiento", Nullable = false)]
        [MaxLength(250)]
        [Required]
        public string Address { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del establecimiento", Nullable = true)]
        //[MaxLength(50)]
        //[Required]
        //[EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string? Email { get; set; }

        [JsonPropertyName("code_sunat")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el Código de establecimiento o de la Sucursal", Nullable = true)]
        [StringLength(4, ErrorMessage = "El código de establecimiento requiere 4 caracteres")]
        public string CodeSunat { get; set; }


        [JsonPropertyName("is_main")]
        [SwaggerSchema("Recibe un boolean, aquí recibe un indicador si es anexo principal", Nullable = false)]
        public bool IsMain { get; set; }

        [JsonPropertyName("type")]
        [SwaggerSchema("Recibe un texto, se coloca el tipo de establecimiento segpun sunat : 'domicilio_fiscal', 'casa_matriz', 'sucursal', 'agencia', 'local_comercial', 'sede_productiva', 'almacen', 'oficina_administrativa'", Nullable = true)]
        [MaxLength(100)]
        public string Type { get; set; }

    }
}
