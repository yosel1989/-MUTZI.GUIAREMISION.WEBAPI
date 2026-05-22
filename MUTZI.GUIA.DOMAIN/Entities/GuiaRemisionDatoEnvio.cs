using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_dato_envio")]
public partial class GuiaRemisionDatoEnvio
{
    [Key]
    [Column("guia_remision_dato_envio_id")]
    public int GuiaRemisionDatoEnvioId { get; set; }

    [Column("guia_remision_id")]
    public int GuiaRemisionId { get; set; }

    [Column("motivo_envio")]
    [MaxLength(100)]
    public string? MotivoEnvio { get; set; }

    [Column("fecha_envio")]
    public DateOnly FechaEnvio { get; set; }

    [Column("peso_bruto", TypeName = "decimal(12,3)")]
    public decimal PesoBruto { get; set; }

    [Column("unidad_medida_id")]
    public int UnidadMedidaId { get; set; }

    [Column("codigo_um")]
    [MaxLength(10)]
    public string CodigoUm { get; set; } = null!;

    [Column("ruc_empresa_currier")]
    [MaxLength(15)]
    public string? RucEmpresaCurrier { get; set; }

    [Column("razon_social_currier")]
    [MaxLength(100)]
    public string? RazonSocialCurrier { get; set; }

    [Column("registro_mtc_currier")]
    [MaxLength(20)]
    public string? RegistroMtcCurrier { get; set; }

    [Column("indicador_vehiculo_conductor")]
    public sbyte? IndicadorVehiculoConductor { get; set; } = 0;

    // Relaciones

    [ForeignKey(nameof(UnidadMedidaId))]
    public virtual UnidadMedida UnidadMedida { get; set; } = null!;

    [ForeignKey(nameof(GuiaRemisionId))]
    public virtual GuiaRemision Guia { get; set; } = null!;

    public virtual ICollection<UnidadTransporteGuid> TransporteGuids { get; set; } = new List<UnidadTransporteGuid>();

    public virtual ICollection<GuiaRemisionConductor> ConductorGuids { get; set; } = new List<GuiaRemisionConductor>();
}
