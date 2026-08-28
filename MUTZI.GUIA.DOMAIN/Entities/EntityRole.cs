using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("entity_role")]
    public class EntityRole
    {
        [Key]
        [Column("entity_id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [Column("role")]
        [Required]
        [Description("Rol de entidad: client,supplier,transporter,issuer")]
        public string Role { get; set; } = "issuer";

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

        [ForeignKey(nameof(EntityId))]
        public virtual Entity Entity { get; set; }

    }
}
