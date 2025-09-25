
using GuiaRemision.Models.DBMS;
using Microsoft.EntityFrameworkCore;

namespace GuiaRemision.Repositories.GuiaRemitente
{
    public class GuiaRemitenteRepository : IGuiaRemitenteRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemitenteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<Models.DBMS.GuiaRemision>> GetAllAsync()
        {
            return await _context.GuiaRemisions.ToListAsync();
        }
        public async Task<Models.DBMS.GuiaRemision> GetByTicketAsync(string ticket)
        {
            return await _context.GuiaRemisions.Include(r => r.Remitente)
                .FirstOrDefaultAsync(g => g.RespuestaTicket == ticket)
                ?? throw new KeyNotFoundException("Número de guía y/o RUC no encontrado en la BBDD");
        }
        public async Task<Models.DBMS.GuiaRemision> GetByIdAsync(int id)
        {
            return await _context.GuiaRemisions
                .Include(r=>r.Remitente)
                    .ThenInclude(rem=>rem.Ubigeo)
                .Include(d=>d.Destinatario)
                    .ThenInclude(des=>des.Ubigeo)
                .Include(p=>p.Proveedor)
                    .ThenInclude(pro=>pro.Ubigeo)
                .Include(dest=>dest.Destinos)
                    .ThenInclude(de => de.Ubigeo)
                .Include(o=>o.Origens)
                    .ThenInclude(or => or.Ubigeo)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.Conductor)
                .Include(env=>env.DatosEnvios)
                    .ThenInclude(de => de.Transporte)
                .Include(det=>det.GuiaDetalles)
                    .ThenInclude(gd => gd.UnidadMedida)
                .FirstOrDefaultAsync(g => g.GuiaId == id)
                ?? throw new KeyNotFoundException("Guia de remisión no encontrada");
        }
        public async Task<int> CreateGuiaRemisionTransactionalAsync(
            Models.DBMS.Remitente remitente,
            Models.DBMS.Destinatario destinatario,
            Models.DBMS.Proveedor proveedor,
            Models.DBMS.Conductor conductor,
            Models.DBMS.Transporte transporte,
            Models.DBMS.GuiaRemision guiaRemision,
            Models.DBMS.DatosEnvio datosEnvio,
            Models.DBMS.Origen origen,
            //Models.DBMS.Destino destino,
            List<Models.DBMS.Destino> destino,
            List<Models.DBMS.GuiaDetalle> detalles)
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
                // Conductor
                if (guiaRemision.TipoTransporte == "PRIVADO")
                {
                    var existingConductor = await GetConductorByLicenciaAsync(conductor.Licencia);
                    if (existingConductor == null)
                    {
                        conductor.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                        conductor.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                        await _context.Conductors.AddAsync(conductor);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        conductor = existingConductor;
                    }
                }

                // Transporte
                if (guiaRemision.TipoTransporte == "PRIVADO")
                {
                    var existingTransporte = await GetTransporteByPlacaAsync(transporte.Placa);
                    if (existingTransporte == null)
                    {
                        transporte.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                        transporte.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                        await _context.Transportes.AddAsync(transporte);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        transporte = existingTransporte;
                    }
                }

                // Guía
                guiaRemision.RemitenteId = remitente.RemitenteId;
                guiaRemision.DestinatarioId = destinatario.DestinatarioId;
                guiaRemision.Estado = "SEND";
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
                guiaRemision.RespuestaTicket = "tc0211446_prueba";
                await _context.GuiaRemisions.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaId = guiaRemision.GuiaId;
                if (guiaRemision.TipoTransporte == "PRIVADO")
                {
                    datosEnvio.ConductorId = conductor.ConductorId;
                    datosEnvio.TransporteId = transporte.TransporteId;
                }
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
        public async Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado)
        {
            var guia = await _context.GuiaRemisions.FindAsync(guiaId);
            if (guia == null)
                throw new KeyNotFoundException("Guía no encontrada");

            guia.RespuestaTicket = ticket;
            guia.Estado = estado;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Models.DBMS.GuiaRemision guiaRemision)
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
        public async Task<Models.DBMS.Remitente> GetRemitenteByRucAsync(string ruc)
        {
            return await _context.Remitentes.FirstOrDefaultAsync(r => r.Ruc == ruc);
        }
        public async Task<Models.DBMS.Remitente> CreateRemitenteAsync(Models.DBMS.Remitente remitente)
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
        public async Task<Models.DBMS.Destinatario> GetDestinatarioByRucAsync(string ruc)
        {
            return await _context.Destinatarios.FirstOrDefaultAsync(d => d.NumeroDocumento == ruc);
        }
        public async Task<Models.DBMS.Destinatario> CreateDestinatarioAsync(Models.DBMS.Destinatario destinatario)
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
        public async Task<Models.DBMS.Proveedor> GetProveedorByRucAsync(string ruc)
        {
            return await _context.Proveedors.FirstOrDefaultAsync(p => p.NumeroDocumento == ruc);
        }
        public async Task<Models.DBMS.Proveedor> CreateProveedorAsync(Models.DBMS.Proveedor proveedor)
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
        public async Task<Models.DBMS.Conductor> GetConductorByLicenciaAsync(string licencia)
        {
            return await _context.Conductors.FirstOrDefaultAsync(c => c.Licencia == licencia);
        }
        public async Task<Models.DBMS.Conductor> CreateConductorAsync(Models.DBMS.Conductor conductor)
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
        public async Task<Models.DBMS.Transporte> GetTransporteByPlacaAsync(string placa)
        {
            return await _context.Transportes.FirstOrDefaultAsync(t => t.Placa == placa);
        }
        public async Task<Transporte> CreateTransporteAsync(Transporte transporte)
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
        public async Task<Models.DBMS.DatosEnvio> CreateDatosEnvioAsync(Models.DBMS.DatosEnvio datosEnvio)
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
        public async Task<Models.DBMS.GuiaDetalle> CreateGuiaDetalleAsync(Models.DBMS.GuiaDetalle guiaDetalle)
        {
            if (guiaDetalle == null)
            {
                throw new ArgumentNullException(nameof(guiaDetalle), "El objeto guiaDetalle no puede ser nulo");
            }
            await _context.GuiaDetalles.AddAsync(guiaDetalle);
            await _context.SaveChangesAsync();
            return guiaDetalle;
        }

        public async Task<Models.DBMS.Origen> CreateOrigenAsync(Models.DBMS.Origen origen)
        {
            if (origen == null)
            {
                throw new ArgumentNullException(nameof(origen), "El objeto origen no puede ser nulo");
            }
            await _context.Origens.AddAsync(origen);
            await _context.SaveChangesAsync();
            return origen;
        }

        public async Task<Models.DBMS.Destino> CreateDestinoAsync(Models.DBMS.Destino destino)
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
