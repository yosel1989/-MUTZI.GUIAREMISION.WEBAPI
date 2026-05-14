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
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Enums;
using MUTZI.GUIA.DOMAIN.Interfaces;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class GuiaRemitenteService : IGuiaRemitenteService
    {
        private readonly IGuiaRemitenteRepository _repository;
        private readonly IGuiaRemisionHistorialRepository _guiaRemisionHistorialRepository;
        private readonly IEstablecimientoRepository _establecimientoRepository;
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IVentaConfigRepository _ventaConfigRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IAppDBContext _appDBContext;
        public GuiaRemitenteService(
            IGuiaRemitenteRepository repository,
            IGuiaRemisionHistorialRepository guiaRemisionHistorialRepository,
            IVentaConfigRepository ventaConfigRepository,
            IEstablecimientoRepository establecimientoRepository,
            IProveedorRepository proveedorRepository,
            IMapper mapper,
            IAuthService authService,
            IAppDBContext appDBContext
        )
        {
            _repository = repository;
            _guiaRemisionHistorialRepository = guiaRemisionHistorialRepository;
            _establecimientoRepository = establecimientoRepository;
            _proveedorRepository = proveedorRepository;
            _ventaConfigRepository = ventaConfigRepository;
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
            var request = new DataTableDTO { Columns = new List<RequestFiltroDTO>() };

            int i = 0;
            while (q.ContainsKey($"columns[{i}][data]"))
            {
                var dataField = q[$"columns[{i}][data]"];
                var searchValue = q[$"columns[{i}][search][value]"];
                var regexValue = q[$"columns[{i}][search][regex]"];

                if (!string.IsNullOrEmpty(searchValue))
                {
                    request.Columns.Add(new RequestFiltroDTO
                    {
                        Data = dataField,
                        Search = new SearchDTO
                        {
                            Value = searchValue,
                            Regex = regexValue
                        }
                    });
                }
                i++;
            }

            var (guias, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize, request);

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
                var request = new DataTableDTO { Columns = new List<RequestFiltroDTO>() };
                int i = 0;
                while (q.ContainsKey($"columns[{i}][data]"))
                {
                    var dataField = q[$"columns[{i}][data]"];
                    var searchValue = q[$"columns[{i}][search][value]"];
                    var regexValue = q[$"columns[{i}][search][regex]"];

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        request.Columns.Add(new RequestFiltroDTO
                        {
                            Data = dataField,
                            Search = new SearchDTO
                            {
                                Value = searchValue,
                                Regex = regexValue
                            }
                        });
                    }
                    i++;
                }

                var (guias, _) = await _repository.GetListAsync(pageNumber, pageSize, request, true);
                var guiasDTO = _mapper.Map<List<GuiaRemisionListDTO>>(guias);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte de Guías");

                    string[] headers = {
                        "#", "Código", "EntidadRemitente", "RUC EntidadRemitente", "Remitente / Local",
                        "Tipo Guía", "N° Guía", "T. Traslado", "T. Transporte",
                        "F. Emisión", "H. Emisión", "Destinatario", "N° Doc. Destinatario",
                        "Origen", "Destino", "Nombre", "F. Registro", "U. Registro"
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
                        worksheet.Cell(row, 3).Value = item.EntidadRemitente;
                        worksheet.Cell(row, 4).Value = item.NumeroDocumentoRemitente;
                        worksheet.Cell(row, 5).Value = item.EstablecimientoRemitente;
                        worksheet.Cell(row, 6).Value = item.TipoGuia;
                        worksheet.Cell(row, 7).Value = item.NumeroGuia;
                        worksheet.Cell(row, 8).Value = item.TipoTraslado;
                        worksheet.Cell(row, 9).Value = item.TipoTransporte;
                        worksheet.Cell(row, 10).Value = item.FechaEmision;
                        worksheet.Cell(row, 11).Value = item.HoraEmision;
                        worksheet.Cell(row, 12).Value = item.EstablecimientoDestinatario;
                        worksheet.Cell(row, 13).Value = item.NumeroDocumentoDestinatario;
                        worksheet.Cell(row, 14).Value = item.DistritoOrigen;
                        worksheet.Cell(row, 15).Value = item.DistritoDestino;

                        var cellEstado = worksheet.Cell(row, 16);
                        cellEstado.Value = item.Estado;
                        cellEstado.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        if (item.Estado?.ToUpper() == "ENVIADO")
                        {
                            cellEstado.Style.Fill.BackgroundColor = XLColor.FromHtml("#C6EFCE");
                            cellEstado.Style.Font.FontColor = XLColor.FromHtml("#006100");
                        }
                        else if (item.Estado?.ToUpper()=="CREADO")
                        {
                            cellEstado.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e7eb");
                        }

                        //worksheet.Cell(row, 17).Value = item.FechaRegistro?.ToString("dd/MM/yyyy hh:mm:ss tt");
                        worksheet.Cell(row, 17).Value = item.FechaRegistro;
                        worksheet.Cell(row, 18).Value = item.UsuarioRegistro;

                        row++;
                    }

                    var headerRange = worksheet.Range(1, 1, 1, headers.Length);
                    headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#DCB2FC"); // Color lila del ejemplo
                    headerRange.Style.Font.FontColor = XLColor.FromHtml("#560591");
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
        public async Task<object> GetByIdAsync(int id)
        {
            var guiaRemision = await _repository.GetByIdAsync(id);

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
        public async Task<object> CreateAsync(string ruc, GuiaRemitenteCreateDTO dto)
        {
            await _appDBContext.BeginTransactionAsync();

            try
            {

                try
                {
                    var currentUser = _authService.CurrentUser();
                    // Mapear todos los objetos
                    var remitente = await _establecimientoRepository.GetByIdAsync(dto.RemitenteId);
                    var destinatario = await _establecimientoRepository.GetByIdAsync(dto.DestinatarioId);
                    var destinatarioEmails = dto.DestinatarioEmail.Select(p => _mapper.Map<EmailDestinatarioGuid>(p)).ToList();
                    var proveedor = await _proveedorRepository.GetByIdAsync(dto.ProveedorId ?? 0);
                    var conductor = dto.DatosEnvio.ConductorId.Select(p => _mapper.Map<ConductorGuid>(p)).ToList();
                    var transporte = dto.DatosEnvio.TransporteId.Select(p => _mapper.Map<UnidadTransporteGuid>(p)).ToList();
                    var guiaRemision = _mapper.Map<GuiaRemision>(dto);
                    var datosEnvio = _mapper.Map<DatosEnvio>(dto.DatosEnvio);
                    var origen = _mapper.Map<GuiaRemisionOrigen>(dto.Origen);
                    var destino = dto.Destino.Select(p => _mapper.Map<GuiaRemisionDestino>(p)).ToList();
                    var detalles = dto.Productos.Select(p => _mapper.Map<GuiaRemisionDetalle>(p)).ToList();
                    var docRelacionados = dto.DocRelacionado?.Select(p => _mapper.Map<DocRelacionado>(p)).ToList();
                    var ventaConfig = await _ventaConfigRepository.GetByRucAsync(ruc);

                    if (remitente is null) throw new Exception("No existe este remitente.");
                    if (destinatario is null) throw new Exception("No existe este destinatario.");
                    if (ventaConfig is null) throw new Exception($"El RUC principal ingresado {ruc} no tiene acceso al facturador. Pruebe con el Número de RUC del emisor del documento que se está relacionando al comprobante Guía de remisión remitente.");

                    var (esValido, error) = ValidarDatosGuiaRemitente(dto, ruc, remitente, destinatario, ventaConfig);
                    if (!esValido)
                    {
                        return new BadRequestResponseDTO
                        {
                            Error = "Error en datos enviados",
                            Detalle = error
                        };
                    }

                    guiaRemision.UsuarioRegistro = currentUser.UserName;
                    guiaRemision.UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}";
                    guiaRemision.FechaRegistro = DateTime.UtcNow;


                    // Llamar al repositorio
                    var (guia_id, numero_guia) = await _repository.CreateGuiaRemisionTransactionalAsync(
                        remitente, destinatario, proveedor, conductor, transporte,
                        guiaRemision, datosEnvio, origen, destino, detalles, docRelacionados, destinatarioEmails
                    );

                    // Registrar el historial
                    await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
                    {
                        GuiaRemisionId = guia_id,
                        Motivo = "Guía registrada",
                        UsuarioRegistro = currentUser.UserName,
                        UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
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
                                    : errorMsg.Contains("UnidadMedida") ? "Unidad de medida no válida."
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
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteCreateDTO guia, string ruc, Establecimiento remitente, Establecimiento destinatario, VentaConfig ventaConfig)
        {
            try
            {
                if (guia.DocRelacionado?.Count > 3)
                {
                    return (false, "Solo se permite hasta 3 documentos relacionados como referencia en la guía de remisión transportista.");
                }

                if (remitente.Ruc != ventaConfig.Ruc)
                    return (false, $"El RUC del remitente ({remitente.Ruc}) no coincide con el RUC de la empresa ({ventaConfig.Ruc}).");
                _ = guia.TipoTRansporteCode18();
                _ = guia.TipoTrasladoCode20();

                if (guia.TipoTransporte=="PUBLICO")
                {
                    if (guia.DatosEnvio.RucEmpresaCurrier.Length != 11) { return (false, "Si tipoTransporte es PUBLICO, entonces debes completar los 11 dígitos de rucEmpresaCurrier."); }
                    if (string.IsNullOrWhiteSpace(guia.DatosEnvio.RazonSocialCurrier)) { return (false, "Si tipoTransporte es PUBLICO, La razonSocialCurrier no puede estar vacía"); }
                    if (string.IsNullOrWhiteSpace(guia.DatosEnvio.RegistroMtcCurrier)) { return (false, "Si tipoTransporte es PUBLICO, el registroMtcCurrier no puede estar vacío"); }
                }
                //if (guia.)
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteDTO guia, string ruc, Establecimiento remitente, Establecimiento destinatario, VentaConfig ventaConfig)
        {
            try
            {
                if (guia.DocRelacionados?.Count > 3)
                {
                    return (false, "Solo se permite hasta 3 documentos relacionados como referencia en la guía de remisión transportista.");
                }
                if (guia.Remitente.Ruc != ruc)
                    return (false, $"El RUC del remitente ({guia.Remitente.Ruc}) no coincide con el RUC de la empresa ({ruc}).");
                _ = guia.TipoTRansporteCode18();
                _ = guia.TipoTrasladoCode20();
                _ = guia.getTipoCodigoDocumento("RUC", guia.Remitente.Ruc);
                //_ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento, guia.Destinatario.NumeroDocumento);

                if (guia.Proveedor != null)
                {
                    _ = guia.getTipoCodigoDocumento(guia.Proveedor.TipoDocumento, guia.Proveedor.NumeroDocumento);
                }
                if (guia.DatosEnvio.ConductorGuia != null)
                {
                    foreach (var conductor in guia.DatosEnvio.ConductorGuia)
                    {
                        _ = guia.getTipoCodigoDocumento(conductor.TipoDocumento, conductor.NumeroDocumento);
                    }
                }
                //Si tipo de transporte es PUBLICO, entonces DatosEnvio.rucEmpresaCurrier debe ser obligatorio
                if (guia.TipoTransporte == "PUBLICO")
                {
                    if (guia.DatosEnvio.RucEmpresaCurrier.Length != 11) { return (false, "Si tipoTransporte es PUBLICO, entonces debes completar los 11 dígitos de rucEmpresaCurrier."); }
                    if (string.IsNullOrWhiteSpace(guia.DatosEnvio.RazonSocialCurrier)) { return (false, "Si tipoTransporte es PUBLICO, La razonSocialCurrier no puede estar vacía"); }
                    if (string.IsNullOrWhiteSpace(guia.DatosEnvio.RegistroMtcCurrier)) { return (false, "Si tipoTransporte es PUBLICO, el registroMtcCurrier no puede estar vacío"); }
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

        public async Task<GuiaRemisionListDTO> ConfirmarAsync(int id)
        {
            var currentUser = _authService.CurrentUser();
            var guiaRemision = await _repository.GetByIdAsync2(id);
            if (guiaRemision == null)
            {
                throw new KeyNotFoundException("GuiaRemision de remisión no encontrada");
            }

            var estado = EnumGuiaRemisionEstado.Confirmada;
            guiaRemision.IdEstado = (int)EnumGuiaRemisionEstado.Confirmada;
            guiaRemision.UsuarioModifico = currentUser.UserName;
            guiaRemision.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}";
            guiaRemision.FechaModifico = DateTime.UtcNow;
            
            await _repository.UpdateAsync(guiaRemision);

            // Registrar el historial
            await _guiaRemisionHistorialRepository.RegistrarAsync(new GuiaRemisionHistorial
            {
                GuiaRemisionId = guiaRemision.GuiaRemisionId,
                Motivo = "Guía confirmada",
                ValorNuevo = EnumGuiaRemisionEstado.Confirmada.GetDescription(),
                UsuarioRegistro = currentUser.UserName,
                UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
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

            var anteriorValor = guiaRemision.Estado;

            guiaRemision.IdEstado = (int)EnumGuiaRemisionEstado.Anulada;
            guiaRemision.UsuarioModifico = currentUser.UserName;
            guiaRemision.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}";
            guiaRemision.FechaModifico = DateTime.UtcNow;

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

            var anteriorValor = guiaRemision.Estado;

            guiaRemision.IdEstado = (int)EnumGuiaRemisionEstado.Rechazada;
            guiaRemision.UsuarioModifico = currentUser.UserName;
            guiaRemision.UsuarioModificoNombre = $"{currentUser.FirstName} {currentUser.LastName}";
            guiaRemision.FechaModifico = DateTime.UtcNow;

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
                FechaRegistro = DateTime.UtcNow
            });

            var guiaRemisionActualizada = await _repository.FindWithDetail(id);
            return _mapper.Map<GuiaRemisionListDTO>(guiaRemisionActualizada);
        }
    }
}
