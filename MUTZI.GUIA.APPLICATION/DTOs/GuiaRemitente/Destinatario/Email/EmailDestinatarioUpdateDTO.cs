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
    public class EmailDestinatarioUpdateDTO
    {
        [JsonPropertyName("email_destinatario_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID del email asociado al destinatario, OJO: Colocar el mismo ID si el email solo se está actualizando, en caso de ser nuevo email se debe dejar como valor cero '0'.", Nullable = false)]
        [Required]
        public int EmailDestinatarioID { get; set; }

        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del destinatario", Nullable = false)]
        [MaxLength(50)]
        [Required]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string Email { get; set; }
    }
}
