using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("segurity_person")]
    public class SecurityPerson
    {
        [Key]
        [Column("segurity_person_id")]
        [Description("Identificador único del registro de la persona de seguridad del local.")]
        public int SecurityPersonId { get; set; }

        [Column("document_number")]
        [Description("Número de documento de identidad")]
        public string DocumentNumber { get; set; }

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


        // Condicionales

        [ForeignKey(nameof(DocumentNumber))]
        public virtual Persona Persona { get; set; }

    }
}
