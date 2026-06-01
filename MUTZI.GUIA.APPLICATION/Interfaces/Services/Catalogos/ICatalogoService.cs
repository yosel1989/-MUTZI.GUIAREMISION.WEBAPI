using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos
{
    public interface ICatalogoService
    {
        IEnumerable<EmisorAutorizadorVehicularDTO> GetEmisoresVehiculares();
        
        IEnumerable<PaisDTO> GetPaises();
        
        IEnumerable<TipoEstablecimientoDTO> GetTiposEstablecimiento();
        
        Task<IEnumerable<TipoDocumentoToSelectDTO>> GetTiposDocumento(string? tipoRegimen);
        
        Task<IEnumerable<EntidadReguladoraToSelectDTO>> GetEntidadesReguladoras();

        Task<IEnumerable<UnidadMedidaToSelectDTO>> GetUnidadesMedida(string? tipo);
    }
}
