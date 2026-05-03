using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos
{
    public interface ICatalogoService
    {
        IEnumerable<EmisorAutorizadorVehicularDTO> GetEmisoresVehiculares();
        IEnumerable<PaisDTO> GetPaises();
        IEnumerable<TipoEstablecimientoDTO> GetTiposEstablecimiento();
    }
}
