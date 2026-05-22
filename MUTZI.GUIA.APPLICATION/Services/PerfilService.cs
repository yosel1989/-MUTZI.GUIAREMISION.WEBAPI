using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Perfil;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class PerfilService: IPerfilService
    {
        private readonly IAuthService _authService;
        private readonly IPerfilRepository _repository;
        private readonly IMapper _mapper;
        public PerfilService(
            IPerfilRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<PerfilDTO?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return _mapper.Map<PerfilDTO>(data);
        }

        public async Task<PagedResult<PerfilDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize, search);

            return new PagedResult<PerfilDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<PerfilDTO>>(data)
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<UpdatedStatusDto> CambiarEstadoAsync(int id, PerfilUpdateEstadoDTO infoRequestDto)
        {
            var userJwt = _authService.CurrentUser();
            string _status = infoRequestDto.EstadoId switch
            {
                1 => "activo",
                0 => "inactivo",
                _ => throw new KeyNotFoundException("El nuevo estado solo acepta valores de '0' ó '1'"),
            };
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Conductor con ID {id} no encontrado.");
            }
            existing.Estado = _status;
            existing.EstadoId = (sbyte)infoRequestDto.EstadoId;
            existing.UsuarioModifico = userJwt.UserName;
            existing.FechaModifico = DateTime.UtcNow;
            await _repository.UpdateAsync(existing);

            return new UpdatedStatusDto
            {
                Id = existing.PerfilId,
                EstadoId = Convert.ToInt32(existing.EstadoId),
                Estado = existing.Estado,
                FechaEdicion = existing.FechaModifico,
                EmpleadoNombreEdicion = userJwt.UserName,
                Detalle = $"Perfil con ID {existing.PerfilId} ha sido actualizado a estado '{existing.Estado}'."
            };
        }

        public async Task<int> CreateAsync(PerfilCreateDTO createDTO)
        {
            try
            {
                var userJwt = _authService.CurrentUser();
                var nuevoPerfil = _mapper.Map<Perfil>(createDTO);
                nuevoPerfil.UsuarioRegistro = userJwt.UserName;
                nuevoPerfil.FechaRegistro = DateTime.UtcNow;
                await _repository.CreateAsync(nuevoPerfil);
                return nuevoPerfil.PerfilId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del establecimiento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }

        public async Task<PerfilDTO> UpdateAsync(int id, PerfilUpdateDTO updateDTO)
        {
            try
            {
                var userJwt = _authService.CurrentUser();
                var perfilActualizado = _mapper.Map<Perfil>(updateDTO);
                perfilActualizado.UsuarioModifico = userJwt.UserName;
                var establecimientoActualizado = await _repository.UpdateAsync(perfilActualizado);
                return _mapper.Map<PerfilDTO>(establecimientoActualizado);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del establecimiento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }

        public async Task<List<PerfilListToSelectDTO>> ListToSelectAsync()
        {
            try
            {
                var data = await _repository.GetListToSelectAsync();
                return _mapper.Map<List<PerfilListToSelectDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

    }
}
