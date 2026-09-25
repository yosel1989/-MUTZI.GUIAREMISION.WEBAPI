using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonReasonForTransfer
{
    public class SecurityPersonReasonForTransferDTO
    {
        [JsonPropertyName("security_person_id")]
        [Description("Identificador único del registro de la persona que utilizará el sistema")]
        public int SecurityPersonId { get; set; }

        [JsonPropertyName("reason_for_transfer_id")]
        [Description("Identificador único del motivo de traslado.")]
        public int ReasonForTransferId { get; set; }

        [JsonPropertyName("reason_for_transfer_name")]
        [Description("Nombre del motivo de traslado.")]
        public string ReasonForTransferName { get; set; }

        [JsonPropertyName("reason_for_transfer_code_sunat")]
        [Description("Código SUNAT del motivo de traslado.")]
        public string ReasonForTransferCodeSunat { get; set; }

        [JsonPropertyName("created_at")]
        [Description("Fecha de cuando se realizo el registro en la bd")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; }

        [JsonPropertyName("created_at_employee_id")]
        [Description("Identificador único del empleado que creó el registro")]
        public string CreatedAtEmployeeId { get; set; }

    }
}
