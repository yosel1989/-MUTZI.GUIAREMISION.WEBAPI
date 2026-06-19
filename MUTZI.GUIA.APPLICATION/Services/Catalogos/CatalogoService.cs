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
            IDocumentoRelacionadoRepository documentoRelacionadoRepository
        )
        {
            _mapper = mapper;
            _tipoDocumentoRepository = tipoDocumentoRepository;
            _entidadReguladoraRepository = entidadReguladoraRepository;
            _unidadMedidaRepository = unidadMedidaRepository;
            _bienNormalizadoRepository = bienNormalizadoRepository;
            _documentoRelacionadoRepository = documentoRelacionadoRepository;
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
        public IEnumerable<PaisDTO> GetPaises()
        {
            var listPaises = new List<PaisDTO>();

            var paises = new[]
            {
                new PaisDTO { Pais = "Afganistán", Codigo = "AF" },
                new PaisDTO { Pais = "Albania", Codigo = "AL" },
                new PaisDTO { Pais = "Alemania", Codigo = "DE" },
                new PaisDTO { Pais = "Andorra", Codigo = "AD" },
                new PaisDTO { Pais = "Angola", Codigo = "AO" },
                new PaisDTO { Pais = "Arabia Saudita", Codigo = "SA" },
                new PaisDTO { Pais = "Argentina", Codigo = "AR" },
                new PaisDTO { Pais = "Armenia", Codigo = "AM" },
                new PaisDTO { Pais = "Australia", Codigo = "AU" },
                new PaisDTO { Pais = "Austria", Codigo = "AT" },
                new PaisDTO { Pais = "Bahamas", Codigo = "BS" },
                new PaisDTO { Pais = "Bahréin", Codigo = "BH" },
                new PaisDTO { Pais = "Bangladesh", Codigo = "BD" },
                new PaisDTO { Pais = "Barbados", Codigo = "BB" },
                new PaisDTO { Pais = "Bélgica", Codigo = "BE" },
                new PaisDTO { Pais = "Belice", Codigo = "BZ" },
                new PaisDTO { Pais = "Bolivia", Codigo = "BO" },
                new PaisDTO { Pais = "Brasil", Codigo = "BR" },
                new PaisDTO { Pais = "Bulgaria", Codigo = "BG" },
                new PaisDTO { Pais = "Canadá", Codigo = "CA" },
                new PaisDTO { Pais = "Chile", Codigo = "CL" },
                new PaisDTO { Pais = "China", Codigo = "CN" },
                new PaisDTO { Pais = "Colombia", Codigo = "CO" },
                new PaisDTO { Pais = "Costa Rica", Codigo = "CR" },
                new PaisDTO { Pais = "Cuba", Codigo = "CU" },
                new PaisDTO { Pais = "Dinamarca", Codigo = "DK" },
                new PaisDTO { Pais = "Ecuador", Codigo = "EC" },
                new PaisDTO { Pais = "Egipto", Codigo = "EG" },
                new PaisDTO { Pais = "El Salvador", Codigo = "SV" },
                new PaisDTO { Pais = "España", Codigo = "ES" },
                new PaisDTO { Pais = "Estados Unidos", Codigo = "US" },
                new PaisDTO { Pais = "Estonia", Codigo = "EE" },
                new PaisDTO { Pais = "Finlandia", Codigo = "FI" },
                new PaisDTO { Pais = "Francia", Codigo = "FR" },
                new PaisDTO { Pais = "Grecia", Codigo = "GR" },
                new PaisDTO { Pais = "Guatemala", Codigo = "GT" },
                new PaisDTO { Pais = "Honduras", Codigo = "HN" },
                new PaisDTO { Pais = "Hungría", Codigo = "HU" },
                new PaisDTO { Pais = "India", Codigo = "IN" },
                new PaisDTO { Pais = "Indonesia", Codigo = "ID" },
                new PaisDTO { Pais = "Irlanda", Codigo = "IE" },
                new PaisDTO { Pais = "Islandia", Codigo = "IS" },
                new PaisDTO { Pais = "Israel", Codigo = "IL" },
                new PaisDTO { Pais = "Italia", Codigo = "IT" },
                new PaisDTO { Pais = "Jamaica", Codigo = "JM" },
                new PaisDTO { Pais = "Japón", Codigo = "JP" },
                new PaisDTO { Pais = "México", Codigo = "MX" },
                new PaisDTO { Pais = "Nicaragua", Codigo = "NI" },
                new PaisDTO { Pais = "Noruega", Codigo = "NO" },
                new PaisDTO { Pais = "Panamá", Codigo = "PA" },
                new PaisDTO { Pais = "Paraguay", Codigo = "PY" },
                new PaisDTO { Pais = "Perú", Codigo = "PE" },
                new PaisDTO { Pais = "Polonia", Codigo = "PL" },
                new PaisDTO { Pais = "Portugal", Codigo = "PT" },
                new PaisDTO { Pais = "Puerto Rico", Codigo = "PR" },
                new PaisDTO { Pais = "Reino Unido", Codigo = "GB" },
                new PaisDTO { Pais = "República Dominicana", Codigo = "DO" },
                new PaisDTO { Pais = "Rusia", Codigo = "RU" },
                new PaisDTO { Pais = "Sudáfrica", Codigo = "ZA" },
                new PaisDTO { Pais = "Suecia", Codigo = "SE" },
                new PaisDTO { Pais = "Suiza", Codigo = "CH" },
                new PaisDTO { Pais = "Turquía", Codigo = "TR" },
                new PaisDTO { Pais = "Ucrania", Codigo = "UA" },
                new PaisDTO { Pais = "Uruguay", Codigo = "UY" },
                new PaisDTO { Pais = "Venezuela", Codigo = "VE" },
                new PaisDTO { Pais = "Vietnam", Codigo = "VN" }
            };

            listPaises.AddRange(paises);

            return listPaises;
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
