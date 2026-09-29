using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("area")]
    public class Area
    {
        [Key]
        [Column("area_id")]
        [Description("Identificador único del área")]
        public int AreaId { get; set; }

        [Column("code")]
        [Required]
        [Description("Código del área: 2 dígitos si es principal (10), 4 dígitos si es subárea (1001)")]
        public string Code { get; set; } = null!;

        [Column("name")]
        [Required]
        [Description("Nombre completo del área. En subáreas incluye al padre: 'Producción – Envasado'")]
        public string Name { get; set; } = null!;

        [Column("parent_area_id")]
        [Description("Id del área padre. NULL si es un área principal")]
        public int? ParentAreaId { get; set; }

        [Column("level")]
        [Description("Nivel jerárquico: 1 = área principal, 2 = subárea")]
        public byte Level { get; set; }

        [Column("active")]
        [Description("Estado del área: activa o inactiva")]
        public bool Active { get; set; } = true;

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [Column("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; } = null!;

        [Column("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; } = null!;

        [Column("created_at_employee_id")]
        [Description("Id del empleado que creó el registro, si el usuario está vinculado a un empleado")]
        public int? CreatedAtEmployeeId { get; set; }

        [Column("updated_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Description("Fecha de última modificación")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_at_user")]
        [Description("Usuario que modificó el registro")]
        public string? UpdatedAtUser { get; set; }

        [Column("updated_at_user_name")]
        [Description("Nombre del usuario que modificó el registro")]
        public string? UpdatedAtUserName { get; set; }

        [Column("updated_at_employee_id")]
        [Description("Id del empleado que modificó el registro, si el usuario está vinculado a un empleado")]
        public int? UpdatedAtEmployeeId { get; set; }

        // Relaciones

        [ForeignKey(nameof(ParentAreaId))]
        [InverseProperty(nameof(Children))]
        [Description("Área padre (navegación)")]
        public virtual Area? Parent { get; set; }

        [InverseProperty(nameof(Parent))]
        [Description("Subáreas que dependen de esta área (navegación)")]
        public virtual ICollection<Area> Children { get; set; } = new List<Area>();
    }

}
