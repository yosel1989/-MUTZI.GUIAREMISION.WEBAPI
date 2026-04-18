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
        public async Task<GuiaRemision> GetByIdAsync(int id)
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
                .Include(doc => doc.DocRelacionados)
                .Include(e=>e.EmailRemiteTransportistaGuids)
                .Include(e => e.EmailDestinatarioGuids)
                .FirstOrDefaultAsync(g => g.GuiaId == id);
            //?? throw new KeyNotFoundException("Guia de remisión no encontrada");
        }
        public async Task<(int GuiaId, string NumeroGuia)> CreateGuiaTransportistaTransactionalAsync(
            Remitente remitente,
            RemiteTransportistum remiteTransportista,
            Destinatario destinatario,
            List<ConductorGuid> conductor,
            List<UnidadTransporteGuid> transporte,
            GuiaRemision guiaRemision,
            DatosEnvio datosEnvio,
            Origen origen,
            List<Destino> destino,
            List<GuiaDetalle> detalles,
            List<DocRelacionado> docRelacionados,
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
                    throw new Exception("El remitente no existe. Por favor, registre el remitente antes de crear la guía de remisión.");
                }
                else
                {
                    remitente = existingRemitente;
                }

                // Guía
                guiaRemision.RemitenteId = remitente.RemitenteId;
                guiaRemision.Ruc = remitente.Ruc;
                guiaRemision.DestinatarioId = destinatario.DestinatarioId;
                guiaRemision.RemiteTransportistaId = remiteTransportista.RemiteTransportistaId;
                guiaRemision.Estado = "REGISTRADO";
                guiaRemision.IdEstado = 0;

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
                guiaRemision.RespuestaTicket = "REGISTRADO";
                await _context.GuiaRemisions.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaId = guiaRemision.GuiaId;

                await _context.DatosEnvios.AddAsync(datosEnvio);
                await _context.SaveChangesAsync();

                // ConductorGUID
                //Registrar en lista
                foreach (var condu in conductor)
                {
                    var conductorGuia = new DOMAIN.Entities.ConductorGuid
                    {
                        ConductorId = condu.ConductorId,
                        DatosEnvioId = datosEnvio.DatosEnvioId
                    };
                    await _context.ConductorGuids.AddAsync(conductorGuia);
                    await _context.SaveChangesAsync();

                }

                // TransporteGUID
                //Registrar en lista
                foreach (var tr in transporte)
                {
                    var transporteGuia = new DOMAIN.Entities.UnidadTransporteGuid
                    {
                        TransporteId = tr.TransporteId,
                        DatosEnvioId = datosEnvio.DatosEnvioId
                    };
                    await _context.UnidadTransporteGuids.AddAsync(transporteGuia);
                    await _context.SaveChangesAsync();
                }
                // DocRelacionados
                if (docRelacionados is not null)
                {
                    foreach (var doc in docRelacionados)
                    {
                        doc.GuiaId = guiaRemision.GuiaId;
                        await _context.DocRelacionados.AddAsync(doc);
                    }
                    await _context.SaveChangesAsync();
                }
                // Email destinatario guía
                if (emailDestinatarioGuia is not null)
                {
                    foreach (var emailDest in emailDestinatarioGuia)
                    {
                        emailDest.GuiaId = guiaRemision.GuiaId;
                        emailDest.DestinatarioId = destinatario.DestinatarioId;
                        await _context.EmailDestinatarioGuids.AddAsync(emailDest);
                    }
                    await _context.SaveChangesAsync();
                }

                //Email remite transportista
                if (emailRemiteTransportistas is not null)
                {
                    foreach (var emailRemite in emailRemiteTransportistas)
                    {
                        emailRemite.GuiaId = guiaRemision.GuiaId;
                        emailRemite.RemiteTransportistaId = remiteTransportista.RemiteTransportistaId;
                        await _context.EmailRemiteTransportistaGuids.AddAsync(emailRemite);
                    }
                    await _context.SaveChangesAsync();
                }

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

                //retornar el ID de la guía creada y asimismo el número de guía generado
                return (guiaRemision.GuiaId, guiaRemision.NumeroGuia);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<RemiteTransportistum> GetRemiteTransportistaByRucAsync(string numeroDocumento)
        {
            return await _context.RemiteTransportista.FirstOrDefaultAsync(r => r.NumeroDocumento == numeroDocumento);
        }
        //Actualizar guía de remisión transportista
        public async Task UpdateAsync(GuiaRemision guiaRemision)
        {
            if (guiaRemision == null)
            {
                throw new ArgumentNullException(nameof(guiaRemision), "La guía de remisión transportista no puede ser nula");
            }
            _context.GuiaRemisions.Update(guiaRemision);
            await _context.SaveChangesAsync();
        }
    }
}
