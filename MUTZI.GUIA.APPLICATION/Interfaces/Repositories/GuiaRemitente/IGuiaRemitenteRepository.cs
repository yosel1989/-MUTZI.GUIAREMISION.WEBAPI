
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente
{
    public interface IGuiaRemitenteRepository
    {
        Task<List<GuiaRemision>> GetAllAsync();
        Task<(List<GuiaRemision>, int)> GetListAsync(int pageNumber, int pageSize, Dictionary<string, string> filters, bool export = false);
        //Task<(List<DOMAIN.Entities.GuiaRemision>, int)> GetListAsync(int pageNumber, int pageSize, List<ColumnFilterDTO> columnFilterDTOs);
        Task<GuiaRemision?> GetByUuidAsync(Guid uuid);
        Task<GuiaRemision?> GetByIdAsync(int id);
        Task<GuiaRemision> GetByIdAsync2(int id);
        Task<GuiaRemision> GetByTicketAsync(string ticket);
        Task <(int GuiaId, string NumeroGuia)> CreateGuiaRemisionTransactionalAsync(
            Establecimiento remitente,
            Establecimiento destinatario,
            Proveedor proveedor,
            List<ConductorGuid> conductor,
            List<UnidadTransporteGuid> transporte,
            GuiaRemision guiaRemision,
            GuiaRemisionDatoEnvio datosEnvio,
            GuiaRemisionOrigen origen,
            List<GuiaRemisionDestino> destino,
            List<GuiaRemisionDetalle> detalles,
            List<DocRelacionado> docRelacionados,
            List<EmailDestinatarioGuid> emailDestinatarioGuia
            );

        //Proveedor
        Task<Proveedor> GetProveedorByRucAsync(string ruc);
        Task<Proveedor> CreateProveedorAsync(Proveedor proveedor);
        //Conductor
        Task<Conductor> GetConductorByNumeroDocumentoAsync(string numeroDocumento);
        Task<Conductor> CreateConductorAsync(Conductor conductor);
        //Transporte
        Task<UnidadTransporte> GetTransporteByPlacaAsync(string placa);
        Task<UnidadTransporte> CreateTransporteAsync(UnidadTransporte transporte);
        //Datos envio
        Task<GuiaRemisionDatoEnvio> CreateDatosEnvioAsync(GuiaRemisionDatoEnvio datosEnvio);
        //Ubicación origen
        Task<GuiaRemisionOrigen> CreateOrigenAsync(GuiaRemisionOrigen origen);
        //Ubicación destino
        Task<GuiaRemisionDestino> CreateDestinoAsync(GuiaRemisionDestino destino);
        //GuiaRemision detalle
        Task<GuiaRemisionDetalle> CreateGuiaDetalleAsync(GuiaRemisionDetalle guiaDetalle);
        Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado, int idEstado);
        Task<GuiaRemision> UpdateAsync(GuiaRemision guiaRemision);
        Task DeleteAsync(int id);

        Task<GuiaRemision?> FindWithDetail(int id);
    }
}
