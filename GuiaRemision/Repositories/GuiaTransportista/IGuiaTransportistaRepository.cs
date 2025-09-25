namespace GuiaRemision.Repositories.GuiaTransportista
{
    public interface IGuiaTransportistaRepository
    {
        Task<Models.DBMS.GuiaRemision> GetByIdAsync(int id);
        Task<int> CreateGuiaTransportistaTransactionalAsync(
            Models.DBMS.Remitente remitente,
            Models.DBMS.RemiteTransportista remiteTransportista,
            Models.DBMS.Destinatario destinatario,
            Models.DBMS.Conductor conductor,
            Models.DBMS.Transporte transporte,
            Models.DBMS.GuiaRemision guiaRemision,
            Models.DBMS.DatosEnvio datosEnvio,
            Models.DBMS.Origen origen,
            List<Models.DBMS.Destino> destino,
            List<Models.DBMS.GuiaDetalle> detalles
            );
        //Remitente
        Task<Models.DBMS.RemiteTransportista> GetRemiteTransportistaByRucAsync(string ruc);
    }
}
