using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("entity")]
    public class Entity
    {
        [Key]
        [Column("entity_id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [Column("type")]
        [Required]
        [Description("Tipo de entidad: company o person")]
        public string Type { get; set; } = "company";

        [Column("name")]
        [Description("Razón social si es empresa")]
        public string? Name { get; set; }

        [Column("first_name")]
        [Description("Nombre si es persona")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        [Description("Apellido si es persona")]
        public string? LastName { get; set; }

        [Column("document_type_id")]
        [Description("Id del tipo de documento (DNI, RUC, etc.) de la tabla master_data.document_type")]
        public int DocumentTypeId { get; set; }

        [Column("document_number")]
        [Description("Número de documento de identidad o RUC")]
        public string DocumentNumber { get; set; }

        [Column("ubigeo_id")]
        [Description("Código de ubicación geográfica (Ubigeo)")]
        public string? UbigeoId { get; set; }

        [Column("address")]
        [Description("Dirección física de la entidad")]
        public string? Address { get; set; }

        [Column("country_id")]
        [Description("Identificador del país")]
        public int? CountryId { get; set; }

        [Column("is_internal")]
        [Description("Indica si la entidad pertenece al consorcio o no es empresa tercera.")]
        public bool IsInternal { get; set; } = false;

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

        [ForeignKey(nameof(DocumentTypeId))]
        public virtual TipoDocumento DocumentType { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        [ForeignKey(nameof(UbigeoId))]
        public virtual Ubigeo Ubigeo { get; set; }

        public virtual List<EntityRole> Roles { get; set; } = new List<EntityRole>();
    }
}
