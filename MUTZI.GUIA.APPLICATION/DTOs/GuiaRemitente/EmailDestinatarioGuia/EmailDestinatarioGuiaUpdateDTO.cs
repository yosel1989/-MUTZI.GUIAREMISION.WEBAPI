using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.EmailDestinatarioGuia
{
    public class EmailDestinatarioGuiaUpdateDTO
    {
        [JsonPropertyName("email_destinatario_guia_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del email de destinatario asociado a la guía, OJO: Colocar el mismo ID si el email sigue siendo el mismo, en caso de ser nuevo email se debe dejar como valor cero '0'.", Nullable = false)]
        [Required]
        public int EmailDestinatarioGuidId { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico ", Nullable = false)]
        [MaxLength(50)]
        [Required]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string Email { get; set; }
    }
}
