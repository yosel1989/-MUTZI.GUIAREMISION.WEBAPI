using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaTransportista.RemiteTransportista
{
    [SwaggerSchema("Recibe un objeto remite_transportista, si es nuevo remite_transportista la api crea un nuevo remite_transportista en la bbdd, tener en cuenta que, si Carolina SAC o la empresa que esté asociada al facturador actúa como transportista este objeto pasa a ser remitente y los datos colocados aquí salen en la guía transportista como remitente.", Nullable=false)]
    public class   RemiteTransportistaCreateDTO
    {
        [JsonPropertyName("ruc")]
        [SwaggerSchema("Recibe un texto, es el ruc del remitente_transportista", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del remitente_transportista es obligatoria")]
        [StringLength(11, ErrorMessage = "El número de documento del remitente_transportista no puede exceder los 11 caracteres")]
        [MinLength(11)]
        public string Ruc { get; set; }

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

        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del remite_transportista", Nullable = false)]
        [MaxLength(50)]
        [Required]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string Email { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el codigo de PAIS referente, ejm: PE", Nullable = false)]
        [MaxLength(3)]
        [Required]
        public string Pais { get; set; }
    }
}
