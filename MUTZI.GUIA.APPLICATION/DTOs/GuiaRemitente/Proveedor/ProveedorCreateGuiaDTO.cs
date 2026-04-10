using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor
{
    public class ProveedorCreateGuiaDTO
    {
        [JsonPropertyName("proveedor_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del proveedor que se va a asociar a la guía de remisión", Nullable = false)]
        [Required]
        public int ProveedorId { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el número de documento del proveedor.", Nullable = false)]
        [Required(ErrorMessage = "El Nro documento del proveedor es obligatorio")]
        [MaxLength(21)]
        [MinLength(8)]
        public string NumeroDocumento { get; set; }
    }
}
