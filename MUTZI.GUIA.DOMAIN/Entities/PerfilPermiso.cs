using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("perfil_permiso")]
public partial class PerfilPermiso
{
    [Column("perfil_id")]
    public int PerfilId { get; set; }

    [Column("permiso_id")]
    public int PermisoId { get; set; }

    [Column("usuario_registro")]
    [MaxLength(45)]
    public string UsuarioRegistro { get; set; }

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(PerfilId))]
    public virtual Perfil Perfil { get; set; }

    [ForeignKey(nameof(PermisoId))]
    public virtual Permiso Permiso { get; set; }

}
