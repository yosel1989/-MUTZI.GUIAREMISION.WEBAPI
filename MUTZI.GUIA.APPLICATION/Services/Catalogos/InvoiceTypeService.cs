using AutoMapper;
using Microsoft.Extensions.Logging;
using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories.Catalogo;

namespace MUTZI.GUIA.APPLICATION.Services.Catalogos
{
    public class InvoiceTypeService: IInvoiceTypeService
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceTypeRepository _repository;
        private readonly ILogger<InvoiceTypeService> _logger;
        public InvoiceTypeService(
            IMapper mapper,
            IInvoiceTypeRepository repository,
            ILogger<InvoiceTypeService> logger
        )
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<InvoiceTypeToSelectDTO>> GetInvoiceTypeToSelect()
        {
            var data = await _repository.GetToSelectAsync();
            var dataDTO = _mapper.Map<IEnumerable<InvoiceTypeToSelectDTO>>(data);
            return dataDTO;
        }
    }
}
