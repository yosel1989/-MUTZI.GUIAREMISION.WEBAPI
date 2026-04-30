using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("conductorGuid")]
public partial class ConductorGuid
{
    [Key]
    [Column("conductorGuidID")]
    public int ConductorGuidId { get; set; }

    [Column("conductorID")]
    public int? ConductorId { get; set; }

    [Column("datosEnvioID")]
    public int? DatosEnvioId { get; set; }

    [ForeignKey(nameof(ConductorId))]
    public virtual Conductor? Conductor { get; set; }

    [ForeignKey(nameof(DatosEnvioId))]
    public virtual DatosEnvio? DatosEnvio { get; set; }
}
