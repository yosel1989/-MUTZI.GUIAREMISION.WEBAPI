using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario.Email
{
    [SwaggerSchema("Recibe el objeto email asociado al destinatario.", Nullable = false)]
    public class EmailDestinatarioCreateDTO
    {
        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del destinatario", Nullable = false)]
        [MaxLength(50)]
        [Required]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string Email { get; set; }
    }
}
