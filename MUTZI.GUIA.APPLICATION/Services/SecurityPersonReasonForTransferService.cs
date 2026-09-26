using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonReasonForTransfer;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class SecurityPersonReasonForTransferService: ISecurityPersonReasonForTransferService
    {
        private readonly ISecurityPersonReasonForTransferRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public SecurityPersonReasonForTransferService(
            ISecurityPersonReasonForTransferRepository repository,
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<bool> CreateAsync(SecurityPersonReasonForTransferCreateDTO request)
        {
            var currentUser = _authService.CurrentUser();
            try
            {
                var newSecurityPersonReasonForTransfer = _mapper.Map<SecurityPersonReasonForTransfer>(request);
                newSecurityPersonReasonForTransfer.CreatedAtUser = currentUser.UserName;
                newSecurityPersonReasonForTransfer.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}".ToUpper();
                newSecurityPersonReasonForTransfer.CreatedAt = DateTime.UtcNow;
                newSecurityPersonReasonForTransfer.CretedAtEmployeeId = currentUser.EmployeeId;
                return await _repository.CreateAsync(newSecurityPersonReasonForTransfer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(SecurityPersonReasonForTransferDeleteDTO request)
        {
            var currentUser = _authService.CurrentUser();
            try
            {
                var SecurityPersonReasonForTransfer = _mapper.Map<SecurityPersonReasonForTransfer>(request);
                return await _repository.DeleteAsync(SecurityPersonReasonForTransfer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PagedResult<SecurityPersonReasonForTransferDTO>> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search)
        {
            var (data, totalRecords) = await _repository.GetListAsync(securityPersonId, pageNumber, pageSize, search);

            return new PagedResult<SecurityPersonReasonForTransferDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<SecurityPersonReasonForTransferDTO>>(data)
            };
        }

    }
}
