using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class VentaConfig
{
    public string VentaConfigId { get; set; } = null!;

    public string Ruc { get; set; } = null!;

    public string Empresa { get; set; } = null!;

    public string CodigoSunat { get; set; } = null!;

    public string? EfactApi { get; set; }

    public string? DireccionFiscal { get; set; }

    public string? RetencionCuenta { get; set; }

    public string? RetencionDescripcion { get; set; }
}
