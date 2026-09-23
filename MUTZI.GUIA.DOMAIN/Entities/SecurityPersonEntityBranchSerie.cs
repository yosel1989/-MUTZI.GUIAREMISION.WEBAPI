using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("security_person__entity_branch_serie")]
    public class SecurityPersonEntityBranchSerie
    {
        [Column("security_person_id")]
        [Description("Identificador único del registro de la persona de seguridad.")]
        public int SecurityPersonId { get; set; }

        [Column("entity_branch_serie_id")]
        [Description("Identificador único del registro de series.")]
        public int EntityBranchSerieId { get; set; }

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

        [ForeignKey(nameof(SecurityPersonId))]
        public virtual SecurityPerson SecurityPerson { get; set; }

        [ForeignKey(nameof(EntityBranchSerieId))]
        public virtual EntityBranchSerie EntityBranchSerie { get; set; }

    }
}
