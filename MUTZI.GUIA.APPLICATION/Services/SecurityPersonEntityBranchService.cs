using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranch;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class SecurityPersonEntityBranchService: ISecurityPersonEntityBranchService
    {
        private readonly ISecurityPersonEntityBranchRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public SecurityPersonEntityBranchService(
            ISecurityPersonEntityBranchRepository repository,
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<bool> CreateAsync(SecurityPersonEntityBranchCreateDTO request)
        {
            var currentUser = _authService.CurrentUser();
            try
            {
                var newSecurityPersonEntityBranch = _mapper.Map<SecurityPersonEntityBranch>(request);
                newSecurityPersonEntityBranch.CreatedAtUser = currentUser.UserName;
                newSecurityPersonEntityBranch.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                newSecurityPersonEntityBranch.CreatedAt = DateTime.UtcNow;
                newSecurityPersonEntityBranch.CreatedAtEmployeeId = currentUser.EmployeeId;
                return await _repository.CreateAsync(newSecurityPersonEntityBranch);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(SecurityPersonEntityBranchDeleteDTO request)
        {
            var currentUser = _authService.CurrentUser();
            try
            {
                var securityPersonEntityBranch = _mapper.Map<SecurityPersonEntityBranch>(request);
                return await _repository.DeleteAsync(securityPersonEntityBranch);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PagedResult<SecurityPersonEntityBranchDTO>> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(securityPersonId, pageNumber, pageSize, search);

            return new PagedResult<SecurityPersonEntityBranchDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<SecurityPersonEntityBranchDTO>>(data)
            };
        }

    }
}
