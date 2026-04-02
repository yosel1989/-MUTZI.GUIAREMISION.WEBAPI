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
    [SwaggerSchema("Recibe el objeto email asociado a la guía.", Nullable = true)]
    public class EmailDestinatarioGuiaCreateDTO
    {
        [JsonPropertyName("email")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el correo electrónico del destinatario", Nullable = true)]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string? Email { get; set; }
    }
}
