using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino
{
    [SwaggerSchema("Recibe un objeto destino, aquí se detalla el punto de llegada de la guía.", Nullable = false)]
    public class DestinoCreateDTO
    {
        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección del punto de llegada o destino, ejm: 150131", Nullable = false)]
        [Required(ErrorMessage = "EL ubigeo es obligatorio")]
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string UbigeoId { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la dirección de destino o del punto de llegada", Nullable = false)]
        [MaxLength(250)]
        [Required]
        public string Direccion { get; set; }
        //public string Ciudad { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el Codigo de PAIS referente, ejm: PE", Nullable = false)]
        [MaxLength(3)]
        [Required]
        public string Pais { get; set; }
    }
}
