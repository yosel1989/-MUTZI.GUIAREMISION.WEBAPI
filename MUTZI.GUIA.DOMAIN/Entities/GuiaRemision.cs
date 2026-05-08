using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision")]
public partial class GuiaRemision
{
    [Key]
    [Column("guia_remision_id")]
    public int GuiaRemisionId { get; set; }

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

    [Column("tipo_traslado")]
    [MaxLength(12)]
    public string TipoTraslado { get; set; } = null!;

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

    [Column("area")]
    [MaxLength(100)]
    public string? Area { get; set; }

    [Column("registro_mtc")]
    [MaxLength(20)]
    public string? RegistroMtc { get; set; }

    [Column("respuesta_ticket")]
    [MaxLength(500)]
    public string RespuestaTicket { get; set; } = null!;

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [Column("fecha_modifico")]
    public DateTime? FechaModifico { get; set; }

    [Column("usuario_registro")]
    public string UsuarioRegistro { get; set; }

    [Column("usuario_registro_nombre")]
    [MaxLength(150)]
    public string UsuarioRegistroNombre { get; set; }

    [Column("usuario_modifico")]
    public string? UsuarioModifico { get; set; }

    [Column("usuario_modifico_nombre")]
    [MaxLength(100)]
    public string? UsuarioModificoNombre { get; set; }

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; }

    [Column("id_estado")]
    public sbyte IdEstado { get; set; }

    [Column("estado_sunat")]
    [MaxLength(50)]
    public string EstadoSunat { get; set; } = "PENDIENTE";

    [Column("id_estado_sunat")]
    public sbyte IdEstadoSunat { get; set; }

    public virtual ICollection<DatosEnvio> DatosEnvios { get; set; } = new List<DatosEnvio>();

    [ForeignKey(nameof(RemitenteId))]
    public Establecimiento Remitente { get; set; } = null!;

    [ForeignKey(nameof(DestinatarioId))]
    public Establecimiento Destinatario { get; set; } = null!;


    public virtual ICollection<GuiaRemisionDestino> Destinos { get; set; } = new List<GuiaRemisionDestino>();

    public virtual ICollection<DocRelacionado> DocRelacionados { get; set; } = new List<DocRelacionado>();

    public virtual ICollection<EmailDestinatarioGuid> EmailDestinatarioGuids { get; set; } = new List<EmailDestinatarioGuid>();

    public virtual ICollection<EmailRemiteTransportistaGuid> EmailRemiteTransportistaGuids { get; set; } = new List<EmailRemiteTransportistaGuid>();

    public virtual ICollection<GuiaRemisionDetalle> GuiaDetalles { get; set; } = new List<GuiaRemisionDetalle>();

    public virtual ICollection<GuiaRemisionOrigen> Origens { get; set; } = new List<GuiaRemisionOrigen>();

    [ForeignKey(nameof(ProveedorId))]
    public virtual Proveedor? Proveedor { get; set; }

    [ForeignKey(nameof(RemiteTransportistaId))]
    public virtual Transportista? RemiteTransportista { get; set; }

    [ForeignKey(nameof(Ruc))]
    public virtual VentaConfig RucNavigation { get; set; } = null!;
}
