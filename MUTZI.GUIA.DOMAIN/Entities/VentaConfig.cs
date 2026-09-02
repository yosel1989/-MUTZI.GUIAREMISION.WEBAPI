using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{

    [Table("VentaConfig")]
    public partial class VentaConfig
    {
        [Column("ventaConfigID")]
        [MaxLength(36)]
        public string VentaConfigId { get; set; } = null!;

        [Key]
        [Column("Ruc")]
        [MaxLength(11)]
        public string Ruc { get; set; } = null!;

        [Column("Empresa")]
        [MaxLength(64)]
        public string Empresa { get; set; } = null!;

        [Column("emailPrincipal")]
        [MaxLength(50)]
        public string EmailPrincipal { get; set; } = null!;

        [Column("CodigoSUNAT")]
        [MaxLength(4)]
        public string CodigoSunat { get; set; } = null!;

        [Column("Efact_API")]
        [MaxLength(256)]
        public string? EfactApi { get; set; }

        [Column("direccionFiscal")]
        [MaxLength(100)]
        public string? DireccionFiscal { get; set; }

        [Column("retencionCuenta")]
        [MaxLength(50)]
        public string? RetencionCuenta { get; set; }

        [Column("retencionDescripcion")]
        [MaxLength(100)]
        public string? RetencionDescripcion { get; set; }


        [Column("tipo_documento")]
        [MaxLength(45)]
        public string? TipoDocumento { get; set; }

        [Column("numero_documento")]
        [MaxLength(45)]
        public string? NumeroDocumento { get; set; }


        public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

        public virtual ICollection<EntityBranch> Establecimientos { get; set; } = new List<EntityBranch>();
    }

}
