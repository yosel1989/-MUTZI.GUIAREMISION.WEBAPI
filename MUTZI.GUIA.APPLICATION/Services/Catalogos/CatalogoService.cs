using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services.Catalogos
{
    public class CatalogoService: ICatalogoService
    {
        private readonly IMapper _mapper;
        private readonly ITipoDocumentoRepository _tipoDocumentoRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IEntidadReguladoraRepository _entidadReguladoraRepository;
        private readonly IUnidadMedidaRepository _unidadMedidaRepository;
        private readonly IBienNormalizadoRepository _bienNormalizadoRepository;
        private readonly IDocumentoRelacionadoRepository _documentoRelacionadoRepository;
        public CatalogoService(
            IMapper mapper,
            ITipoDocumentoRepository tipoDocumentoRepository,
            IEntidadReguladoraRepository entidadReguladoraRepository,
            IUnidadMedidaRepository unidadMedidaRepository,
            IBienNormalizadoRepository bienNormalizadoRepository,
            IDocumentoRelacionadoRepository documentoRelacionadoRepository,
            ICountryRepository countryRepository
        )
        {
            _mapper = mapper;
            _tipoDocumentoRepository = tipoDocumentoRepository;
            _entidadReguladoraRepository = entidadReguladoraRepository;
            _unidadMedidaRepository = unidadMedidaRepository;
            _bienNormalizadoRepository = bienNormalizadoRepository;
            _documentoRelacionadoRepository = documentoRelacionadoRepository;
            _countryRepository = countryRepository;
        }
        public IEnumerable<EmisorAutorizadorVehicularDTO> GetEmisoresVehiculares()
        {
            var listEmisores = new List<EmisorAutorizadorVehicularDTO>();
            var emisores = new[]
            {
                new EmisorAutorizadorVehicularDTO { Codigo = "01", Nombre = "Superintendencia Nacional de Control de Servicios de Seguridad, Armas, Municiones y Explosivos de Uso Civil", Abreviatura = "SUCAMEC" },
                new EmisorAutorizadorVehicularDTO { Codigo = "02", Nombre = "Dirección General de Medicamentos Insumos y Drogas", Abreviatura = "DIGEMID" },
                new EmisorAutorizadorVehicularDTO { Codigo = "03", Nombre = "Dirección General de Salud Ambiental", Abreviatura = "DIGESA" },
                new EmisorAutorizadorVehicularDTO { Codigo = "04", Nombre = "Servicio Nacional de Sanidad Agraria", Abreviatura = "SENASA" },
                new EmisorAutorizadorVehicularDTO { Codigo = "05", Nombre = "Servicio Nacional Forestal y de Fauna Silvestre", Abreviatura = "SERFOR" },
                new EmisorAutorizadorVehicularDTO { Codigo = "06", Nombre = "Ministerio de Transportes y Comunicaciones", Abreviatura = "MTC" },
                new EmisorAutorizadorVehicularDTO { Codigo = "07", Nombre = "Ministerio de la Producción", Abreviatura = "PRODUCE" },
                new EmisorAutorizadorVehicularDTO { Codigo = "08", Nombre = "Ministerio del Ambiente", Abreviatura = "MIN. AMBIENTE" },
                new EmisorAutorizadorVehicularDTO { Codigo = "09", Nombre = "Organismo Nacional de Sanidad Pesquera", Abreviatura = "SANIPES" },
                new EmisorAutorizadorVehicularDTO { Codigo = "10", Nombre = "Municipalidad Metropolitana de Lima", Abreviatura = "MML" },
                new EmisorAutorizadorVehicularDTO { Codigo = "11", Nombre = "Ministerio de Salud", Abreviatura = "MINSA" },
                new EmisorAutorizadorVehicularDTO { Codigo = "12", Nombre = "Gobierno Regional", Abreviatura = "GR" }
            };
            listEmisores.AddRange(emisores);
            return listEmisores;
        }
        public async Task<IEnumerable<CountryDTO>> GetCountryCollection()
        {
            var data = await _countryRepository.GetListToSelectAsync();
            var dataDTO = _mapper.Map<IEnumerable<CountryDTO>>(data);

            return dataDTO;
        }


        public IEnumerable<TipoEstablecimientoDTO> GetTiposEstablecimiento()
        {
            var listPaises = new List<TipoEstablecimientoDTO>();

            var paises = new[]
            {
                new TipoEstablecimientoDTO { Tipo = "DOMICILIO FISCAL", Codigo = "domicilio_fiscal" },
                new TipoEstablecimientoDTO { Tipo = "CASA MATRIZ", Codigo = "casa_matriz" },
                new TipoEstablecimientoDTO { Tipo = "SUCURSAL", Codigo = "sucursal" },
                new TipoEstablecimientoDTO { Tipo = "AGENDA", Codigo = "agenda" },
                new TipoEstablecimientoDTO { Tipo = "LOCAL COMERCIAL O DE SERVICIOS", Codigo = "local_comercial" },
                new TipoEstablecimientoDTO { Tipo = "SEDE PRODUCTIVA", Codigo = "sede_productiva" },
                new TipoEstablecimientoDTO { Tipo = "DEPOSITO (ALMACÉN)", Codigo = "almacen" },
                new TipoEstablecimientoDTO { Tipo = "OFICINA ADMINISTRATIVA", Codigo = "oficina_administrativa" }
            };

            listPaises.AddRange(paises);

            return listPaises;
        }


        public async Task<IEnumerable<TipoDocumentoToSelectDTO>> GetTiposDocumento(string? tipoRegimen)
        {
            var tiposDocumento = await _tipoDocumentoRepository.GetListToSelectAsync(tipoRegimen);
            return _mapper.Map<IEnumerable<TipoDocumentoToSelectDTO>>(tiposDocumento);
        }


        public async Task<IEnumerable<EntidadReguladoraToSelectDTO>> GetEntidadesReguladoras()
        {
            var entidades = await _entidadReguladoraRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EntidadReguladoraToSelectDTO>>(entidades);
        }

        public async Task<IEnumerable<UnidadMedidaToSelectDTO>> GetUnidadesMedida(string? tipo)
        {
            var unidadesMedida = await _unidadMedidaRepository.GetListPesoToSelectAsync(tipo);
            return _mapper.Map<IEnumerable<UnidadMedidaToSelectDTO>>(unidadesMedida);
        }

        public async Task<IEnumerable<BienNormalizadoToSelectDTO>> GetBienesNormalizados()
        {
            var bienesNormalizados = await _bienNormalizadoRepository.GetAllToSelectAsync();
            return _mapper.Map<IEnumerable<BienNormalizadoToSelectDTO>>(bienesNormalizados);
        }

        public async Task<IEnumerable<DocumentoRelacionadoToSelectDTO>> GetDocumentosRelacionados(string? tipo)
        {
            var documentosRelacionados = await _documentoRelacionadoRepository.GetAllToSelectAsync(tipo);
            return _mapper.Map<IEnumerable<DocumentoRelacionadoToSelectDTO>>(documentosRelacionados);
        }

    }
}
