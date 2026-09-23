using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class SecurityPersonService: ISecurityPersonService
    {
        private readonly ISecurityPersonRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public SecurityPersonService(
            ISecurityPersonRepository repository,
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<int> CreateAsync(SecurityPersonCreateDTO request)
        {
            var currentUser = _authService.CurrentUser();
            try
            {
                var establecimiento = _mapper.Map<SecurityPerson>(request);
                establecimiento.CreatedAtUser = currentUser.UserName;
                establecimiento.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                establecimiento.CreatedAt = DateTime.UtcNow;
                await _repository.CreateAsync(establecimiento);
                return establecimiento.SecurityPersonId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<SecurityPersonDTO> UpdateAsync(int id, SecurityPersonUpdateDTO request)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var editSecurityPerson = await _repository.GetByIdAsync(id, strict: true);

                editSecurityPerson!.PersonId = request.PersonId;
                editSecurityPerson!.UpdatedAt = DateTime.UtcNow;
                editSecurityPerson!.UpdatedAtUser = currentUser.UserName;
                editSecurityPerson!.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                var establecimientoActualizado = await _repository.UpdateAsync(editSecurityPerson);
                return _mapper.Map<SecurityPersonDTO>(establecimientoActualizado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<SecurityPersonDTO?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id, strict: true);
            return _mapper.Map<SecurityPersonDTO>(data);
        }

        public async Task<PagedResult<SecurityPersonDTO>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize, search);
            var dataDTO = _mapper.Map<List<SecurityPersonDTO>>(data);

            return new PagedResult<SecurityPersonDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = dataDTO
            };
        }

    }
}
