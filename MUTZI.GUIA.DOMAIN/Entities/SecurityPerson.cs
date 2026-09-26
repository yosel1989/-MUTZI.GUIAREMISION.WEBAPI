using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("security_person")]
    public class SecurityPerson
    {
        [Key]
        [Column("security_person_id")]
        [Description("Identificador único del registro de la persona de seguridad del local.")]
        public int SecurityPersonId { get; set; }

        [Column("person_id")]
        [Description("Identificador único del registro de la tabla persona.")]
        public int PersonId { get; set; }

        [Column("active")]
        [Description("Estado de la entidad: activa o inactiva")]
        public bool Active { get; set; } = true;

        [Column("created_at")]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [Column("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; }

        [Column("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; }

        [Column("updated_at")]
        [Description("Fecha de última modificación")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_at_user")]
        [Description("Usuario que modificó el registro")]
        public string? UpdatedAtUser { get; set; }

        [Column("updated_at_user_name")]
        [Description("Nombre del usuario que modificó el registro")]
        public string? UpdatedAtUserName { get; set; }

        // Relaciones

        [ForeignKey(nameof(PersonId))]
        public virtual Persona Persona { get; set; }

        public List<SecurityPersonEntityBranchSerie> SecurityPersonEntityBranchSeries = new List<SecurityPersonEntityBranchSerie>();

        public List<SecurityPersonEntityBranch> SecurityPersonEntityBranchs = new List<SecurityPersonEntityBranch>();

        public List<SecurityPersonReasonForTransfer> SecurityPersonReasonForTransfers = new List<SecurityPersonReasonForTransfer>();
    }
}

