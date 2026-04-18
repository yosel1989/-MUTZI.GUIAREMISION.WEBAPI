using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class DatosEnvio
{
    public int DatosEnvioId { get; set; }

    public int GuiaId { get; set; }

    public string? MotivoEnvio { get; set; }

    public DateOnly FechaEnvio { get; set; }

    public decimal PesoBruto { get; set; }

    public string CodigoUm { get; set; } = null!;

    public string? RucEmpresaCurrier { get; set; }

    public string? RazonSocialCurrier { get; set; }

    public string? RegistroMtcCurrier { get; set; }

    public sbyte? IndicadorVehiculoConductor { get; set; }

    public virtual UnidadMedidum CodigoUmNavigation { get; set; } = null!;

    public virtual ICollection<ConductorGuid> ConductorGuids { get; set; } = new List<ConductorGuid>();

    public virtual GuiaRemision Guia { get; set; } = null!;

    public virtual ICollection<UnidadTransporteGuid> TransporteGuids { get; set; } = new List<UnidadTransporteGuid>();
}
