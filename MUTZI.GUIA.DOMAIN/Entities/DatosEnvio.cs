using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("datosEnvio", Schema = "logistica")]
public partial class DatosEnvio
{
    [Key]
    [Column("datosEnvioID")]
    public int DatosEnvioId { get; set; }

    [Column("guiaID")]
    public int GuiaId { get; set; }

    [Column("motivoEnvio")]
    [MaxLength(100)]
    public string? MotivoEnvio { get; set; }

    [Column("fechaEnvio")]
    public DateOnly FechaEnvio { get; set; }

    [Column("pesoBruto", TypeName = "decimal(12,3)")]
    public decimal PesoBruto { get; set; }

    [Column("codigoUM")]
    [MaxLength(10)]
    public string CodigoUm { get; set; } = null!;

    [Column("rucEmpresaCurrier")]
    [MaxLength(15)]
    public string? RucEmpresaCurrier { get; set; }

    [Column("razonSocialCurrier")]
    [MaxLength(100)]
    public string? RazonSocialCurrier { get; set; }

    [Column("registroMtcCurrier")]
    [MaxLength(20)]
    public string? RegistroMtcCurrier { get; set; }

    [Column("indicadorVehiculoConductor")]
    public sbyte? IndicadorVehiculoConductor { get; set; } = 0;

    [ForeignKey(nameof(CodigoUm))]
    public virtual UnidadMedida CodigoUmNavigation { get; set; } = null!;

    [ForeignKey(nameof(GuiaId))]
    public virtual GuiaRemision Guia { get; set; } = null!;

    public virtual ICollection<UnidadTransporteGuid> TransporteGuids { get; set; } = new List<UnidadTransporteGuid>();

    public virtual ICollection<ConductorGuid> ConductorGuids { get; set; } = new List<ConductorGuid>();
}
