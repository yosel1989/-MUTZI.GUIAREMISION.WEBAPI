using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("integration_credential")]
    public class IntegrationCredential
    {
        [Key]
        [Column("integration_credential_id")]
        [Description("Identificador único de la integración.")]
        public int IntegrationCredentialId { get; set; }

        [Column("entity_id")]
        [Description("Identificador único de la entidad")]
        public int? EntityId { get; set; }

        [Column("provider")]
        [Description("Proveedor del servicio a integrar")]
        public string Provider { get; set; }

        [Column("key")]
        [Description("Llave para servicio a integrar")]
        public string Key { get; set; }

        [Column("key_test")]
        [Description("Llave para prueba del servicio a integrar")]
        public string? KeyTest { get; set; }

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

        [ForeignKey(nameof(EntityId))]
        public virtual Entity? Entity { get; set; }

    }
}
