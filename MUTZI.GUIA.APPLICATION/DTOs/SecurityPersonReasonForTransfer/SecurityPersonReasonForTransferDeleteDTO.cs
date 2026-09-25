using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonReasonForTransfer
{
    public class SecurityPersonReasonForTransferDeleteDTO
    {
        [JsonPropertyName("security_person_id")]
        [Description("Identificador único del registro de la persona que utilizará el sistema")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("reason_for_transfer_id")]
        [Description("Identificador único del motivo de traslado.")]
        public int ReasonForTransferId { get; set; }
    }
}
