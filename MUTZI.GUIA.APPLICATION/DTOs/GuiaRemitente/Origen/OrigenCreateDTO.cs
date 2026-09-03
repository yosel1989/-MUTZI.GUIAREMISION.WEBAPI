using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen
{
    [SwaggerSchema("Recibe un objeto origen, aquí se detalla el punto de partida de la guía.", Nullable = false)]
    public class OrigenCreateDTO
    {
        [JsonPropertyName("ubigeo_id")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ubigeo de la dirección fiscal del proveedor, ejm: 150131", Nullable = false)]
        [Required(ErrorMessage = "EL ubigeo es obligatorio")]
        [StringLength(6, ErrorMessage = "El ubigeo debe tener 6 caracteres")]
        [RegularExpression(@"^(?!000000)\d{6}$", ErrorMessage = "El ubigeo debe tener 6 caracteres y no puede ser 000000")]
        public string UbigeoId { get; set; }

        [JsonPropertyName("direccion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la dirección de origen o del punto de partida", Nullable = false)]
        [MaxLength(250)]
        [Required]
        public string Direccion { get; set; }
        //public string Ciudad { get; set; }
        [JsonPropertyName("pais")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el CodeSunat de PAIS referente, ejm: PE", Nullable = false)]
        [MaxLength(3)]
        [Required]
        public string Pais { get; set; }
    }
}
