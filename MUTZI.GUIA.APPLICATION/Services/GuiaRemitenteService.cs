using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Enums;
using MUTZI.GUIA.DOMAIN.Interfaces;
using MUTZI.GUIA.DOMAIN.Interfaces.Factories;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using QuestPDF.Fluent;
using System.Text.Json;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class GuiaRemitenteService : IGuiaRemitenteService
    {
        private readonly IGuiaRemitenteRepository _repository;
        private readonly IGuiaRemisionHistorialRepository _guiaRemisionHistorialRepository;
        private readonly IIntegrationCredentialService _integrationCredentialService;
        private readonly IIntegrationCredentialRepository _integrationCredentialRepository;
        private readonly IGuiaRemisionTransportUnitRepository _guiaRemisionTransportUnitRepository;
        private readonly IGuiaRemisionEntityCarrierRepository _guiaRemisionEntityCarrierRepository;
        private readonly ISecurityPersonReasonForTransferRepository _securityPersonReasonForTransferRepository;
        private readonly IEntityBranchRepository _establecimientoRepository;
        private readonly IEntityBranchSerieRepository _entityBranchSerieRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IMotivoTrasladoRepository _motivoTrasladoRepository;
        private readonly ITransportistaRepository _transportistaRepository;
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IVentaConfigRepository _ventaConfigRepository;
        private readonly IGuiaRemitenteUBLService _guiaRemitenteUBLService;
        private readonly IGuiaRemisionDocumentFactory _guiaRemisionDocumentFactory;
        private readonly IEnvioService _envioService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IAppDBContext _appDBContext;
        public GuiaRemitenteService(
            IGuiaRemitenteRepository repository,
            IGuiaRemisionHistorialRepository guiaRemisionHistorialRepository,
            IGuiaRemisionTransportUnitRepository guiaRemisionTransportUnitRepository,
            IGuiaRemisionEntityCarrierRepository guiaRemisionEntityCarrierRepository,
            ISecurityPersonReasonForTransferRepository securityPersonReasonForTransferRepository,
            IGuiaRemitenteUBLService guiaRemitenteUBLService,
            IEnvioService envioService,
            IVentaConfigRepository ventaConfigRepository,
            IMotivoTrasladoRepository motivoTrasladoRepository,
            ITransportistaRepository transportistaRepository,
            IEntityBranchRepository establecimientoRepository,
            IEntityBranchSerieRepository entityBranchSerieRepository,
            IIntegrationCredentialService integrationCredentialService,
            IIntegrationCredentialRepository integrationCredentialRepository,
            IProveedorRepository proveedorRepository,
            IGuiaRemisionDocumentFactory guiaRemisionDocumentFactory,
            IEntityRepository entityRepository,
            IMapper mapper,
            IAuthService authService,
            IAppDBContext appDBContext
        )
        {
            _repository = repository;
            _guiaRemisionHistorialRepository = guiaRemisionHistorialRepository;
            _guiaRemisionTransportUnitRepository = guiaRemisionTransportUnitRepository;
            _guiaRemisionEntityCarrierRepository = guiaRemisionEntityCarrierRepository;
            _securityPersonReasonForTransferRepository = securityPersonReasonForTransferRepository;
            _guiaRemitenteUBLService = guiaRemitenteUBLService;
            _envioService = envioService;
            _establecimientoRepository = establecimientoRepository;
            _motivoTrasladoRepository = motivoTrasladoRepository;
            _transportistaRepository = transportistaRepository;
            _proveedorRepository = proveedorRepository;
            _entityRepository = entityRepository;
            _entityBranchSerieRepository = entityBranchSerieRepository;
            _ventaConfigRepository = ventaConfigRepository;
            _guiaRemisionDocumentFactory = guiaRemisionDocumentFactory;
            _integrationCredentialService = integrationCredentialService;
            _integrationCredentialRepository = integrationCredentialRepository;
            _mapper = mapper;
            _authService = authService;
            _appDBContext = appDBContext;
        }
        public async Task<List<GuiaRemitenteDTO>> GetAllAsync()
        {
            var guiaRemisions = await _repository.GetAllAsync();
            return _mapper.Map<List<GuiaRemitenteDTO>>(guiaRemisions);
        }
        public async Task<PagedResult<GuiaRemisionListDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q)
        {
            var filters = new Dictionary<string, string>();

            int i = 0;
            while (q.ContainsKey($"columns[{i}][data]"))
            {
                var dataField = q[$"columns[{i}][data]"].ToString();
                var searchValue = q[$"columns[{i}][search][value]"].ToString();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    filters[dataField] = searchValue;
                }

                i++;
            }

            var (guias, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize, filters);

            var guiasDTO = _mapper.Map<List<GuiaRemisionListDTO>>(guias);

            return new PagedResult<GuiaRemisionListDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = guiasDTO
            };
        }
        public async Task<byte[]> ExportCollectionGuiaAsync(int pageNumber, int pageSize, IQueryCollection q)
        {
            throw new NotImplementedException();
        }
        public async Task<byte[]> ExportGuiasAsync(int pageNumber, int pageSize, IQueryCollection q)
        {
            try
            {
                var filters = new Dictionary<string, string>();

                int i = 0;
                while (q.ContainsKey($"columns[{i}][data]"))
                {
                    var dataField = q[$"columns[{i}][data]"].ToString();
                    var searchValue = q[$"columns[{i}][search][value]"].ToString();

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        filters[dataField] = searchValue;
                    }

                    i++;
                }

                var (guias, _) = await _repository.GetListAsync(pageNumber, pageSize, filters, true);
                var guiasDTO = _mapper.Map<List<GuiaRemisionListDTO>>(guias);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte de Guías");

                    string[] headers = {
                        "#", "Código", "EntitySenderName", "RUC EntitySenderName", "Remitente / Local",
                        "Tipo Guía", "N° Guía", "T. Traslado", "T. UnidadTransporte",
                        "F. Emisión", "H. Emisión", "Destinatario", "N° Doc. Destinatario",
                        "Origen", "Destino", "Estado", "F. Registro", "U. Registro"
                    };

                    for (int col = 0; col < headers.Length; col++)
                    {
                        worksheet.Cell(1, col + 1).Value = headers[col];
                    }

                    int row = 2;
                    foreach (var item in guiasDTO)
                    {
                        worksheet.Cell(row, 1).Value = row - 1;
                        worksheet.Cell(row, 2).Value = $"COD-{item.GuiaRemisionId.ToString("D4")}";
                        //worksheet.Cell(row, 3).Value = item.EntitySenderName;
                        //worksheet.Cell(row, 4).Value = item.EntitySenderDocumentNumber;
                        //worksheet.Cell(row, 5).Value = item.EstablecimientoRemitente;
                        worksheet.Cell(row, 6).Value = item.TipoGuia;
                        worksheet.Cell(row, 7).Value = item.NumeroGuia;
                        worksheet.Cell(row, 8).Value = item.MotivoTrasladoNombre;
                        worksheet.Cell(row, 9).Value = item.TipoTransporte;
                        worksheet.Cell(row, 10).Value = item.FechaEmision;
                        worksheet.Cell(row, 11).Value = item.HoraEmision;
                        //worksheet.Cell(row, 12).Value = item.EstablecimientoDestinatario;
                        worksheet.Cell(row, 13).Value = item.NumeroDocumentoDestinatario;
                        worksheet.Cell(row, 14).Value = item.DistritoOrigen;
                        worksheet.Cell(row, 15).Value = item.DistritoDestino;

                        var cellEstado = worksheet.Cell(row, 16);
                        cellEstado.Value = item.Estado?.ToUpper();
                        cellEstado.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        if (item.Estado == "Emitida")
                        {
                            cellEstado.Style.Fill.BackgroundColor = XLColor.FromHtml("#ab4141");
                            cellEstado.Style.Font.FontColor = XLColor.FromHtml("#006100");
                        }
                        else if (item.Estado == "Registrada")
                        {
                            cellEstado.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e7eb");
                        }

                        //worksheet.Cell(row, 17).Value = item.CreatedAt?.ToString("dd/MM/yyyy hh:mm:ss tt");
                        worksheet.Cell(row, 17).Value = item.FechaRegistro;
                        worksheet.Cell(row, 18).Value = item.UsuarioRegistro;

                        row++;
                    }

                    var headerRange = worksheet.Range(1, 1, 1, headers.Length);
                    headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#fce8e8");
                    headerRange.Style.Font.FontColor = XLColor.FromHtml("#ab4141");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    var usedRange = worksheet.RangeUsed();
                    usedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    usedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    usedRange.Style.Font.FontSize = 9;

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GuiaRemitenteDTO> GetByUuidAsync(Guid uuid)
        {
            var guiaRemision = await _repository.GetByUuidAsync(uuid);
            if(guiaRemision is null)
            {
                throw new Exception("No se encontró la guía de remisión.");
            }

            var guiaRemisionMapeado = _mapper.Map<GuiaRemitenteDTO>(guiaRemision);

            return guiaRemisionMapeado;
        }

        public async Task<object> GetByIdAsync(int id)
        {
            var guiaRemision = await _repository.GetByIdAsync(id);

            if(guiaRemision is null) throw new ArgumentNullException(nameof(guiaRemision));

            return guiaRemision.TipoGuia switch
            {
                "REMITENTE" => _mapper.Map<GuiaRemitenteDTO>(guiaRemision),
                "TRANSPORTISTA" => _mapper.Map<GuiaTransportistaDTO>(guiaRemision),
                _ => throw new KeyNotFoundException("El tipo guia solo puede ser REMITENTE o TRANSPORTISTA.")
            };
        }
        public async Task<GuiaPorTicketDTO> GetByTicketAsync(string ticket)
        {
            var guiaRemision = await _repository.GetByTicketAsync(ticket);
            return _mapper.Map<GuiaPorTicketDTO>(guiaRemision);
        }
        public async Task<object> CreateAsync(string ruc, GuiaRemitenteCreateDTO request)
        {
            await _appDBContext.BeginTransactionAsync();

            try
            {
                var currentUser = _authService.CurrentUser();

                // Validators

                if (!await _securityPersonReasonForTransferRepository.HasAssigned(currentUser.EmployeeId, request.MotivoTrasladoId))
                {
                    throw new Exception("No tiene asignado el motivo de traslado para poder emitir la guía de remisión.");
                }

                throw new Exception("dd");

                try
                {
                    var currentDate = DateTime.UtcNow;
                    // Mapear todos los objetos

                    var entityBranchSerie = await _entityBranchSerieRepository.GetByIdAsync(request.EntityBranchSerieId, strict: true);
                    var motivoTraslado = await _motivoTrasladoRepository.GetByIdAsync(request.MotivoTrasladoId, strict: true);
                    var remitente = await _establecimientoRepository.GetByIdAsync(request.RemitenteId, strict: true, "Remitente");
                    var destinatario = await _establecimientoRepository.GetByIdAsync(request.DestinatarioId, strict: true, "Destinatario");
                    var destinatarioEmails = request.DestinatarioEmail.Select(p => _mapper.Map<GuiaRemisionEmailDestinatario>(p)).ToList();
                    var proveedor = await _proveedorRepository.GetByIdAsync(request.ProveedorId ?? 0);
                    var conductor = request.DatosEnvio.ConductorId?.Select(p => _mapper.Map<GuiaRemisionConductor>(p)).ToList() ?? [];

                    var entityTransporter = await _entityRepository.GetByIdAsync(request.EntityCarrierId ?? 0);
                    var transportUnits = request.TransportUnits;

                    var guiaRemision = _mapper.Map<GuiaRemision>(request);
                    var datosEnvio = _mapper.Map<GuiaRemisionDatoEnvio>(request.DatosEnvio);

                    var origen = _mapper.Map<GuiaRemisionOrigen>(request.Origen);
                    var destino = request.Destino.Select(p => _mapper.Map<GuiaRemisionDestino>(p)).ToList();
                    var detalles = request.Productos.Select(p => _mapper.Map<GuiaRemisionDetalle>(p)).ToList();
                    var docRelacionados = request.DocRelacionado?.Select(p => _mapper.Map<GuiaRemisionDocumentoRelacionado>(p)).ToList();
                    var ventaConfig = await _ventaConfigRepository.GetByRucAsync(ruc);
                    var integrationCredential = await _integrationCredentialRepository.GetByEntityId(request.EntityId ?? 0, "efact", strict: true);

                    request.ValidarBienesNormalizados();

                    var (esValido, error) = ValidarDatosGuiaRemitente(request, ruc, remitente, destinatario, integrationCredential);
                    if (!esValido)
                    {
                        return new BadRequestResponseDTO
                        {
                            Error = "Error en datos enviados",
                            Detalle = error
                        };
                    }

                    guiaRemision.CreatedAtUser = currentUser.UserName;
                    guiaRemision.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
                    guiaRemision.CreatedAtEmployeeId = currentUser.EmployeeId;
                    guiaRemision.AreaId = currentUser.AreaId;
                    guiaRemision.CreatedAt = DateTime.UtcNow;


                    // Llamar al repositorio
                    var (guia_id, numero_guia) = await _repository.CreateGuiaRemisionTransactionalAsync(
                        remitente,
                        entityBranchSerie,
                        destinatario, 
                        proveedor, 
                        conductor,
                        //entityTransporter,
                        //transportUnits,
                        guiaRemision, 
                        datosEnvio, 
                        origen, 
                        destino, 
                        detalles, 
                        docRelacionados, 
                        destinatarioEmails
                    );

                    // Registrar la empresa transportista si tiene registro

                    if (request.EntityCarrierId != null)
                    {
                        await _guiaRemisionEntityCarrierRepository.Create(new GuiaRemisionEntityCarrier
                        {
                            GuiaRemisionId = guia_id,
                            EntityId = request.EntityCarrierId ?? 0,
                            CreatedAt = currentDate,
                            CreatedAtUser = currentUser.UserName,
                            CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}"
                        });
                    }

                    // Registrar los vehiculos solo si tiene registro
                    if(request.TransportUnits.Count > 0)
                    {
                        var _transportUnits = request.TransportUnits.Select(transportUnit =>
                        {
                            var guiaRemisionTransportUnit = _mapper.Map<GuiaRemisionTransportUnit>(transportUnit);
                            guiaRemisionTransportUnit.GuiaRemisionId = guia_id;
                            guiaRemisionTransportUnit.CreatedAt = currentDate;
                            guiaRemisionTransportUnit.CreatedAtUser = currentUser.UserName;
                            guiaRemisionTransportUnit.CreatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";

                            return guiaRemisionTransportUnit;
                        }).ToList();
                        await _guiaRemisionTransportUnitRepository.Create(_transportUnits);
                    }

                    // Registrar el historial
                    await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
                    {
                        GuiaRemisionId = guia_id,
                        Motivo = "Guía registrada",
                        UsuarioRegistro = currentUser.UserName,
                        UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
                        AreaId = currentUser.AreaId,
                        FechaRegistro = DateTime.UtcNow
                    });

                    await _appDBContext.CommitAsync();

                    return new GuiaCreatedResponseDTO
                    {
                        GuiaID = guia_id,
                        NumeroGuia = numero_guia,
                        TipoGuia = "REMITENTE"
                    };
                }
                catch (Exception ex)
                {
                    await _appDBContext.RollbackAsync();
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];

                    string errorBox = errorMsg.Contains("FOREIGN KEY") ? "Error de clave foránea."
                                    : errorMsg.Contains("UnitMeasurement") ? "Unidad de medida no válida."
                                    : errorMsg.Contains("String") ? "Error de longitud de campo."
                                    : "";

                    return new BadRequestResponseDTO
                    {
                        Error = "Error al guardar guía remitente",
                        Detalle = $"{errorBox} Más detalle: {errorMsg}"
                    };
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                return new InternalErrorResponseDTO
                {
                    Error = "Error inesperado",
                    Detalle = errorMsg
                };
            }
            //SE obtiene la guia de remision creada
            //var guiaCreada = await _repository.GetByIdAsync(guia_id);
            //return _mapper.Map<GuiaRemitenteDTO>(guiaCreada);
        }

        public async Task<GuiaRemitenteDTO> UpdateAsync(int id, GuiaRemitenteUpdateDTO guiaRemisionDto)
        {
            var existingGuiaRemision = await _repository.GetByIdAsync2(id);
            if (existingGuiaRemision == null)
            {
                throw new KeyNotFoundException("GuiaRemision de remisión no encontrada");
            }
            _mapper.Map(guiaRemisionDto, existingGuiaRemision);
            await _repository.UpdateAsync(existingGuiaRemision);
            return _mapper.Map<GuiaRemitenteDTO>(existingGuiaRemision);
        }
        public async Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado, int idEstado)
        {
            await _repository.UpdateRespuestaTicketAsync(guiaId, ticket, estado, idEstado);
        }
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteCreateDTO guia, string ruc, EntityBranch remitente, EntityBranch destinatario, IntegrationCredential integrationCredential)
        {
            try
            {
                if (guia.DocRelacionado?.Count > 3)
                {
                    return (false, "Solo se permite hasta 3 documentos relacionados como referencia en la guía de remisión transportista.");
                }

                if(guia.MotivoTrasladoId != 7)
                {
                    if (remitente.EntityId != integrationCredential.EntityId)
                        return (false, $"El remitente no pertenece a la entidad emisora.");
                }

                _ = guia.TipoTRansporteCode18();
                //_ = guia.TipoTrasladoCode20();

                if (guia.TipoTransporte=="PUBLICO")
                {
                    if (!guia.DatosEnvio.IndicadorTrasladoVehiculoCategoria)
                    {
                        if (guia.EntityCarrierId == null) { return (false, "Si tipoTransporte es PUBLICO, entonces debes seleccionar una empresa transportista."); }
                        if (guia.EntityCarrier?.DocumentNumber.Length != 11) { return (false, "Si tipoTransporte es PUBLICO, entonces debes completar los 11 dígitos de rucEmpresaCurrier."); }
                        //if (string.IsNullOrWhiteSpace(guia.DatosEnvio.Transportista.RazonSocial)) { return (false, "Si tipoTransporte es PUBLICO, La razonSocialCurrier no puede estar vacía"); }
                        //if (string.IsNullOrWhiteSpace(guia.DatosEnvio.Transportista.RegistroMTC)) { return (false, "Si tipoTransporte es PUBLICO, el registroMtcCurrier no puede estar vacío"); }
                    }
                }
                //if (guia.)
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteDTO guia, string ruc, EntityBranch remitente, EntityBranch destinatario, IntegrationCredential ventaConfig)
        {
            try
            {
                if (guia.DocRelacionados?.Count > 3)
                {
                    return (false, "Solo se permite hasta 3 documentos relacionados como referencia en la guía de remisión transportista.");
                }

                if(guia.MotivoTrasladoCodigo != "07")
                {
                    if (guia.Remitente.EntityDocumentNumber != ruc)
                        return (false, $"El RUC del remitente ({guia.Remitente.EntityDocumentNumber}) no coincide con el RUC de la empresa ({ruc}).");
                }

                


                _ = guia.TipoTRansporteCode18();
                //_ = guia.TipoTrasladoCode20();
                _ = guia.getTipoCodigoDocumento("RUC", guia.Remitente.EntityDocumentNumber);
                //_ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento, guia.Destinatario.NumeroDocumento);

                if (guia.Proveedor != null)
                {
                    _ = guia.getTipoCodigoDocumento(guia.Proveedor.TipoDocumentoNombre, guia.Proveedor.NumeroDocumento);
                }
                if (guia.DatosEnvio.GuiaRemisionConductores != null)
                {
                    foreach (var conductor in guia.DatosEnvio.GuiaRemisionConductores)
                    {
                        _ = guia.getTipoCodigoDocumento(conductor.TipoDocumento, conductor.NumeroDocumento);
                    }
                }
                //Si tipo de transporte es PUBLICO, entonces DatosEnvio.rucEmpresaCurrier debe ser obligatorio
                if (guia.TipoTransporte == "PUBLICO")
                {
                    if (!guia.DatosEnvio.IndicadorTrasladoVehiculoCategoria)
                    {
                        if (guia.EntityCarrierId == null) { return (false, "Si el tipo de transporte es PUBLICO, entonces debes seleccionar una entidad transportista."); }
                        if (guia.EntityCarrier?.DocumentNumber.Length != 11) { return (false, "Si tipo de transporte es PUBLICO, la entidad transportista debe tener su número de documento en un formato RUC válido."); }
                        if (string.IsNullOrWhiteSpace(guia.EntityCarrier?.Name)) { return (false, "Si el tipo de transporte es PUBLICO, la razón social de la entidad transportista no puede estar vacía"); }
                        if (string.IsNullOrWhiteSpace(guia.EntityCarrier?.MtcCode)) { return (false, "Si tipo de transporte es PUBLICO, la entidad debe tener registrado el código MTC."); }
                    }
                    
                }
                //if (guia.)
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }


        public async Task<GuiaRemisionListDTO> AprobarAsync(int id)
        {
            var currentUser = _authService.CurrentUser();
            GuiaRemision guiaRemision = await _repository.GetByIdWithoutRelations(id, strict: true);

            guiaRemision!.EstadoId = (int)EnumGuiaRemisionEstado.Aprobada;
            guiaRemision!.UpdatedAtUser = currentUser.UserName;
            guiaRemision!.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
            guiaRemision!.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(guiaRemision);

            // Registrar el historial
            await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
            {
                GuiaRemisionId = guiaRemision.GuiaRemisionId,
                Motivo = "Guía aprobada",
                ValorNuevo = EnumGuiaRemisionEstado.Aprobada.GetDescription(),
                UsuarioRegistro = currentUser.UserName,
                UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
                AreaId = currentUser.AreaId,
                FechaRegistro = DateTime.UtcNow
            });

            var guiaRemisionActualizada = await _repository.FindWithDetail(id);
            return _mapper.Map<GuiaRemisionListDTO>(guiaRemisionActualizada);
        }

        public async Task<GuiaRemisionListDTO> EmitirAsync(int id)
        {
            var currentUser = _authService.CurrentUser();
            var guiaRemision = await _repository.GetByIdAsync2(id);
            if (guiaRemision == null)
            {
                throw new KeyNotFoundException("GuiaRemision de remisión no encontrada");
            }
            /*if (guiaRemision.EstadoId == (int)EnumGuiaRemisionEstado.Aprobada)
            {
                throw new InvalidOperationException($"La guía ya se encuentra en estado '{EnumGuiaRemisionEstado.Aprobada.GetDescription()}'");
            }*/
            guiaRemision.EstadoId = (int)EnumGuiaRemisionEstado.Emitida;
            guiaRemision.UpdatedAtUser = currentUser.UserName;
            guiaRemision.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
            guiaRemision.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(guiaRemision);

            // Registrar el historial
            await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
            {
                GuiaRemisionId = guiaRemision.GuiaRemisionId,
                Motivo = "Guía emitida",
                ValorNuevo = EnumGuiaRemisionEstado.Emitida.GetDescription(),
                UsuarioRegistro = currentUser.UserName,
                UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
                AreaId = currentUser.AreaId,
                FechaRegistro = DateTime.UtcNow
            });

            var guiaRemisionActualizada = await _repository.FindWithDetail(id);
            return _mapper.Map<GuiaRemisionListDTO>(guiaRemisionActualizada);
        }


        public async Task<GuiaRemisionListDTO> ConfirmarAsync(int id)
        {
            var currentUser = _authService.CurrentUser();
            var guiaRemision = await _repository.GetByIdAsync2(id);
            if (guiaRemision == null)
            {
                throw new KeyNotFoundException("GuiaRemision de remisión no encontrada");
            }
            if (guiaRemision.EstadoId == (int)EnumGuiaRemisionEstado.Confirmada)
            {
                throw new InvalidOperationException($"La guía ya se encuentra en estado '{EnumGuiaRemisionEstado.Confirmada.GetDescription()}'");
            }

            guiaRemision.EstadoId = (int)EnumGuiaRemisionEstado.Confirmada;
            guiaRemision.UpdatedAtUser = currentUser.UserName;
            guiaRemision.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
            guiaRemision.UpdatedAt = DateTime.UtcNow;
            
            await _repository.UpdateAsync(guiaRemision);

            // Registrar el historial
            await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
            {
                GuiaRemisionId = guiaRemision.GuiaRemisionId,
                Motivo = "Guía confirmada",
                ValorNuevo = EnumGuiaRemisionEstado.Confirmada.GetDescription(),
                UsuarioRegistro = currentUser.UserName,
                UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
                AreaId = currentUser.AreaId,
                FechaRegistro = DateTime.UtcNow
            });

            var guiaRemisionActualizada = await _repository.FindWithDetail(id);
            return _mapper.Map<GuiaRemisionListDTO>(guiaRemisionActualizada);
        }

        public async Task<GuiaRemisionListDTO> AnularAsync(int id, string descripcion)
        {
            var currentUser = _authService.CurrentUser();
            var guiaRemision = await _repository.GetByIdAsync2(id);
            if (guiaRemision == null)
            {
                throw new KeyNotFoundException("GuiaRemision de remisión no encontrada");
            }
            if(guiaRemision.EstadoId == (int)EnumGuiaRemisionEstado.Anulada)
            {
                throw new InvalidOperationException($"La guía ya se encuentra en estado '{EnumGuiaRemisionEstado.Anulada.GetDescription()}'");
            }

            guiaRemision.EstadoId = (int)EnumGuiaRemisionEstado.Anulada;
            guiaRemision.UpdatedAtUser = currentUser.UserName;
            guiaRemision.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
            guiaRemision.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(guiaRemision);

            // Registrar el historial
            await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
            {
                GuiaRemisionId = guiaRemision.GuiaRemisionId,
                Motivo = "Guía anulada",
                Descripcion = descripcion,
                ValorNuevo = EnumGuiaRemisionEstado.Anulada.GetDescription(),
                UsuarioRegistro = currentUser.UserName,
                UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
                AreaId = currentUser.AreaId,
                FechaRegistro = DateTime.UtcNow
            });

            var guiaRemisionActualizada = await _repository.FindWithDetail(id);
            return _mapper.Map<GuiaRemisionListDTO>(guiaRemisionActualizada);
        }

        public async Task<GuiaRemisionListDTO> RechazarAsync(int id, string descripcion)
        {
            var currentUser = _authService.CurrentUser();
            var guiaRemision = await _repository.GetByIdAsync2(id);
            if (guiaRemision == null)
            {
                throw new KeyNotFoundException("GuiaRemision de remisión no encontrada");
            }
            if (guiaRemision.EstadoId == (int)EnumGuiaRemisionEstado.Rechazada)
            {
                throw new InvalidOperationException($"La guía ya se encuentra en estado '{EnumGuiaRemisionEstado.Rechazada.GetDescription()}'");
            }

            guiaRemision.EstadoId = (int)EnumGuiaRemisionEstado.Rechazada;
            guiaRemision.UpdatedAtUser = currentUser.UserName;
            guiaRemision.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
            guiaRemision.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(guiaRemision);

            // Registrar el historial
            await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
            {
                GuiaRemisionId = guiaRemision.GuiaRemisionId,
                Motivo = "Guía rechazada",
                Descripcion = descripcion,
                ValorNuevo = EnumGuiaRemisionEstado.Rechazada.GetDescription(),
                UsuarioRegistro = currentUser.UserName,
                UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
                AreaId = currentUser.AreaId,
                FechaRegistro = DateTime.UtcNow
            });

            var guiaRemisionActualizada = await _repository.FindWithDetail(id);
            return _mapper.Map<GuiaRemisionListDTO>(guiaRemisionActualizada);
        }




        public async Task<object> AprobarEnviarAsync(int id)
        {

            string ticket = "";
            string estado = "";
            int estadoId = 0;

            await _appDBContext.BeginTransactionAsync();

            try
            {
                // Cambiar a estado aprobado
                var currentUser = _authService.CurrentUser();
                var currentDate = DateTime.UtcNow;
                GuiaRemision guiaRemision = await _repository.GetByIdWithoutRelations(id, strict: true);

                guiaRemision!.EstadoId = (int)EnumGuiaRemisionEstado.Aprobada;
                guiaRemision!.UpdatedAtUser = currentUser.UserName;
                guiaRemision!.UpdatedAtUserName = $"{currentUser.FirstName} {currentUser.LastName}";
                guiaRemision!.UpdatedAt = currentDate;

                await _repository.UpdateAsync(guiaRemision);



                // Registrar el historial
                await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
                {
                    GuiaRemisionId = guiaRemision.GuiaRemisionId,
                    Motivo = "Guía aprobada",
                    ValorNuevo = EnumGuiaRemisionEstado.Aprobada.GetDescription(),
                    UsuarioRegistro = currentUser.UserName,
                    UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
                    AreaId = currentUser.AreaId,
                    FechaRegistro = currentDate
                });



                var guiaExistente = await GetByIdAsync(id);

                if (guiaExistente == null)
                {
                    return new BadRequestResponseDTO
                    {
                        Error = "Guía no encontrada",
                        Detalle = $"No se encontró una guía de remisión con ID {id}."
                    };
                }
                string nombreArchivo;
                if (guiaExistente is GuiaRemitenteDTO guiaRemitente)
                {
                    var (esValido, error) = ValidarDatosGuiaRemitente(guiaRemitente, guiaRemitente.Ruc, null, null, null);
                    if (!esValido)
                    {
                        return new BadRequestResponseDTO
                        {
                            Error = "Error en datos enviados",
                            Detalle = error
                        };
                    }

                    nombreArchivo = await _guiaRemitenteUBLService.GenerarGuiaRemisionJson(guiaRemitente);
                }
                else
                {
                    throw new KeyNotFoundException("Tipo de guía no válido.");
                }


                string efactRawResponse;

                try
                {
                    efactRawResponse = await _envioService.EnviarGuiaElectronicaAsync(guiaRemitente.Ruc, nombreArchivo);
                }
                catch (Exception ex)
                {
                    estado = "ERROR";
                    estadoId = -1;

                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                    await UpdateRespuestaTicketAsync(id, errorMsg, estado, estadoId);
                    return new InternalErrorResponseDTO
                    {
                        Error = "Error al enviar a EFACT",
                        Detalle = errorMsg
                    };
                }

                EfactResponseDTO efactParsed;

                try
                {
                    var json = JsonDocument.Parse(efactRawResponse).RootElement;

                    efactParsed = new EfactResponseDTO
                    {
                        Code = json.GetProperty("code").GetString() ?? "",
                        Description = json.GetProperty("description").GetString() ?? ""
                    };

                    ticket = efactParsed.Description;
                    estado = "ENVIADO";
                    estadoId = 1;
                }
                catch
                {
                    ticket = efactRawResponse.Length > 500 ? efactRawResponse[..500] : efactRawResponse;
                    estado = "ERROR";
                    estadoId = -1;
                    await UpdateRespuestaTicketAsync(id, ticket, estado, estadoId);
                    efactParsed = new EfactResponseDTO
                    {
                        Code = "-1",
                        Description = ticket
                    };
                }

                await UpdateRespuestaTicketAsync(id, ticket, estado, estadoId);

                await EmitirAsync(id);

                await _appDBContext.CommitAsync();

                return new SendGuiaResponseDTO
                {
                    Success = true,
                    EfactResponse = efactParsed
                };

            }
            catch (Exception ex)
            {
                await _appDBContext.RollbackAsync();
                estado = "ERROR";
                estadoId = -1;
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                await UpdateRespuestaTicketAsync(id, errorMsg, estado, estadoId);
                return new InternalErrorResponseDTO
                {
                    Error = "Error inesperado",
                    Detalle = errorMsg
                };
            }
        }


        public async Task<byte[]> GetDocumentAsync(
             int guiaRemisionId, int entityId
        )
        {
            TokenResponseDTO token = await _integrationCredentialService.GetTokenAsync(entityId, "efact");
            GuiaRemitenteDTO guia = _mapper.Map<GuiaRemitenteDTO>(await _repository.GetByIdAsync(guiaRemisionId));

            string guid = await _integrationCredentialService.ObtenerGuidDesdeTicketAsync($"{guia.Ruc}-{guia.TipoGuia}-{guia.SerieCorrelativo}", token.AccessToken);
            byte[] pdfByte = await _integrationCredentialService.ObtenerDocumentoAsync("pdf", guid, token.AccessToken);

            return pdfByte;
        }


        public async Task<byte[]> GetDocumentInternalAsync(int guiaRemisionId, int entityId)
        {
            try
            {
                var guiaRemision = await _repository.GetByIdAsync(guiaRemisionId);
                var document = _guiaRemisionDocumentFactory.Create(guiaRemision);

                return document.GeneratePdf();
            }
            catch (Exception e)
            {
                throw;
            }
            
        }
    }
}
