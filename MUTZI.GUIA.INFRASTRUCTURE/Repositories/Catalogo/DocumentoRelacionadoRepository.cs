using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class DocumentoRelacionadoRepository: IDocumentoRelacionadoRepository
    {
        private readonly LogisticaContext _context;
        public DocumentoRelacionadoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentoRelacionado>> GetAllAsync()
        {
            try
            {
                return await _context.DocumentoRelacionado
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
             
        }

        public async Task<IEnumerable<DocumentoRelacionado>> GetAllToSelectAsync(string? tipo)
        {
            try
            {
                var query = _context.DocumentoRelacionado
                    .Where(x => x.Activo)
                    .AsQueryable();

                switch (tipo)
                {
                    case "REMITENTE":
                        query = query.Where(x => x.Remitente);
                        break;
                    case "TRANSPORTISTA":
                        query = query.Where(x => x.Transportista);
                        break;
                    default: break;
                }

                return await query.Select(x => new DocumentoRelacionado
                    {
                        DocumentoRelacionadoId = x.DocumentoRelacionadoId,
                        Descripcion = x.Descripcion,
                        DescripcionCorta = x.DescripcionCorta,
                        CodigoSunat = x.CodigoSunat
                    })
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
