using GuiaRemision.Models.DBMS;

namespace GuiaRemision.Repositories.GuiaRemitente
{
    public interface IGuiaRemitenteRepository
    {
        Task<List<Models.DBMS.GuiaRemision>> GetAllAsync();
        Task<Models.DBMS.GuiaRemision> GetByIdAsync(int id);
        Task<Models.DBMS.GuiaRemision> GetByTicketAsync(string ticket);
        Task <int> CreateGuiaRemisionTransactionalAsync(
            Models.DBMS.Remitente remitente,
            Models.DBMS.Destinatario destinatario,
            Models.DBMS.Proveedor proveedor,
            Models.DBMS.Conductor conductor,
            Models.DBMS.Transporte transporte,
            Models.DBMS.GuiaRemision guiaRemision,
            Models.DBMS.DatosEnvio datosEnvio,
            Models.DBMS.Origen origen,
            List<Models.DBMS.Destino> destino,
            List<Models.DBMS.GuiaDetalle> detalles
            );
        //Remitente
        Task<Models.DBMS.Remitente> GetRemitenteByRucAsync(string ruc);
        Task<Models.DBMS.Remitente> CreateRemitenteAsync(Models.DBMS.Remitente remitente);
        //Destinatario
        Task<Models.DBMS.Destinatario> GetDestinatarioByRucAsync(string ruc);
        Task<Models.DBMS.Destinatario> CreateDestinatarioAsync(Models.DBMS.Destinatario destinatario);
        //Proveedor
        Task<Models.DBMS.Proveedor> GetProveedorByRucAsync(string ruc);
        Task<Models.DBMS.Proveedor> CreateProveedorAsync(Models.DBMS.Proveedor proveedor);
        //Conductor
        Task<Models.DBMS.Conductor> GetConductorByLicenciaAsync(string licencia);
        Task<Models.DBMS.Conductor> CreateConductorAsync(Models.DBMS.Conductor conductor);
        //Transporte
        Task<Models.DBMS.Transporte> GetTransporteByPlacaAsync(string placa);
        Task<Models.DBMS.Transporte> CreateTransporteAsync(Models.DBMS.Transporte transporte);
        //Datos envio
        Task<Models.DBMS.DatosEnvio> CreateDatosEnvioAsync(Models.DBMS.DatosEnvio datosEnvio);
        //Ubicación origen
        Task<Models.DBMS.Origen> CreateOrigenAsync(Models.DBMS.Origen origen);
        //Ubicación destino
        Task<Models.DBMS.Destino> CreateDestinoAsync(Models.DBMS.Destino destino);
        //Guia detalle
        Task<Models.DBMS.GuiaDetalle> CreateGuiaDetalleAsync(Models.DBMS.GuiaDetalle guiaDetalle);
        Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado);
        Task UpdateAsync(Models.DBMS.GuiaRemision guiaRemision);
        Task DeleteAsync(int id);
    }
}
