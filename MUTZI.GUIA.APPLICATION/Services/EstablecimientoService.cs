using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class EstablecimientoService: IEstablecimientoService
    {
        private readonly IEstablecimientoRepository _establecimientoRepository;
        private readonly IMapper _mapper;
        public EstablecimientoService(IEstablecimientoRepository establecimientoRepository, IMapper mapper)
        {
            _establecimientoRepository = establecimientoRepository;
            _mapper = mapper;
        }
        public async Task<List<EstablecimientoListDTO>> GetAllByRucToSelectAsync(string ruc)
        {
            var data = await _establecimientoRepository.GetAllByRucToSelectAsync(ruc);
            return _mapper.Map<List<EstablecimientoListDTO>>(data);
        }

        public async Task<List<EstablecimientoListToModalDTO>> GetListBySuggestionAsync(string ruc, string? search)
        {
            var data = await _establecimientoRepository.GetListBySuggestion(ruc, search);
            return _mapper.Map<List<EstablecimientoListToModalDTO>>(data);
        }

        public async Task<EstablecimientoDTO?> GetByIdAsync(int id)
        {
            var data = await _establecimientoRepository.GetByIdAsync(id);
            return _mapper.Map<EstablecimientoDTO>(data);
        }
    }
}
