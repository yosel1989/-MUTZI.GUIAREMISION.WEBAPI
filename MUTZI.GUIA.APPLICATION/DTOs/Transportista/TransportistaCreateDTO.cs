using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Transportista
{
    [SwaggerSchema("Recibe un objeto remite_transportista, tener en cuenta que, si Carolina SAC o la empresa que esté asociada al facturador actúa como transportista este objeto pasa a ser remitente y según Efact, menciona que: 'El 'Número de documento de identidad del remitente', no debe ser igual al número de RUC del transportista.'.", Nullable=false)]
    public class   TransportistaCreateDTO
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
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del remitente_transportista, ejm: 150131", Nullable = true)]
        //EL ubigeo debe tener 6 caracteres
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [MinLength(6)]
        //El ubigeo no puede ser 000000
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string? UbigeoID { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la direccion fiscal del remitente_transportista", Nullable = true)]
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

        [JsonPropertyName("registro_mtc")]
        [SwaggerSchema("Recibe un texto, el codigo de registro MTC del transportista", Nullable = true)]
        public string? RegistroMTC { get; set; }

        [JsonPropertyName("email_contacto")]
        [SwaggerSchema("Recibe un texto, el email de contacto del transportista", Nullable = false)]
        public string? EmailContacto { get; set; }
    }
}
