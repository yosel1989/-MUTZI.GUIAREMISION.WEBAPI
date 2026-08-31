using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos
{
    public interface ICatalogoService
    {
        IEnumerable<EmisorAutorizadorVehicularDTO> GetEmisoresVehiculares();

        Task<IEnumerable<CountryDTO>> GetCountryCollection();

        IEnumerable<TipoEstablecimientoDTO> GetTiposEstablecimiento();
        
        Task<IEnumerable<TipoDocumentoToSelectDTO>> GetTiposDocumento(string? tipoRegimen);
        
        Task<IEnumerable<EntidadReguladoraToSelectDTO>> GetEntidadesReguladoras();

        Task<IEnumerable<UnidadMedidaToSelectDTO>> GetUnidadesMedida(string? tipo);

        Task<IEnumerable<BienNormalizadoToSelectDTO>> GetBienesNormalizados();
        Task<IEnumerable<DocumentoRelacionadoToSelectDTO>> GetDocumentosRelacionados(string? tipo);
    }
}
