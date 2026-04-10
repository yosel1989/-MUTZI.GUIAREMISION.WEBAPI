using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.VentaConfig;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;
using System;
using System.Drawing;
using System.Text.Json;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente
{
    public class GuiaRemitenteService : IGuiaRemitenteService
    {
        private readonly IGuiaRemitenteRepository _repository;
        private readonly IVentaConfigRepository _ventaConfigRepository;
        private readonly IMapper _mapper;
        public GuiaRemitenteService(IGuiaRemitenteRepository repository, IVentaConfigRepository ventaConfigRepository, IMapper mapper)
        {
            _repository = repository;
            _ventaConfigRepository = ventaConfigRepository;
            _mapper = mapper;
        }
        public async Task<List<GuiaRemitenteDTO>> GetAllAsync()
        {
            var guiaRemisions = await _repository.GetAllAsync();
            return _mapper.Map<List<GuiaRemitenteDTO>>(guiaRemisions);
        }
        //public async Task<List<GuiaListDTO>> GetListAsync()
        //{
        //    var guiaRemisions = await _repository.GetAllAsync();
        //    return _mapper.Map<List<GuiaListDTO>>(guiaRemisions);
        //}
        //public async Task<PagedResult<GuiaListDTO>> GetListAsync2(int pageNumber, int pageSize)
        //{
        //    var (guias, totalRecords) = await _repository.GetListAsync(pageNumber, pageSize);

        //    var guiasDTO = _mapper.Map<List<GuiaListDTO>>(guias);

        //    return new PagedResult<GuiaListDTO>
        //    {
        //        PageNumber = pageNumber,
        //        PageSize = pageSize,
        //        TotalRecords = totalRecords,
        //        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
        //        Data = guiasDTO
        //    };
        //    throw new NotImplementedException();
        //}
        public async Task<PagedResult<GuiaListDTO>> GetListAsync(int pageNumber, int pageSize, IQueryCollection q)
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

            var guiasDTO = _mapper.Map<List<GuiaListDTO>>(guias);

            return new PagedResult<GuiaListDTO>
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
                var guiasDTO = _mapper.Map<List<GuiaListDTO>>(guias);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte de Guías");

                    string[] headers = {
                        "#", "Código", "Empresa", "RUC Empresa", "Remitente / Local",
                        "Tipo Guía", "N° Guía", "T. Traslado", "T. Transporte",
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
                        worksheet.Cell(row, 2).Value = $"COD-{item.GuiaID.ToString("D4")}";
                        worksheet.Cell(row, 3).Value = item.Empresa;
                        worksheet.Cell(row, 4).Value = item.Ruc;
                        worksheet.Cell(row, 5).Value = item.RazonRemitente;
                        worksheet.Cell(row, 6).Value = item.TipoGuia;
                        worksheet.Cell(row, 7).Value = item.NumeroGuia;
                        worksheet.Cell(row, 8).Value = item.TipoTraslado;
                        worksheet.Cell(row, 9).Value = item.TipoTransporte;
                        worksheet.Cell(row, 10).Value = item.FechaEmision;
                        worksheet.Cell(row, 11).Value = item.HoraEmision;
                        worksheet.Cell(row, 12).Value = item.RazonDestinatario;
                        worksheet.Cell(row, 13).Value = item.NroDocumentoDestinatario;
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

                        //worksheet.Cell(row, 17).Value = item.FechaCreacion?.ToString("dd/MM/yyyy hh:mm:ss tt");
                        worksheet.Cell(row, 17).Value = item.FechaCreacion;
                        worksheet.Cell(row, 18).Value = item.EmpleadoNombreCreacion;

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
        //public async Task<GuiaRemitenteDTO> GetByIdAsync(int id)
        //{
        //    var guiaRemision = await _repository.GetByIdAsync(id);
        //    return _mapper.Map<GuiaRemitenteDTO>(guiaRemision);
        //}
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
            try
            {
                var (esValido, error) = ValidarDatosGuiaRemitente(dto, ruc);
                if (!esValido)
                {
                    return new BadRequestResponseDTO
                    {
                        Error = "Error en datos enviados",
                        Detalle = error
                    };
                }

                GuiaRemitenteDTO guiaCreada;
                try
                {
                    // Mapear todos los objetos
                    var remitente = _mapper.Map<DOMAIN.Entities.Remitente>(dto.Remitente);
                    var destinatario = _mapper.Map<DOMAIN.Entities.Destinatario>(dto.Destinatario);
                    var proveedor = _mapper.Map<DOMAIN.Entities.Proveedor>(dto.Proveedor);
                    var conductor = dto.DatosEnvio.ConductorId.Select(p => _mapper.Map<DOMAIN.Entities.ConductorGuid>(p)).ToList();
                    var transporte = dto.DatosEnvio.TransporteId.Select(p => _mapper.Map<DOMAIN.Entities.TransporteGuid>(p)).ToList();
                    var guiaRemision = _mapper.Map<DOMAIN.Entities.GuiaRemision>(dto);
                    var datosEnvio = _mapper.Map<DOMAIN.Entities.DatosEnvio>(dto.DatosEnvio);
                    var origen = _mapper.Map<DOMAIN.Entities.Origen>(dto.Origen);
                    //var destino = _mapper.Map<DOMAIN.Entities.Destino>(dto.Destino);
                    var destino = dto.Destino.Select(p => _mapper.Map<DOMAIN.Entities.Destino>(p)).ToList();
                    var detalles = dto.Productos.Select(p => _mapper.Map<DOMAIN.Entities.GuiaDetalle>(p)).ToList();
                    var docRelacionados = dto.DocRelacionado?.Select(p => _mapper.Map<DOMAIN.Entities.DocRelacionado>(p)).ToList();
                    var emailDestinatarioGuia = dto.Destinatario?.EmailDestinatarioGuia?.Select(p => _mapper.Map<DOMAIN.Entities.EmailDestinatarioGuid>(p)).ToList();

                    // Llamar al repositorio
                    var (guia_id, numero_guia) = await _repository.CreateGuiaRemisionTransactionalAsync(
                        remitente, destinatario, proveedor, conductor, transporte,
                        guiaRemision, datosEnvio, origen, destino, detalles, docRelacionados, emailDestinatarioGuia
                    );
                    return new GuiaCreatedResponseDTO
                    {
                        GuiaID = guia_id,
                        NumeroGuia = numero_guia,
                        TipoGuia="REMITENTE"
                    };
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];

                    string errorBox = errorMsg.Contains("FOREIGN KEY") ? "Error de clave foránea."
                                    : errorMsg.Contains("UnidadMedida") ? "Unidad de medida no válida."
                                    : errorMsg.Contains("String") ? "Error de longitud de campo."
                                    : "";

                    return new BadRequestResponseDTO
                    {
                        Error = "Error al guardar la guía",
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
                throw new KeyNotFoundException("Guia de remisión no encontrada");
            }
            _mapper.Map(guiaRemisionDto, existingGuiaRemision);
            await _repository.UpdateAsync(existingGuiaRemision);
            return _mapper.Map<GuiaRemitenteDTO>(existingGuiaRemision);
        }
        public async Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado, int idEstado)
        {
            await _repository.UpdateRespuestaTicketAsync(guiaId, ticket, estado, idEstado);
        }
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteCreateDTO guia, string ruc)
        {
            try
            {
                if (guia.DocRelacionado?.Count > 3)
                {
                    return (false, "Solo se permite hasta 3 documentos relacionados como referencia en la guía de remisión transportista.");
                }
                    var ventaConfig = _ventaConfigRepository.GetByRucAsync(ruc).Result;
                if (ventaConfig == null || string.IsNullOrEmpty(ventaConfig.Ruc))
                {
                    return (false, $"El RUC principal ingresado {ruc} no tiene acceso al facturador. Pruebe con el Número de RUC del emisor del documento que se está relacionando al comprobante Guía de remisión remitente.");
                }
                if (guia.Remitente.Ruc != ventaConfig.Ruc)
                    return (false, $"El RUC del remitente ({guia.Remitente.Ruc}) no coincide con el RUC de la empresa ({ventaConfig.Ruc}).");
                _ = guia.TipoTRansporteCode18();
                _ = guia.TipoTrasladoCode20();
                //_ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento, guia.Destinatario.NumeroDocumento);
                //if (guia.Proveedor != null)
                //{
                //    _ = guia.getTipoCodigoDocumento(guia.Proveedor.TipoDocumento, guia.Proveedor.NumeroDocumento);
                //}
                //if (guia.DatosEnvio.Conductor != null)
                //{
                //    foreach (var conductor in guia.DatosEnvio.Conductor)
                //    {
                //        _ = guia.getTipoCodigoDocumento(conductor.TipoDocumento, conductor.NumeroDocumento);
                //    }
                //}
                //Si tipo de transporte es PUBLICO, entonces DatosEnvio.rucEmpresaCurrier debe ser obligatorio
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
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteDTO guia, string ruc)
        {
            try
            {
                if (guia.DocRelacionados?.Count > 3)
                {
                    return (false, "Solo se permite hasta 3 documentos relacionados como referencia en la guía de remisión transportista.");
                }
                var ventaConfig = _ventaConfigRepository.GetByRucAsync(ruc).Result;
                if (ventaConfig == null || string.IsNullOrEmpty(ventaConfig.Ruc))
                {
                    return (false, $"El RUC principal ingresado {ruc} no tiene acceso al facturador. Pruebe con el Número de RUC del emisor del documento que se está relacionando al comprobante Guía de remisión remitente.");
                }
                if (guia.Remitente.Ruc != ventaConfig.Ruc)
                    return (false, $"El RUC del remitente ({guia.Remitente.Ruc}) no coincide con el RUC de la empresa ({ventaConfig.Ruc}).");
                _ = guia.TipoTRansporteCode18();
                _ = guia.TipoTrasladoCode20();
                _ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento, guia.Destinatario.NumeroDocumento);
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
    }
}
