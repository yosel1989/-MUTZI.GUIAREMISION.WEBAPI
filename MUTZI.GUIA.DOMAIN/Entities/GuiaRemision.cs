using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guiaRemision")]
public partial class GuiaRemision
{
    [Key]
    [Column("guiaID")]
    public int GuiaId { get; set; }

    [Column("numeroGuia")]
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

    [Column("tipoGuia")]
    [MaxLength(20)]
    public string TipoGuia { get; set; } = null!;

    [Column("tipoTransporte")]
    [MaxLength(10)]
    public string TipoTransporte { get; set; } = null!;

    [Column("tipoTraslado")]
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

    [Column("remitenteID")]
    public int RemitenteId { get; set; }

    [Column("remiteTransportistaID")]
    public int? RemiteTransportistaId { get; set; }

    [Column("destinatarioID")]
    public int DestinatarioId { get; set; }

    [Column("proveedorID")]
    public int? ProveedorId { get; set; }

    [Column("area")]
    [MaxLength(100)]
    public string? Area { get; set; }

    [Column("registroMTC")]
    [MaxLength(20)]
    public string? RegistroMtc { get; set; }

    [Column("respuestaTicket")]
    [MaxLength(500)]
    public string RespuestaTicket { get; set; } = null!;

    [Column("fechaCreacion")]
    public DateTime? FechaCreacion { get; set; } = DateTime.UtcNow;

    [Column("fechaEdicion")]
    public DateTime? FechaEdicion { get; set; }

    [Column("empleadoIDCreacion")]
    public int? EmpleadoIdcreacion { get; set; }

    [Column("empleadoNombreCreacion")]
    [MaxLength(100)]
    public string? EmpleadoNombreCreacion { get; set; }

    [Column("empleadoIDEdicion")]
    public int? EmpleadoIdedicion { get; set; }

    [Column("empleadoNombreEdicion")]
    [MaxLength(100)]
    public string? EmpleadoNombreEdicion { get; set; }

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; }

    [Column("idEstado")]
    public sbyte IdEstado { get; set; }

    [Column("estadoSunat")]
    [MaxLength(50)]
    public string EstadoSunat { get; set; } = "PENDIENTE";

    [Column("idEstadoSunat")]
    public sbyte IdEstadoSunat { get; set; }

    public virtual ICollection<DatosEnvio> DatosEnvios { get; set; } = new List<DatosEnvio>();

    [ForeignKey(nameof(RemitenteId))]
    public Establecimiento Remitente { get; set; } = null!;

    [ForeignKey(nameof(DestinatarioId))]
    public Establecimiento Destinatario { get; set; } = null!;


    public virtual ICollection<Destino> Destinos { get; set; } = new List<Destino>();

    public virtual ICollection<DocRelacionado> DocRelacionados { get; set; } = new List<DocRelacionado>();

    public virtual ICollection<EmailDestinatarioGuid> EmailDestinatarioGuids { get; set; } = new List<EmailDestinatarioGuid>();

    public virtual ICollection<EmailRemiteTransportistaGuid> EmailRemiteTransportistaGuids { get; set; } = new List<EmailRemiteTransportistaGuid>();

    public virtual ICollection<GuiaDetalle> GuiaDetalles { get; set; } = new List<GuiaDetalle>();

    public virtual ICollection<Origen> Origens { get; set; } = new List<Origen>();

    [ForeignKey(nameof(ProveedorId))]
    public virtual Proveedor? Proveedor { get; set; }

    [ForeignKey(nameof(RemiteTransportistaId))]
    public virtual RemiteTransportistum? RemiteTransportista { get; set; }

    [ForeignKey(nameof(Ruc))]
    public virtual VentaConfig RucNavigation { get; set; } = null!;
}
