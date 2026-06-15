using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    public DateOnly? FechaEnvio { get; set; }

    [Column("fecha_entrega_transportista")]
    public DateOnly? FechaEntregaTransportista { get; set; }

    [Column("peso_bruto", TypeName = "decimal(12,3)")]
    public decimal PesoBruto { get; set; }

    [Column("unidad_medida_id")]
    public int UnidadMedidaId { get; set; }

    [Column("codigo_um")]
    [MaxLength(10)]
    public string CodigoUm { get; set; } = null!;

    [Column("indicador_traslado_vehiculo_categoria")]
    public bool IndicadorTrasladoVehiculoCategoria { get; set; } = false;

    [Column("traslado_vehiculo_categoria_placa_vehiculo")]
    public string? TrasladoVehiculoCategoriaPlacaVehiculo { get; set; } = null;

    [Column("transportista_id")]
    public int? TransportistaId { get; set; } = null;

    [Column("indicador_registro_vehiculo_conductor")]
    public bool IndicadorRegistroVehiculoConductor { get; set; } = false;

    [Column("indicador_transbordo_programado")]
    public bool IndicadorTransbordoProgramado { get; set; } = false;

    [Column("indicador_retorno_vehiculo_envases_vacios")]
    public bool IndicadorRetornoVehiculoEnvasesVacios { get; set; } = false;

    [Column("indicador_retorno_vehiculo_vacio")]
    public bool IndicadorRetornoVechiuloVacio { get; set; } = false;

    [Column("indicador_traslado_total_dam")]
    public bool IndicadorTrasladoTotalDam { get; set; } = false;

    // Relaciones

    [ForeignKey(nameof(UnidadMedidaId))]
    public virtual UnidadMedida UnidadMedida { get; set; } = null!;

    [ForeignKey(nameof(GuiaRemisionId))]
    public virtual GuiaRemision Guia { get; set; } = null!;

    [ForeignKey(nameof(TransportistaId))]
    public virtual Transportista? Transportista { get; set; } = null!;

    public virtual ICollection<GuiaRemisionUnidadTransporte> GuiaRemisionUnidadesTransporte { get; set; } = new List<GuiaRemisionUnidadTransporte>();

    public virtual ICollection<GuiaRemisionConductor> GuiaRemisionConductores { get; set; } = new List<GuiaRemisionConductor>();
}
