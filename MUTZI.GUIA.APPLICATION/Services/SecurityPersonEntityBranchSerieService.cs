using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranchSerie;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class SecurityPersonEntityBranchSerieService: ISecurityPersonEntityBranchSerieService
    {
        private readonly ISecurityPersonEntityBranchSerieRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public SecurityPersonEntityBranchSerieService(
            ISecurityPersonEntityBranchSerieRepository repository,
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<bool> CreateAsync(SecurityPersonEntityBranchSerieCreateDTO request)
        {
            var currentUser = _authService.CurrentUser();
            try
            {
                var newSecurityPersonEntityBranchSerie = _mapper.Map<SecurityPersonEntityBranchSerie>(request);
                newSecurityPersonEntityBranchSerie.CreatedAtUser = currentUser.UserName;
                newSecurityPersonEntityBranchSerie.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                newSecurityPersonEntityBranchSerie.CreatedAt = DateTime.UtcNow;
                newSecurityPersonEntityBranchSerie.CreatedAtEmployeeId = currentUser.EmployeeId;
                return await _repository.CreateAsync(newSecurityPersonEntityBranchSerie);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(SecurityPersonEntityBranchSerieDeleteDTO request)
        {
            var currentUser = _authService.CurrentUser();
            try
            {
                var SecurityPersonEntityBranchSerie = _mapper.Map<SecurityPersonEntityBranchSerie>(request);
                return await _repository.DeleteAsync(SecurityPersonEntityBranchSerie);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PagedResult<SecurityPersonEntityBranchSerieDTO>> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(securityPersonId, pageNumber, pageSize, search);

            return new PagedResult<SecurityPersonEntityBranchSerieDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<SecurityPersonEntityBranchSerieDTO>>(data)
            };
        }

    }
}
