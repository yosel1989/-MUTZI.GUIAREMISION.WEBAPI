
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemitenteRepository : IGuiaRemitenteRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemitenteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<DOMAIN.Entities.GuiaRemision>> GetAllAsync()
        {
            return await _context.GuiaRemisions
                .Include(r => r.Remitente)
                    .ThenInclude(rem => rem.Ubigeo)
                .Include(d => d.Destinatario)
                    .ThenInclude(des => des.Ubigeo)
                .Include(p => p.Proveedor)
                    .ThenInclude(pro => pro.Ubigeo)
                .Include(dest => dest.Destinos)
                    .ThenInclude(de => de.Ubigeo)
                .Include(o => o.Origens)
                    .ThenInclude(or => or.Ubigeo)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.ConductorGuids)
                        .ThenInclude(con=>con.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.TransporteGuids)
                        .ThenInclude(tr=>tr.Transporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.CodigoUmNavigation)
                .Include(doc => doc.DocRelacionados)
                .ToListAsync();
        }
        public async Task<DOMAIN.Entities.GuiaRemision> GetByTicketAsync(string ticket)
        {
            return await _context.GuiaRemisions.Include(r => r.Remitente)
                .FirstOrDefaultAsync(g => g.RespuestaTicket == ticket);
        }
        public async Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync(int id)
        {
            return await _context.GuiaRemisions
                .Include(r => r.Remitente)
                    .ThenInclude(rem => rem.Ubigeo)
                .Include(d => d.Destinatario)
                    .ThenInclude(des => des.Ubigeo)
                .Include(p => p.Proveedor)
                    .ThenInclude(pro => pro.Ubigeo)
                .Include(dest => dest.Destinos)
                    .ThenInclude(de => de.Ubigeo)
                .Include(o => o.Origens)
                    .ThenInclude(or => or.Ubigeo)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.ConductorGuids)
                        .ThenInclude(con => con.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.TransporteGuids)
                        .ThenInclude(tr => tr.Transporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.CodigoUmNavigation)
                .Include(doc => doc.DocRelacionados)
                .FirstOrDefaultAsync(g => g.GuiaId == id);
                //?? throw new KeyNotFoundException("Guia de remisión no encontrada");
        }
        public async Task<int> CreateGuiaRemisionTransactionalAsync(
            DOMAIN.Entities.Remitente remitente,
            DOMAIN.Entities.Destinatario destinatario,
            DOMAIN.Entities.Proveedor proveedor,
            List<DOMAIN.Entities.Conductor> conductor,
            List<DOMAIN.Entities.Transporte> transporte,
            DOMAIN.Entities.GuiaRemision guiaRemision,
            DOMAIN.Entities.DatosEnvio datosEnvio,
            DOMAIN.Entities.Origen origen,
            //DOMAIN.Entities.Destino destino,
            List<DOMAIN.Entities.Destino> destino,
            List<DOMAIN.Entities.GuiaDetalle> detalles,
            List<DOMAIN.Entities.DocRelacionado> docRelacionados
            )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remitente
                var existingRemitente = await GetRemitenteByRucAsync(remitente.Ruc);
                if (existingRemitente == null)
                {
                    remitente.EmpleadoIdcreacion= guiaRemision.EmpleadoIdcreacion;
                    remitente.EmpleadoNombreCreacion=guiaRemision.EmpleadoNombreCreacion;
                    await _context.Remitentes.AddAsync(remitente);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    remitente = existingRemitente;
                }

                // Destinatario
                var existingDestinatario = await GetDestinatarioByRucAsync(destinatario.NumeroDocumento);
                if (existingDestinatario == null)
                {
                    destinatario.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                    destinatario.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                    await _context.Destinatarios.AddAsync(destinatario);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    destinatario = existingDestinatario;
                }

                // Proveedor
                if (guiaRemision.TipoTraslado == "COMPRA")
                {
                    var existingProveedor = await GetProveedorByRucAsync(proveedor.NumeroDocumento);
                    if (existingProveedor == null)
                    {
                        proveedor.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                        proveedor.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                        await _context.Proveedors.AddAsync(proveedor);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        proveedor = existingProveedor;
                    }
                }
                

                // Guía
                guiaRemision.RemitenteId = remitente.RemitenteId;
                guiaRemision.DestinatarioId = destinatario.DestinatarioId;
                guiaRemision.Estado = "CREADO";
                if (guiaRemision.TipoTraslado == "COMPRA") { guiaRemision.ProveedorId = proveedor.ProveedorId; }

                string serie = guiaRemision.Serie ?? "T001";

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


                guiaRemision.TipoGuia = "REMITENTE";
                guiaRemision.RespuestaTicket = "tc11446_no_enviado_REMITENTE";
                await _context.GuiaRemisions.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaId = guiaRemision.GuiaId;
                //if (guiaRemision.TipoTransporte == "PRIVADO")
                //{
                //    datosEnvio.ConductorId = conductor.ConductorId;
                //    datosEnvio.TransporteId = transporte.TransporteId;
                //}
                await _context.DatosEnvios.AddAsync(datosEnvio);
                await _context.SaveChangesAsync();

                // Conductor
                //Registrar en lista
                foreach (var condu in conductor)
                {
                    if (guiaRemision.TipoTransporte == "PRIVADO")
                    {
                        var existingConductor = await GetConductorByLicenciaAsync(condu.Licencia);
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
                }

                // Transporte
                //Registrar en lista
                foreach (var tr in transporte)
                {
                    if (guiaRemision.TipoTransporte == "PRIVADO")
                    {
                        var existingTransporte = await GetTransporteByPlacaAsync(tr.Placa);
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
                }

                // DocRelacionados
                foreach (var doc in docRelacionados)
                {
                    doc.GuiaId = guiaRemision.GuiaId;
                    await _context.DocRelacionados.AddAsync(doc);
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
        public async Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado)
        {
            var guia = await _context.GuiaRemisions.FindAsync(guiaId);
            if (guia == null)
                throw new KeyNotFoundException("Guía no encontrada");

            guia.RespuestaTicket = ticket;
            guia.Estado = estado;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.GuiaRemision guiaRemision)
        {
            if (guiaRemision == null)
            {
                throw new ArgumentNullException(nameof(guiaRemision), "El objeto guiaRemision no puede ser nulo");
            }
            _context.GuiaRemisions.Update(guiaRemision);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var guiaRemision = await _context.GuiaRemisions.FindAsync(id);
            if (guiaRemision == null)
            {
                throw new KeyNotFoundException("Guia de remisión no encontrada");
            }
            _context.GuiaRemisions.Remove(guiaRemision);
            await _context.SaveChangesAsync();
        }
        //Remitente
        public async Task<DOMAIN.Entities.Remitente> GetRemitenteByRucAsync(string ruc)
        {
            return await _context.Remitentes.FirstOrDefaultAsync(r => r.Ruc == ruc);
        }
        public async Task<DOMAIN.Entities.Remitente> CreateRemitenteAsync(DOMAIN.Entities.Remitente remitente)
        {
            if (remitente == null)
            {
                throw new ArgumentNullException(nameof(remitente), "El objeto remitente no puede ser nulo");
            }
            await _context.Remitentes.AddAsync(remitente);
            await _context.SaveChangesAsync();
            return remitente;
        }
        //Destinatario
        public async Task<DOMAIN.Entities.Destinatario> GetDestinatarioByRucAsync(string ruc)
        {
            return await _context.Destinatarios.FirstOrDefaultAsync(d => d.NumeroDocumento == ruc);
        }
        public async Task<DOMAIN.Entities.Destinatario> CreateDestinatarioAsync(DOMAIN.Entities.Destinatario destinatario)
        {
            if (destinatario == null)
            {
                throw new ArgumentNullException(nameof(destinatario), "El objeto destinatario no puede ser nulo");
            }
            await _context.Destinatarios.AddAsync(destinatario);
            await _context.SaveChangesAsync();
            return destinatario;
        }
        //Proveedor
        public async Task<DOMAIN.Entities.Proveedor> GetProveedorByRucAsync(string ruc)
        {
            return await _context.Proveedors.FirstOrDefaultAsync(p => p.NumeroDocumento == ruc);
        }
        public async Task<DOMAIN.Entities.Proveedor> CreateProveedorAsync(DOMAIN.Entities.Proveedor proveedor)
        {
            if (proveedor == null)
            {
                throw new ArgumentNullException(nameof(proveedor), "El objeto proveedor no puede ser nulo");
            }
            await _context.Proveedors.AddAsync(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }
        //Conductor
        public async Task<DOMAIN.Entities.Conductor> GetConductorByLicenciaAsync(string licencia)
        {
            return await _context.Conductors.FirstOrDefaultAsync(c => c.Licencia == licencia);
        }
        public async Task<DOMAIN.Entities.Conductor> CreateConductorAsync(DOMAIN.Entities.Conductor conductor)
        {
            if (conductor == null)
            {
                throw new ArgumentNullException(nameof(conductor), "El objeto conductor no puede ser nulo");
            }
            await _context.Conductors.AddAsync(conductor);
            await _context.SaveChangesAsync();
            return conductor;
        }
        //Transporte
        public async Task<DOMAIN.Entities.Transporte> GetTransporteByPlacaAsync(string placa)
        {
            return await _context.Transportes.FirstOrDefaultAsync(t => t.Placa == placa);
        }
        public async Task<DOMAIN.Entities.Transporte> CreateTransporteAsync(DOMAIN.Entities.Transporte transporte)
        {
            if (transporte == null)
            {
                throw new ArgumentException(nameof(transporte), "El objeto transporte no puede ser nulo");
            }
            await _context.Transportes.AddAsync(transporte);
            await _context.SaveChangesAsync();
            return transporte;
        }
        //Datos envio
        public async Task<DOMAIN.Entities.DatosEnvio> CreateDatosEnvioAsync(DOMAIN.Entities.DatosEnvio datosEnvio)
        {
            if (datosEnvio == null)
            {
                throw new ArgumentNullException(nameof(datosEnvio), "El objeto datosEnvio no puede ser nulo");
            }
            await _context.DatosEnvios.AddAsync(datosEnvio);
            await _context.SaveChangesAsync();
            return datosEnvio;
        }
        //Guia detalle
        public async Task<DOMAIN.Entities.GuiaDetalle> CreateGuiaDetalleAsync(DOMAIN.Entities.GuiaDetalle guiaDetalle)
        {
            if (guiaDetalle == null)
            {
                throw new ArgumentNullException(nameof(guiaDetalle), "El objeto guiaDetalle no puede ser nulo");
            }
            await _context.GuiaDetalles.AddAsync(guiaDetalle);
            await _context.SaveChangesAsync();
            return guiaDetalle;
        }

        public async Task<DOMAIN.Entities.Origen> CreateOrigenAsync(DOMAIN.Entities.Origen origen)
        {
            if (origen == null)
            {
                throw new ArgumentNullException(nameof(origen), "El objeto origen no puede ser nulo");
            }
            await _context.Origens.AddAsync(origen);
            await _context.SaveChangesAsync();
            return origen;
        }

        public async Task<DOMAIN.Entities.Destino> CreateDestinoAsync(DOMAIN.Entities.Destino destino)
        {
            if (destino == null)
            {
                throw new ArgumentNullException(nameof(destino), "El objeto destino no puede ser nulo");
            }
            await _context.Destinos.AddAsync(destino);
            await _context.SaveChangesAsync();
            return destino;
        }

    }
}
