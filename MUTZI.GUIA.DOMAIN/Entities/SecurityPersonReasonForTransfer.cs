using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("security_person__reason_for_transfer")]
    public class SecurityPersonReasonForTransfer
    {
        [Column("security_person_id")]
        [Description("Identificador único de la persona en la tabla security_person")]
        public int SecurityPersonId { get; set; }

        [Column("reason_for_transfer_id")]
        [Description("Identificador único del motivo de traslado en la tabla master_data.sunat_reason_for_transfer")]
        public int ReasonForTransferId { get; set; }

        [Column("created_at")]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [Column("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; }

        [Column("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; }

        [Column("created_at_employee_id")]
        [Description("Identificador único del personal en la tabla seguridad.PERSONA")]
        public string CretedAtEmployeeId { get; set; }

        // Relaciones

        [ForeignKey(nameof(SecurityPersonId))]
        public virtual SecurityPerson SecurityPerson { get; set; }

        [ForeignKey(nameof(ReasonForTransferId))]
        public virtual MotivoTraslado ReasonForTransfer { get; set; }

    }
}
