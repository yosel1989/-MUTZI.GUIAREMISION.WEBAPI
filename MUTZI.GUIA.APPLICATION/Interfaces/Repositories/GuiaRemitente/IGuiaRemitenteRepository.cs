
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente
{
    public interface IGuiaRemitenteRepository
    {
        Task<List<DOMAIN.Entities.GuiaRemision>> GetAllAsync();
        Task<(List<DOMAIN.Entities.GuiaRemision>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO? request);
        //Task<(List<DOMAIN.Entities.GuiaRemision>, int)> GetListAsync(int pageNumber, int pageSize, List<ColumnFilterDTO> columnFilterDTOs);
        Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync(int id);
        Task<DOMAIN.Entities.GuiaRemision> GetByTicketAsync(string ticket);
        Task <int> CreateGuiaRemisionTransactionalAsync(
            DOMAIN.Entities.Remitente remitente,
            DOMAIN.Entities.Destinatario destinatario,
            DOMAIN.Entities.Proveedor proveedor,
            List<DOMAIN.Entities.Conductor> conductor,
            List<DOMAIN.Entities.Transporte> transporte,
            DOMAIN.Entities.GuiaRemision guiaRemision,
            DOMAIN.Entities.DatosEnvio datosEnvio,
            DOMAIN.Entities.Origen origen,
            List<DOMAIN.Entities.Destino> destino,
            List<DOMAIN.Entities.GuiaDetalle> detalles,
            List<DOMAIN.Entities.DocRelacionado> docRelacionados,
            List<DOMAIN.Entities.EmailDestinatarioGuid> emailDestinatarioGuia
            );
        //Remitente
        Task<DOMAIN.Entities.Remitente> GetRemitenteByNameAsync(string name);
        Task<DOMAIN.Entities.Remitente> CreateRemitenteAsync(DOMAIN.Entities.Remitente remitente);
        //Destinatario
        Task<DOMAIN.Entities.Destinatario> GetDestinatarioByRucAsync(string ruc);
        Task<DOMAIN.Entities.Destinatario> CreateDestinatarioAsync(DOMAIN.Entities.Destinatario destinatario);
        Task<DOMAIN.Entities.Destinatario> UpdateDestinatarioAsync(DOMAIN.Entities.Destinatario destinatario);
        //Proveedor
        Task<DOMAIN.Entities.Proveedor> GetProveedorByRucAsync(string ruc);
        Task<DOMAIN.Entities.Proveedor> CreateProveedorAsync(DOMAIN.Entities.Proveedor proveedor);
        //Conductor
        Task<DOMAIN.Entities.Conductor> GetConductorByNumeroDocumentoAsync(string numeroDocumento);
        Task<DOMAIN.Entities.Conductor> CreateConductorAsync(DOMAIN.Entities.Conductor conductor);
        //Transporte
        Task<DOMAIN.Entities.Transporte> GetTransporteByPlacaAsync(string placa);
        Task<DOMAIN.Entities.Transporte> CreateTransporteAsync(DOMAIN.Entities.Transporte transporte);
        //Datos envio
        Task<DOMAIN.Entities.DatosEnvio> CreateDatosEnvioAsync(DOMAIN.Entities.DatosEnvio datosEnvio);
        //Ubicación origen
        Task<DOMAIN.Entities.Origen> CreateOrigenAsync(DOMAIN.Entities.Origen origen);
        //Ubicación destino
        Task<DOMAIN.Entities.Destino> CreateDestinoAsync(DOMAIN.Entities.Destino destino);
        //Guia detalle
        Task<DOMAIN.Entities.GuiaDetalle> CreateGuiaDetalleAsync(DOMAIN.Entities.GuiaDetalle guiaDetalle);
        Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado, int idEstado);
        Task UpdateAsync(DOMAIN.Entities.GuiaRemision guiaRemision);
        Task DeleteAsync(int id);
    }
}
