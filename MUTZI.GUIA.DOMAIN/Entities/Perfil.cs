using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("perfil")]
public partial class Perfil
{
    [Key]
    [Column("perfil_id")]
    public int PerfilId { get; set; }

    [Column("nombre")]
    [MaxLength(75)]
    public string Nombre { get; set; }

    [Column("descripcion")]
    [MaxLength(150)]
    public string? Descripcion { get; set; }

    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("estado_id")]
    public int EstadoId { get; set; } = 1;

    [Column("estado")]
    public string Estado { get; set; } = "activo";

    [Column("usuario_registro")]
    [MaxLength(45)]
    public string UsuarioRegistro { get; set; }

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; }

    [Column("usuario_modifico")]
    [MaxLength(45)]
    public string? UsuarioModifico { get; set; }

    [Column("fecha_modifico")]
    public DateTime? FechaModifico { get; set; }


    public virtual ICollection<PerfilPermiso> PerfilPermisos { get; set; } = new List<PerfilPermiso>();
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
