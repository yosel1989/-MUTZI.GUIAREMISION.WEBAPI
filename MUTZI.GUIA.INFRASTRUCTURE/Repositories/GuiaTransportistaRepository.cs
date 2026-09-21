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
            return await _context.GuiaRemision
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
                    .ThenInclude(de => de.GuiaRemisionConductores)
                        .ThenInclude(con=>con.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.GuiaRemisionUnidadesTransporte)
                        .ThenInclude(tr => tr.UnidadTransporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.UnidadMedida)
                .Include(doc => doc.DocRelacionados)
                .Include(e=>e.EmailRemiteTransportistaGuids)
                .Include(e => e.EmailDestinatarioGuids)
                .Include(e => e.Estado)
                .FirstOrDefaultAsync(g => g.GuiaRemisionId == id);
            //?? throw new KeyNotFoundException("GuiaRemision de remisión no encontrada");
        }
        public async Task<(int GuiaId, string NumeroGuia)> CreateGuiaTransportistaTransactionalAsync(
            EntityBranch remitente,
            EntityBranchSerie remitenteSerie,
            Entity remiteTransportista,
            EntityBranch destinatario,
            List<GuiaRemisionConductor> conductor,
            List<GuiaRemisionUnidadTransporte> transporte,
            GuiaRemision guiaRemision,
            GuiaRemisionDatoEnvio datosEnvio,
            GuiaRemisionOrigen origen,
            List<GuiaRemisionDestino> destino,
            List<GuiaRemisionDetalle> detalles,
            List<GuiaRemisionDocumentoRelacionado> docRelacionados,
            List<GuiaRemisionEmailDestinatario> emailDestinatarioGuia,
            List<EmailRemiteTransportistaGuid> emailRemiteTransportistas
            )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                // Guía
                guiaRemision.RemitenteId = remitente.EntityBranchId;
                guiaRemision.Ruc = remitente.Entity.DocumentNumber;
                guiaRemision.DestinatarioId = destinatario.EntityBranchId;
                guiaRemision.RemiteTransportistaId = remiteTransportista.EntityId;
                //guiaRemision.Nombre = "REGISTRADO";
                guiaRemision.EstadoId = 0;

                //string serie = guiaRemision.Serie ?? "V001";
                string serie = "V" + remitenteSerie.Serie.Trim();

                //Buscar el último correlativo para esa serie
                var lastGuia = await _context.GuiaRemision
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
                //guiaRemision.MotivoTrasladoId = 1; //01 por defecto


                guiaRemision.TipoGuia = "TRANSPORTISTA";
                guiaRemision.RespuestaTicket = "REGISTRADO";
                await _context.GuiaRemision.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaRemisionId = guiaRemision.GuiaRemisionId;

                await _context.DatosEnvios.AddAsync(datosEnvio);
                await _context.SaveChangesAsync();

                // ConductorGUID
                //Registrar en lista
                foreach (var condu in conductor)
                {
                    var conductorGuia = new DOMAIN.Entities.GuiaRemisionConductor
                    {
                        ConductorId = condu.ConductorId,
                        GuiaRemisionDatoEnvioId = datosEnvio.GuiaRemisionDatoEnvioId
                    };
                    await _context.ConductorGuids.AddAsync(conductorGuia);
                    await _context.SaveChangesAsync();

                }

                // TransporteGUID
                //Registrar en lista
                foreach (var tr in transporte)
                {
                    var transporteGuia = new DOMAIN.Entities.GuiaRemisionUnidadTransporte
                    {
                        UnidadTransporteId = tr.UnidadTransporteId,
                        GuiaRemisionDatoEnvioId = datosEnvio.GuiaRemisionDatoEnvioId
                    };
                    await _context.UnidadTransporteGuids.AddAsync(transporteGuia);
                    await _context.SaveChangesAsync();
                }
                // DocRelacionados
                if (docRelacionados is not null)
                {
                    foreach (var doc in docRelacionados)
                    {
                        doc.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                        await _context.DocRelacionados.AddAsync(doc);
                    }
                    await _context.SaveChangesAsync();
                }
                // Email destinatario guía
                if (emailDestinatarioGuia is not null)
                {
                    foreach (var emailDest in emailDestinatarioGuia)
                    {
                        emailDest.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                        emailDest.EstablecimientoId = destinatario.EntityBranchId;
                        await _context.EmailDestinatarioGuids.AddAsync(emailDest);
                    }
                    await _context.SaveChangesAsync();
                }

                //Email remite transportista
                if (emailRemiteTransportistas is not null)
                {
                    foreach (var emailRemite in emailRemiteTransportistas)
                    {
                        emailRemite.GuiaId = guiaRemision.GuiaRemisionId;
                        emailRemite.RemiteTransportistaId = remiteTransportista.EntityId;
                        await _context.EmailRemiteTransportistaGuids.AddAsync(emailRemite);
                    }
                    await _context.SaveChangesAsync();
                }

                //Origen
                origen.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                await _context.GuiaRemisionOrigen.AddAsync(origen);
                await _context.SaveChangesAsync();

                foreach (var dest in destino)
                {
                    dest.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                    await _context.GuiaRemisionDestino.AddAsync(dest);
                }
                await _context.SaveChangesAsync();

                // Detalles
                foreach (var detalle in detalles)
                {
                    detalle.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                    await _context.GuiaRemisionDetalle.AddAsync(detalle);
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                //retornar el ID de la guía creada y asimismo el número de guía generado
                return (guiaRemision.GuiaRemisionId, guiaRemision.NumeroGuia);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<Transportista> GetRemiteTransportistaByRucAsync(string numeroDocumento)
        {
            return await _context.Transportista.FirstOrDefaultAsync(r => r.NumeroDocumento == numeroDocumento);
        }
        //Actualizar guía de remisión transportista
        public async Task UpdateAsync(GuiaRemision guiaRemision)
        {
            if (guiaRemision == null)
            {
                throw new ArgumentNullException(nameof(guiaRemision), "La guía de remisión transportista no puede ser nula");
            }
            _context.GuiaRemision.Update(guiaRemision);
            await _context.SaveChangesAsync();
        }
    }
}
