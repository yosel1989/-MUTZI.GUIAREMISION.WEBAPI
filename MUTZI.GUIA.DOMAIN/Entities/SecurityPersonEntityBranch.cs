using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("segurity_person__entity_branch")]
    public class SecurityPersonEntityBranch
    {
        [Column("segurity_person_id")]
        [Description("Identificador único del registro de la persona de seguridad.")]
        public int SecurityPersonId { get; set; }

        [Column("entity_branch_id")]
        [Description("Identificador único del registro del establecimiento..")]
        public int EntityBranchId { get; set; }

        [Column("created_at")]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [Column("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; }

        [Column("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; }

        // Relaciones

        [ForeignKey(nameof(EntityBranchId))]
        public virtual EntityBranch EntityBranch { get; set; }

    }
}
