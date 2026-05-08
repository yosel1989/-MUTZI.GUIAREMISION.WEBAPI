
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente
{
    public interface IGuiaRemitenteRepository
    {
        Task<List<DOMAIN.Entities.GuiaRemision>> GetAllAsync();
        Task<(List<DOMAIN.Entities.GuiaRemision>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO? request, bool export = false);
        //Task<(List<DOMAIN.Entities.GuiaRemision>, int)> GetListAsync(int pageNumber, int pageSize, List<ColumnFilterDTO> columnFilterDTOs);
        Task<DOMAIN.Entities.GuiaRemision?> GetByIdAsync(int id);
        Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync2(int id);
        Task<DOMAIN.Entities.GuiaRemision> GetByTicketAsync(string ticket);
        Task <(int GuiaId, string NumeroGuia)> CreateGuiaRemisionTransactionalAsync(
            Establecimiento remitente,
            Establecimiento destinatario,
            DOMAIN.Entities.Proveedor proveedor,
            List<DOMAIN.Entities.ConductorGuid> conductor,
            List<DOMAIN.Entities.UnidadTransporteGuid> transporte,
            DOMAIN.Entities.GuiaRemision guiaRemision,
            DOMAIN.Entities.DatosEnvio datosEnvio,
            DOMAIN.Entities.GuiaRemisionOrigen origen,
            List<DOMAIN.Entities.GuiaRemisionDestino> destino,
            List<DOMAIN.Entities.GuiaRemisionDetalle> detalles,
            List<DOMAIN.Entities.DocRelacionado> docRelacionados,
            List<DOMAIN.Entities.EmailDestinatarioGuid> emailDestinatarioGuia
            );

        //Proveedor
        Task<DOMAIN.Entities.Proveedor> GetProveedorByRucAsync(string ruc);
        Task<DOMAIN.Entities.Proveedor> CreateProveedorAsync(DOMAIN.Entities.Proveedor proveedor);
        //Conductor
        Task<DOMAIN.Entities.Conductor> GetConductorByNumeroDocumentoAsync(string numeroDocumento);
        Task<DOMAIN.Entities.Conductor> CreateConductorAsync(DOMAIN.Entities.Conductor conductor);
        //Transporte
        Task<DOMAIN.Entities.UnidadTransporte> GetTransporteByPlacaAsync(string placa);
        Task<DOMAIN.Entities.UnidadTransporte> CreateTransporteAsync(DOMAIN.Entities.UnidadTransporte transporte);
        //Datos envio
        Task<DOMAIN.Entities.DatosEnvio> CreateDatosEnvioAsync(DOMAIN.Entities.DatosEnvio datosEnvio);
        //Ubicación origen
        Task<DOMAIN.Entities.GuiaRemisionOrigen> CreateOrigenAsync(DOMAIN.Entities.GuiaRemisionOrigen origen);
        //Ubicación destino
        Task<DOMAIN.Entities.GuiaRemisionDestino> CreateDestinoAsync(DOMAIN.Entities.GuiaRemisionDestino destino);
        //GuiaRemision detalle
        Task<DOMAIN.Entities.GuiaRemisionDetalle> CreateGuiaDetalleAsync(DOMAIN.Entities.GuiaRemisionDetalle guiaDetalle);
        Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado, int idEstado);
        Task UpdateAsync(DOMAIN.Entities.GuiaRemision guiaRemision);
        Task DeleteAsync(int id);
    }
}
