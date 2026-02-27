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
                    .ThenInclude(de => de.ConductorGuids)
                        .ThenInclude(con=>con.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.TransporteGuids)
                        .ThenInclude(tr => tr.Transporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.CodigoUmNavigation)
                .FirstOrDefaultAsync(g => g.GuiaId == id);
            //?? throw new KeyNotFoundException("Guia de remisión no encontrada");
        }
        public async Task<int> CreateGuiaTransportistaTransactionalAsync(Remitente remitente, RemiteTransportistum remiteTransportista, Destinatario destinatario, List<Conductor> conductor, List<Transporte> transporte, DOMAIN.Entities.GuiaRemision guiaRemision, DatosEnvio datosEnvio, Origen origen, List<Destino> destino,
            List<GuiaDetalle> detalles,
            List<EmailDestinatarioGuid> emailDestinatarioGuia,
            List<EmailRemiteTransportistaGuid> emailRemiteTransportistas
            )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remitente
                var existingRemitente = await _repository.GetRemitenteByNameAsync(remitente.Descripcion);
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

                // Guía
                guiaRemision.RemitenteId = remitente.RemitenteId;
                guiaRemision.DestinatarioId = destinatario.DestinatarioId;
                guiaRemision.RemiteTransportistaId = remiteTransportista.RemiteTransportistaId;
                guiaRemision.Estado = "CREADO";

                //string serie = guiaRemision.Serie ?? "V001";
                string serie = "V" + remitente.Serie.Trim();

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
                guiaRemision.TipoTransporte = "PRIVADO"; //01 por defecto
                guiaRemision.TipoTraslado = "VENTA"; //01 por defecto


                guiaRemision.TipoGuia = "TRANSPORTISTA";
                guiaRemision.RespuestaTicket = "tc11446_no_enviado_TRANSPORTISTA";
                await _context.GuiaRemisions.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaId = guiaRemision.GuiaId;
                //datosEnvio.ConductorId = conductor.ConductorId;
                //datosEnvio.TransporteId = transporte.TransporteId;


                await _context.DatosEnvios.AddAsync(datosEnvio);
                await _context.SaveChangesAsync();

                // Conductor
                //Registrar en lista
                foreach (var condu in conductor)
                {
                    var existingConductor = await _repository.GetConductorByNumeroDocumentoAsync(condu.NumeroDocumento);
                    if (existingConductor == null)
                    {
                        condu.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                        condu.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                        await _context.Conductors.AddAsync(condu);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        condu.ConductorId = existingConductor.ConductorId;
                    }
                    var conductorGuia = new DOMAIN.Entities.ConductorGuid
                    {
                        ConductorId = condu.ConductorId,
                        DatosEnvioId = datosEnvio.DatosEnvioId
                    };
                    await _context.ConductorGuids.AddAsync(conductorGuia);
                    await _context.SaveChangesAsync();

                }

                // Transporte
                //Registrar en lista
                foreach (var tr in transporte)
                {
                    var existingTransporte = await _repository.GetTransporteByPlacaAsync(tr.Placa);
                    if (existingTransporte == null)
                    {
                        tr.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                        tr.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                        await _context.Transportes.AddAsync(tr);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        tr.TransporteId = existingTransporte.TransporteId;
                    }
                    var transporteGuia = new DOMAIN.Entities.TransporteGuid
                    {
                        TransporteId = tr.TransporteId,
                        DatosEnvioId = datosEnvio.DatosEnvioId
                    };
                    await _context.TransporteGuids.AddAsync(transporteGuia);
                    await _context.SaveChangesAsync();
                }
                // Email destinatario guía
                foreach (var emailDest in emailDestinatarioGuia)
                {
                    emailDest.GuiaId = guiaRemision.GuiaId;
                    emailDest.DestinatarioId = destinatario.DestinatarioId;
                    await _context.EmailDestinatarioGuids.AddAsync(emailDest);
                }
                await _context.SaveChangesAsync();

                //Email remite transportista
                foreach (var emailRemite in emailRemiteTransportistas)
                {
                    emailRemite.GuiaId = guiaRemision.GuiaId;
                    emailRemite.RemiteTransportistaId = remiteTransportista.RemiteTransportistaId;
                    await _context.EmailRemiteTransportistaGuids.AddAsync(emailRemite);
                }
                await _context.SaveChangesAsync();

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
