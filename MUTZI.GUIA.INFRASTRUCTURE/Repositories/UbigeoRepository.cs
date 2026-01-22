
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.Ubigeo;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.Ubigeo;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class UbigeoRepository : IUbigeoRepository
    {
        private readonly LogisticaContext _context;
        private readonly IMapper _mapper;
        public UbigeoRepository(LogisticaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UbigeoDistritoDto>> GetDistritoAsync(string ubigeoProvincia)
        {
            return await _context.Ubigeos
                .Where(u => u.UbigeoId.StartsWith(ubigeoProvincia))
                .GroupBy(u => new { Code = u.UbigeoId.Substring(0, 6), u.Distrito })
                .Select(g => new UbigeoDistritoDto
                {
                    Id = g.Key.Code,
                    Distrito = g.Key.Distrito!
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<UbigeoProvinciaDto>> GetProvinciaAsync(string ubigeoDepartamento)
        {
            return await _context.Ubigeos
                .Where(u => u.UbigeoId.StartsWith(ubigeoDepartamento))
                .GroupBy(u => new { Code = u.UbigeoId.Substring(0, 4), u.Provincia })
                .Select(g => new UbigeoProvinciaDto
                {
                    Id = g.Key.Code,
                    Provincia = g.Key.Provincia!
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<UbigeoDepartamentoDto>> GetDepartamentoAsync()
        {
            return await _context.Ubigeos
                .GroupBy(u => new { Id = u.UbigeoId.Substring(0, 2), u.Departamento })
                .Select(g => new UbigeoDepartamentoDto
                {
                    Id = g.Key.Id,
                    Departamento = g.Key.Departamento!
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UbigeoDto> FindByIdAsync(string ubigeo)
        {
            var entity = await _context.Ubigeos.FindAsync(ubigeo); 
            if (entity == null) throw new Exception("No se encontró el ubigeo."); 
            return _mapper.Map<UbigeoDto>(entity);
        }
    }
}
