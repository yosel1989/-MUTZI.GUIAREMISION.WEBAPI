using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.OperacionResponsable;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class OperacionResponsableService: IOperacionResponsableService
    {
        private readonly IAuthService _authService;
        private readonly IOperacionResponsableRepository _repository;
        private readonly IMapper _mapper;
        public OperacionResponsableService(
            IOperacionResponsableRepository repository, 
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<OperacionResponsableDTO?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return _mapper.Map<OperacionResponsableDTO>(data);
        }

        public async Task<PagedResult<OperacionResponsableDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize, search);

            return new PagedResult<OperacionResponsableDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<OperacionResponsableDTO>>(data)
            };
        }
        
        public async Task<int> CreateAsync(OperacionResponsableCreateDTO createDTO)
        {
            try
            {
                var userJwt = _authService.CurrentUser();
                var nuevoOperacionResponsable = _mapper.Map<OperacionResponsable>(createDTO);
                nuevoOperacionResponsable.PersonalRegistroId = userJwt.EmployeeId;
                nuevoOperacionResponsable.UsuarioRegistro = userJwt.UserName;
                nuevoOperacionResponsable.UsuarioRegistroNombre = $"{userJwt.FirstName} {userJwt.LastName}";
                nuevoOperacionResponsable.FechaRegistro = DateTime.UtcNow;
                await _repository.CreateAsync(nuevoOperacionResponsable);
                return nuevoOperacionResponsable.OperacionResponsableId;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

        public async Task<OperacionResponsableDTO> UpdateAsync(int id, OperacionResponsableUpdateDTO updateDTO)
        {
            try
            {
                var userJwt = _authService.CurrentUser();
                var OperacionResponsableActualizado = _mapper.Map<OperacionResponsable>(updateDTO);
                OperacionResponsableActualizado.UsuarioModifico = userJwt.UserName;
                var establecimientoActualizado = await _repository.UpdateAsync(OperacionResponsableActualizado);
                return _mapper.Map<OperacionResponsableDTO>(establecimientoActualizado);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del establecimiento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }

        public async Task<List<OperacionResponsableToSelectDTO>> ListToSelectAsync()
        {
            try
            {
                var data = await _repository.GetListToSelectAsync();
                return _mapper.Map<List<OperacionResponsableToSelectDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

    }
}
