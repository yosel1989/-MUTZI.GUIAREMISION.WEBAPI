using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo;

[Table("vw_mdata_sunat_invoice_type")]
public partial class InvoiceType
{
    [Key]
    [Column("invoice_type_id")]
    public int InvoiceTypeId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("code_sunat")]
    public string CodeSunat { get; set; }

    [Column("active")]
    public bool Active { get; set; } = true;

}
