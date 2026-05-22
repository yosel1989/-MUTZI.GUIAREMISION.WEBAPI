using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("permiso")]
public partial class Permiso
{
    [Key]
    [Column("permiso_id")]
    public int PermisoId { get; set; }

    [Column("nombre")]
    [MaxLength(75)]
    public string Nombre { get; set; }

    [Column("descripcion")]
    [MaxLength(150)]
    public string? Descripcion { get; set; }

    [Column("codigo")]
    [MaxLength(75)]
    public string Codigo { get; set; }

    [Column("estado_id")]
    public int EstadoId { get; set; } = 1;

    public virtual ICollection<PerfilPermiso> PerfilPermisos { get; set; } = new List<PerfilPermiso>();
    public virtual ICollection<Perfil> Perfiles { get; set; } = new List<Perfil>();


}
