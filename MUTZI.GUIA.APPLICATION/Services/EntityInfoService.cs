using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.PersonaEmpresaInfo;
using MUTZI.GUIA.APPLICATION.Interfaces.Ports;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;


namespace MUTZI.GUIA.APPLICATION.Services
{
    public class EntityInfoService: IEntityInfoService
    {
        public IEntityInfoPort _entityInfoPort;
        public IMapper _mapper;
        public EntityInfoService(IEntityInfoPort entityInfoPort, IMapper mapper)
        {
            _entityInfoPort = entityInfoPort;
            _mapper = mapper;
        }

        public async Task<PersonInfoDTO> GetPersonInfoAsync(string documentNumber)
        {
            try
            {
                var data = await _entityInfoPort.GetPersonInfoAsync(documentNumber);
                var dataDTO = _mapper.Map<PersonInfoDTO>(data);

                return dataDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultas la información de la persona: {documentNumber}");
            }
        }


        public async Task<CompanyInfoDTO> GetCompanyInfoAsync(string documentNumber)
        {
            try
            {
                var data = await _entityInfoPort.GetCompanyInfoAsync(documentNumber);
                var dataDTO = _mapper.Map<CompanyInfoDTO>(data);

                return dataDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultas la información de la empresa: {documentNumber}");
            }
        }
    }
}
