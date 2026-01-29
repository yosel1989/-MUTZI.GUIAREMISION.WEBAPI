using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaTransportistaRepository : IGuiaTransportistaRepository
    {
        private readonly LogisticaContext _context;
        private readonly IGuiaRemitenteRepository _repository;
        public GuiaTransportistaRepository(LogisticaContext context, IGuiaRemitenteRepository repository)
        {
            _context = context;
            _repository = repository;
        }
        public async Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync(int id)
        {
            return await _context.GuiaRemisions
                .Include(r => r.Remitente)
                    .ThenInclude(rem => rem.Ubigeo)
                .Include(rt => rt.RemiteTransportista)
                    .ThenInclude(re => re.Ubigeo)
                .Include(d => d.Destinatario)
                    .ThenInclude(des => des.Ubigeo)
                .Include(dest => dest.Destinos)
                    .ThenInclude(de => de.Ubigeo)
                .Include(o => o.Origens)
                    .ThenInclude(or => or.Ubigeo)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.Transporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.CodigoUmNavigation)
                .FirstOrDefaultAsync(g => g.GuiaId == id);
                //?? throw new KeyNotFoundException("Guia de remisión no encontrada");
        }
        public async Task<int> CreateGuiaTransportistaTransactionalAsync(Remitente remitente,RemiteTransportistum remiteTransportista, Destinatario destinatario, Conductor conductor, Transporte transporte, DOMAIN.Entities.GuiaRemision guiaRemision, DatosEnvio datosEnvio, Origen origen, List<Destino> destino, List<GuiaDetalle> detalles)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remitente
                var existingRemitente = await _repository.GetRemitenteByRucAsync(remitente.Ruc);
                if (existingRemitente == null)
                {
                    await _context.Remitentes.AddAsync(remitente);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    remitente = existingRemitente;
                }
                //Agregar datos de remitente transportista
                var existingRemiteTransportista = await GetRemiteTransportistaByRucAsync(remiteTransportista.Ruc);
                if (existingRemiteTransportista == null)
                {
                    await _context.RemiteTransportista.AddAsync(remiteTransportista);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    remiteTransportista = existingRemiteTransportista;
                }

                // Destinatario
                var existingDestinatario = await _repository.GetDestinatarioByRucAsync(destinatario.NumeroDocumento);
                if (existingDestinatario == null)
                {
                    await _context.Destinatarios.AddAsync(destinatario);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    destinatario = existingDestinatario;
                }

                // Conductor
                var existingConductor = await _repository.GetConductorByLicenciaAsync(conductor.Licencia);
                if (existingConductor == null)
                {
                    await _context.Conductors.AddAsync(conductor);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    conductor = existingConductor;
                }

                // Transporte
                var existingTransporte = await _repository.GetTransporteByPlacaAsync(transporte.Placa);
                if (existingTransporte == null)
                {
                    await _context.Transportes.AddAsync(transporte);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    transporte = existingTransporte;
                }

                // Guía
                guiaRemision.RemitenteId = remitente.RemitenteId;
                guiaRemision.DestinatarioId = destinatario.DestinatarioId;
                guiaRemision.RemiteTransportistaId = remiteTransportista.RemiteTransportistaId;
                guiaRemision.Estado = "SEND";

                string serie = guiaRemision.Serie ?? "V001";

                //Buscar el último correlativo para esa serie
                var lastGuia = await _context.GuiaRemisions
                    .Where(g => g.Serie == serie)
                    .OrderByDescending(g => g.Correlativo)
                    .FirstOrDefaultAsync();

                int nextCorrelativo = 1;
                if (lastGuia != null && int.TryParse(lastGuia.Correlativo, out int lastCorrelativo))
                {
                    nextCorrelativo = lastCorrelativo + 1;
                }

                //Formatear el correlativo a 8 dígitos
                string correlativoStr = nextCorrelativo.ToString("D8");

                //Guardar los campos
                guiaRemision.Correlativo = correlativoStr;
                guiaRemision.Serie = serie;
                guiaRemision.NumeroGuia = $"{serie}-{correlativoStr}";
                guiaRemision.TipoTransporte= "PRIVADO"; //01 por defecto
                guiaRemision.TipoTraslado= "VENTA"; //01 por defecto


                guiaRemision.TipoGuia = "TRANSPORTISTA";
                guiaRemision.RespuestaTicket = "tc0211446_prueba_TRANSPORTISTA";
                await _context.GuiaRemisions.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaId = guiaRemision.GuiaId;
                datosEnvio.ConductorId = conductor.ConductorId;
                datosEnvio.TransporteId = transporte.TransporteId;
                datosEnvio.MotivoEnvio = "01"; //01 por defecto
                
                
                await _context.DatosEnvios.AddAsync(datosEnvio);
                await _context.SaveChangesAsync();

                //// Origen/Destino
                //origen.GuiaId = guiaRemision.GuiaId;
                //destino.GuiaId = guiaRemision.GuiaId;
                //await _context.Origens.AddAsync(origen);
                //await _context.Destinos.AddAsync(destino);
                //await _context.SaveChangesAsync();

                //Origen
                origen.GuiaId = guiaRemision.GuiaId;
                await _context.Origens.AddAsync(origen);
                await _context.SaveChangesAsync();

                foreach (var dest in destino)
                {
                    dest.GuiaId = guiaRemision.GuiaId;
                    await _context.Destinos.AddAsync(dest);
                }
                await _context.SaveChangesAsync();

                // Detalles
                foreach (var detalle in detalles)
                {
                    detalle.GuiaId = guiaRemision.GuiaId;
                    await _context.GuiaDetalles.AddAsync(detalle);
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                //retornar el ID de la guía creada
                return guiaRemision.GuiaId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<RemiteTransportistum> GetRemiteTransportistaByRucAsync(string ruc)
        {
            return await _context.RemiteTransportista.FirstOrDefaultAsync(r => r.Ruc == ruc);
        }
    }
}
