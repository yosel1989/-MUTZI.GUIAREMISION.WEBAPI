using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos
{
    public interface IGuiaRemisionIndicadorTrasladoService
    {
        IEnumerable<EmisorAutorizadorVehicularDTO> GetEmisoresVehiculares();
    }
}
