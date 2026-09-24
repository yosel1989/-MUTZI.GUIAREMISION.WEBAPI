using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision")]
public partial class GuiaRemision
{
    [Key]
    [Column("guia_remision_id")]
    public int GuiaRemisionId { get; set; }

    [Column("guia_remision_uuid")]
    public Guid GuiaRemisionUuid { get; set; } = Guid.NewGuid();

    [Column("entity_id")]
    public int? EntityId { get; set; }

    [Column("numero_guia")]
    [MaxLength(15)]
    public string NumeroGuia { get; set; } = null!;

    [Column("ruc")]
    [MaxLength(11)]
    public string Ruc { get; set; } = null!;

    [Column("serie")]
    [MaxLength(4)]
    public string Serie { get; set; } = null!;

    [Column("correlativo")]
    [MaxLength(8)]
    public string Correlativo { get; set; } = null!;

    [Column("tipo_guia")]
    [MaxLength(20)]
    public string TipoGuia { get; set; } = null!;

    [Column("tipo_transporte")]
    [MaxLength(10)]
    public string TipoTransporte { get; set; } = null!;

    [Column("fecha")]
    public DateOnly Fecha { get; set; }

    [Column("hora")]
    [MaxLength(10)]
    public string Hora { get; set; } = null!;

    [Column("observacion")]
    [MaxLength(100)]
    public string? Observacion { get; set; }

    [Column("remitente_id")]
    public int RemitenteId { get; set; }

    [Column("remite_transportista_id")]
    public int? RemiteTransportistaId { get; set; }

    [Column("destinatario_id")]
    public int DestinatarioId { get; set; }

    [Column("proveedor_id")]
    public int? ProveedorId { get; set; }

    [Column("area_id")]
    public int AreaId { get; set; }

    [Column("registro_mtc")]
    [MaxLength(20)]
    public string? RegistroMtc { get; set; }

    [Column("respuesta_ticket")]
    [MaxLength(500)]
    public string RespuestaTicket { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_at_user")]
    public string CreatedAtUser { get; set; }

    [Column("created_at_user_name")]
    [MaxLength(150)]
    public string CreatedAtUserName { get; set; }

    [Column("created_at_employee_id")]
    public int CreatedAtEmployeeId { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_at_user")]
    public string? UpdatedAtUser { get; set; }

    [Column("updated_at_user_name")]
    public string? UpdatedAtUserName { get; set; }

    [Column("updated_at_employee_id")]
    public int UpdatedAtEmployeeId { get; set; }

    [Column("estado_id")]
    public int EstadoId { get; set; }

    [Column("estado_sunat")]
    [MaxLength(50)]
    public string EstadoSunat { get; set; } = "PENDIENTE";

    [Column("estado_sunat_id")]
    public sbyte EstadoSunatId { get; set; }

    [Column("motivo_traslado_id")]
    public int MotivoTrasladoId { get; set; }

    // Relaciones

    [ForeignKey(nameof(EntityId))]
    public virtual Entity? Entity { get; set; }

    [ForeignKey(nameof(Ruc))]
    public virtual VentaConfig RucNavigation { get; set; } = null!;

    [ForeignKey(nameof(RemitenteId))]
    public EntityBranch Remitente { get; set; } = null!;

    [ForeignKey(nameof(DestinatarioId))]
    public EntityBranch Destinatario { get; set; } = null!;

    [ForeignKey(nameof(ProveedorId))]
    public virtual Proveedor? Proveedor { get; set; }

    [ForeignKey(nameof(EstadoId))]
    public virtual GuiaRemisionEstado Estado { get; set; }

    [ForeignKey(nameof(AreaId))]
    public virtual Area Area { get; set; }

    [ForeignKey(nameof(MotivoTrasladoId))]
    public virtual MotivoTraslado MotivoTraslado { get; set; }

    [ForeignKey(nameof(RemiteTransportistaId))]
    public virtual Transportista? RemiteTransportista { get; set; }

    public virtual ICollection<GuiaRemisionDatoEnvio> DatosEnvios { get; set; } = new List<GuiaRemisionDatoEnvio>();
    public virtual ICollection<GuiaRemisionEntityCarrier> GuiaRemisionEntityCarriers { get; set; } = new List<GuiaRemisionEntityCarrier>();
    public virtual ICollection<GuiaRemisionOrigen> Origens { get; set; } = new List<GuiaRemisionOrigen>();

    public virtual ICollection<GuiaRemisionDestino> Destinos { get; set; } = new List<GuiaRemisionDestino>();

    public virtual ICollection<GuiaRemisionDocumentoRelacionado> DocRelacionados { get; set; } = new List<GuiaRemisionDocumentoRelacionado>();

    public virtual ICollection<GuiaRemisionEmailDestinatario> EmailDestinatarioGuids { get; set; } = new List<GuiaRemisionEmailDestinatario>();

    public virtual ICollection<EmailRemiteTransportistaGuid> EmailRemiteTransportistaGuids { get; set; } = new List<EmailRemiteTransportistaGuid>();

    public virtual ICollection<GuiaRemisionDetalle> GuiaDetalles { get; set; } = new List<GuiaRemisionDetalle>();
    public virtual ICollection<GuiaRemisionTransportUnit> GuiaRemisionTransportUnits { get; set; } = new List<GuiaRemisionTransportUnit>();
}
