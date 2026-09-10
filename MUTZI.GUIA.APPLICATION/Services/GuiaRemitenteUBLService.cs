using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Enums;
using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using Newtonsoft.Json;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class GuiaRemitenteUBLService : IGuiaRemitenteUBLService
    {
        private readonly IConfiguration _config;
        private readonly IS3FileService _s3FileService;
        private readonly string _baseUrl = "";
        public GuiaRemitenteUBLService(IConfiguration config, IS3FileService s3FileService)
        {
            _config = config;
            _s3FileService = s3FileService;
            _baseUrl = _config["Aws:BaseUrl"] ?? "";
        }
        public async Task<string> GenerarGuiaRemisionJson(GuiaRemitenteDTO guia)
        {
            var root = new GuiaRemisionRemitente()
            {
                _D = EspacioNombreGuiaRemision._D,
                _A = EspacioNombreGuiaRemision._A,
                _B = EspacioNombreGuiaRemision._B,
                _E = EspacioNombreGuiaRemision._E
            };

            root.DespatchAdvice = new List<DespatchAdvice> {
                new DespatchAdvice {

                    // Versión del UBL
                    UBLVersionID = new List<UBLVersionID>{ new UBLVersionID { _ =EspacioNombreGuiaRemision._UBLVersionID }},

                    // Versión de la estructura del documento
                    CustomizationID = new List<CustomizationID>{ new CustomizationID {  _ = EspacioNombreGuiaRemision._CustomizationID }},
                    
                    // Serie y correlativo
                    ID = new List<ID> { new ID { _ = guia.SerieCorrelativo }},

                    // Fecha de emisión
                    IssueDate = new List<IssueDate> { new IssueDate { _ = guia.Fecha }},

                    // Hora de emisión
                    IssueTime = new List<IssueTime> { new IssueTime { _ = guia.Hora }},

                    // catálogo N°01 del anexo 8 de la SUNAT, 09 Guía de remisión remitente
                    // Código del tipo de documento
                    DespatchAdviceTypeCode = new List<DespatchAdviceTypeCode>{ new DespatchAdviceTypeCode { _ = EspacioNombreGuiaRemision._DespatchAdviceTypeCode }},
                    
                    // Cantidad de items
                    LineCountNumeric = new List<LineCountNumeric> { new LineCountNumeric { _ = guia.Productos.Count.ToString() }},
                    
                    //Agregar en lista los documentos relacionados (FACTURA Y BOLETA por el momento, Catalogo N° 61)
                    AdditionalDocumentReference = guia.DocRelacionados.Select(docRelacionado => new AdditionalDocumentReference {
                        // Número del documento relacionado
                        ID = new List<ID>{ new ID { _ = docRelacionado.NumeroDocumentoReferencia }},

                        // Código del tipo de documento relacionado
                        DocumentTypeCode = new List<DocumentTypeCode>{ new DocumentTypeCode { _ = docRelacionado.DocumentoAdicionalTipo() }},
                        
                        // Descripción del tipo de documento relacionado
                        DocumentType=new List<DocumentType>{new DocumentType { _= docRelacionado.TipoDocumentoReferencia } },

                        // Número de RUC del emisor del documento relacionado
                        IssuerParty = new List<IssuerParty>{ new IssuerParty {
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification {
                                ID = new List<ID>{ new ID { 
                                    // Número de documento
                                    _ = docRelacionado.RucDocumentoReferencia , 
                                    // Tipo de documento
                                    schemeID = EspacioNombreGuiaRemision._DocumentType06 
                                }}
                            }}
                        }}
                    }).ToList(),


                    // Firma

                    Signature = guia.MotivoTrasladoCodigo != "07" ? new List<Signature>{ new Signature{
                        ID = new List<ID>{ new ID { _ = "IDSignature" } },
                        SignatoryParty = new List<SignatoryParty>{ new SignatoryParty {
                            // Ruc del remitente
                            PartyIdentification= new List<PartyIdentification>{ new PartyIdentification { ID = new List<ID> { new ID {  _ = guia.Remitente.EntityDocumentNumber } } }},
                            // Razón social del remitente
                            PartyName = new List<PartyName>{ new PartyName { Name = new List<Name> { new Name {  _ = guia.Remitente.EntityName} } }},
                        }},
                        DigitalSignatureAttachment = new List<DigitalSignatureAttachment>{ new DigitalSignatureAttachment {
                            ExternalReference = new List<ExternalReference>{new ExternalReference { URI = new List<URI> { new URI() {_ = "IDSignature" }}}}
                        }}
                    }} :
                    new List<Signature>{ new Signature{
                        ID = new List<ID>{ new ID { _ = "IDSignature" } },
                        SignatoryParty = new List<SignatoryParty>{ new SignatoryParty {
                            // Ruc del remitente
                            PartyIdentification= new List<PartyIdentification>{ new PartyIdentification { ID = new List<ID> { new ID {  _ = guia.Destinatario.EntityDocumentNumber } } }},
                            // Razón social del remitente
                            PartyName = new List<PartyName>{ new PartyName { Name = new List<Name> { new Name {  _ = guia.Destinatario.EntityName} } }},
                        }},
                        DigitalSignatureAttachment = new List<DigitalSignatureAttachment>{ new DigitalSignatureAttachment {
                            ExternalReference = new List<ExternalReference>{new ExternalReference { URI = new List<URI> { new URI() {_ = "IDSignature" }}}}
                        }}
                    }},


                    // Datos del remitente
                    DespatchSupplierParty = guia.MotivoTrasladoCodigo != "07" ? new List<DespatchSupplierParty>{ new DespatchSupplierParty {
                        Party = new List<Party>{ new Party {

                            // Código SUNAT del tipo de documento de identidad y numeró de documento del remitente
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification { ID  = new List<ID> { new ID { 
                                // Número de documento de identidad del remitente
                                _ = guia.Remitente.EntityDocumentNumber,  
                                // Código del tipo de documento de identidad del remitente 
                                schemeID = EspacioNombreGuiaRemision._DocumentType06  
                            }}}},

                            // Domicilio Fiscal remitente
                            PostalAddress = new List<PostalAddress>{ new PostalAddress {
                                // Código de Ubigeo
                                ID = new List<ID> { new ID { _ = guia.Remitente.UbigeoId } },
                                // Departamento
                                CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = guia.Remitente.Department } },
                                // Provincia
                                CityName = new List<CityName>{ new CityName { _ = guia.Remitente.Province} },
                                // Dirección completa y detallada
                                StreetName = new List<StreetName>{ new StreetName { _ = guia.Remitente.Address } },
                                // Urbanización
                                CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Remitente.Department } },
                                // Distrito
                                District= new List<District>{ new District { _ = guia.Remitente.District } },
                                // Código de país
                                Country = new List<Country>{ new Country {  IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Remitente.EntityCountry } }}}
                            }},

                            // Apellidos y nombres, denominación o razón social del remitente
                            PartyLegalEntity = new List<PartyLegalEntity>{ 
                                new PartyLegalEntity { 
                                    RegistrationName = new List<RegistrationName> { 
                                        new RegistrationName { _  = guia.Remitente.EntityName }}
                                }},

                            // 
                            /*Contact = guia.EmailFacturador == null ? null : new List<Contact>
                            {
                                new Contact
                                {
                                    ElectronicMail=new List<ElectronicMail>
                                    {
                                        new ElectronicMail
                                        {
                                            _ = guia.EmailFacturador
                                        }
                                    }
                                }
                            }*/
                        }},
                    }} :
                    new List<DespatchSupplierParty>{ new DespatchSupplierParty {
                        Party = new List<Party>{ new Party {

                            // Código SUNAT del tipo de documento de identidad y numeró de documento del remitente
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification { ID  = new List<ID> { new ID { 
                                // Número de documento de identidad del remitente
                                _ = guia.Destinatario.EntityDocumentNumber,  
                                // Código del tipo de documento de identidad del remitente 
                                schemeID = EspacioNombreGuiaRemision._DocumentType06
                            }}}},

                            // Domicilio Fiscal remitente
                            PostalAddress = new List<PostalAddress>{ new PostalAddress {
                                // Código de Ubigeo
                                ID = new List<ID> { new ID { _ = guia.Destinatario.UbigeoId } },
                                // Departamento
                                CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = guia.Destinatario.Department } },
                                // Provincia
                                CityName = new List<CityName>{ new CityName { _ = guia.Destinatario.Province} },
                                // Dirección completa y detallada
                                StreetName = new List<StreetName>{ new StreetName { _ = guia.Destinatario.Address } },
                                // Urbanización
                                CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Destinatario.Department } },
                                // Distrito
                                District= new List<District>{ new District { _ = guia.Destinatario.District } },
                                // Código de país
                                Country = new List<Country>{ new Country {  IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Destinatario.EntityCountry } }}}
                            }},

                            // Apellidos y nombres, denominación o razón social del remitente
                            PartyLegalEntity = new List<PartyLegalEntity>{
                                new PartyLegalEntity {
                                    RegistrationName = new List<RegistrationName> {
                                        new RegistrationName { _  = guia.Destinatario.EntityName }}
                                }},

                            // 
                            /*Contact = guia.EmailFacturador == null ? null : new List<Contact>
                            {
                                new Contact
                                {
                                    ElectronicMail=new List<ElectronicMail>
                                    {
                                        new ElectronicMail
                                        {
                                            _ = guia.EmailFacturador
                                        }
                                    }
                                }
                            }*/
                        }},
                    }},

                    // Datos del Destinatario
                    DeliveryCustomerParty = new List<DeliveryCustomerParty>{ new DeliveryCustomerParty {
                        Party = new List<Party>{ new Party {

                            // Código SUNAT del tipo de documento de identidad y numeró de documento del destinatario
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification {
                                ID = new List<ID>{ new ID { 
                                    // Número de documento de identidad del destinatario
                                    _ = guia.Destinatario.EntityDocumentNumber, 
                                    // Código del tipo de documento de identidad del destinatario
<<<<<<< HEAD
                                    schemeID = guia.getTipoCodigoDocumento("RUC", guia.Destinatario.EntityName)  
=======
                                    schemeID = guia.getTipoCodigoDocumento("RUC", guia.Destinatario.EntityDocumentNumber)  
>>>>>>> 36e910039164c0a62d67b3e4d04ae8dc2c9f2b69
                                }}
                            }},

                            // Domicilio Fiscal destinatario
                            PostalAddress = new List<PostalAddress>{ new PostalAddress {
                                // Código de ubigeo
                                ID = new List<ID> { new ID { _ = guia.Destinatario.UbigeoId}},
                                // Dirección completa y detallada
                                StreetName = new List<StreetName>{ new StreetName { _ = guia.Destinatario.Address } },
                                // Urbanizacion
                                CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Destinatario.Department } },
                                // Provincia
                                CityName = new List<CityName>{ new CityName { _ = guia.Destinatario.Province } },
                                // Departamento
                                CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = guia.Destinatario.Department } },
                                // Distrito
                                District= new List<District>{ new District { _ = guia.Destinatario.District } },
                                // Codigo de Pais
                                Country = new List<Country>{ new Country {  IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Destinatario.EntityCountry } }}}
                            }},

                            // Apellidos y nombres, denominación o razón social del destinatario
                            PartyLegalEntity = new List<PartyLegalEntity>{ new PartyLegalEntity {
                                RegistrationName = new List<RegistrationName>{ new RegistrationName { _ = guia.Destinatario.EntityName } }
                            }},
                            
                            // Correo del destinatario
                            Contact = guia.DestinatarioCorreo.Count > 0 ? new List<Contact>
                            {
                                new Contact
                                {
                                    ElectronicMail = new List<ElectronicMail>
                                    {
                                        new ElectronicMail
                                        {
                                            _ = string.Join(",", guia.DestinatarioCorreo.Select(email => email.Email))
                                        }
                                    }
                                }
                            } : null
                        }}
                    }},

                    // Datos del Proveedor (Solo se registra si el motivo de traslado es COMPRA)
                    SellerSupplierParty = (guia.MotivoTrasladoId == (int)SunatCatalogoEnum.MotivoTraslado.COMPRA) ? new List<SellerSupplierParty>{ new SellerSupplierParty {
                        Party = new List<Party>{ new Party {

                            // Número y codigo del tipo de documento de identidad del proveedor
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification { ID  = new List<ID> { new ID { 
                                // Número de documento de identidad del proveedor
                                _ = guia.Proveedor.NumeroDocumento,  
                                // Código del tipo de documento de identidad del proveedor
                                schemeID = guia.Proveedor.TipoDocumentoCodigoSunat
                            }}}},

                            // Domicilio Fiscal proveedor
                            PostalAddress = new List<PostalAddress>{ new PostalAddress {
                                // Código de ubigeo
                                ID = new List<ID> { new ID { _ = guia.Proveedor.UbigeoId } },
                                // Dirección completa y detallada
                                StreetName = new List<StreetName>{ new StreetName { _ = guia.Proveedor.Direccion } },
                                // Urbanizacion
                                CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Proveedor.Departamento } },
                                // Provincia
                                CityName = new List<CityName>{ new CityName { _ = guia.Proveedor.Provincia} },
                                // Departamento
                                CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = guia.Proveedor.Departamento } },
                                // Distrito
                                District= new List<District>{ new District { _ = guia.Proveedor.Distrito } },
                                // Codigo de Pais
                                Country = new List<Country>{ new Country {  IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Proveedor.Pais } }}}
                            }},

                            // Apellidos y nombres, denominación o razón social del proveedor
                            PartyLegalEntity = new List<PartyLegalEntity>{ new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _  = guia.Proveedor.RazonSocial }}}}
                        }},
                    }} : null,

                    DespatchLine = new List<DespatchLine>{}
                }
            };

            // Datos del envío
            root.DespatchAdvice[0].Shipment = new List<Shipment> { new Shipment {
                // Identificador del traslado
                ID = new List<ID>{ new ID { _ = "SUNAT_Envio" }},

                // Código SUNAT del Motivo de Traslado
                HandlingCode = new List<HandlingCode>{ new HandlingCode { _ = guia.MotivoTrasladoCodigo }},

                // Descripción del motivo de traslado
                HandlingInstructions = new List<HandlingInstruction>{ new HandlingInstruction { _ = guia.DatosEnvio.MotivoEnvio }},

                // Peso bruto total de la carga y unidad de medida del peso bruto
                GrossWeightMeasure = new List<GrossWeightMeasure>{ new GrossWeightMeasure { 
                    // Peso bruto total de la carga
                    _ =guia.DatosEnvio.PesoBruto.ToString() ,
                    // Código SUNAT Unidad de Medida
                    unitCode = guia.DatosEnvio?.CodigoUm ?? "KGM" 
                }},
                    
                // Indicadores
                // Indicador deTransbordo Programado
                // Indicador de traslado en vehículos de categoría M1 o L
                // Indicador de retorno de vehículo con envases o embalajes vacíos
                // Indicador de retorno de vehículo vacío
                // Indicador de traslado total de la DAM o la DS
                // Indicador de registro de vehículos y conductores del transportista (de acuerdo con el último párrafo del numeral 3.1. del artículo 3) de la RS N° 255-2015-SUNAT)
                SpecialInstructions = new[]
                {
                    guia.DatosEnvio?.IndicadorTransbordoProgramado == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorTransbordoProgramado" }
                        : null,
                    guia.DatosEnvio?.IndicadorTrasladoVehiculoCategoria == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorTrasladoVehiculoM1L" }
                        : null,
                    guia.DatosEnvio?.IndicadorRetornoVehiculoEnvasesVacios == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorRetornoVehiculoEnvaseVacio" }
                        : null,
                    guia.DatosEnvio?.IndicadorRetornoVehiculoVacio == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorRetornoVehiculoVacio" }
                        : null,
                    guia.DatosEnvio?.IndicadorTrasladoTotalDam == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorTrasladoTotalDAMoDS" }
                        : null,
                    guia.DatosEnvio?.IndicadorRegistroVehiculoConductor == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorVehiculoConductoresTransp" }
                        : null,
                }
                .Where(x => x != null)
                .Select(y => new SpecialInstructions { _ = y._ })
                .ToList().Count() > 0 ? new[]
                {
                    guia.DatosEnvio?.IndicadorTransbordoProgramado == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorTransbordoProgramado" }
                        : null,
                    guia.DatosEnvio?.IndicadorTrasladoVehiculoCategoria == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorTrasladoVehiculoM1L" }
                        : null,
                    guia.DatosEnvio?.IndicadorRetornoVehiculoEnvasesVacios == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorRetornoVehiculoEnvaseVacio" }
                        : null,
                    guia.DatosEnvio?.IndicadorRetornoVehiculoVacio == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorRetornoVehiculoVacio" }
                        : null,
                    guia.DatosEnvio?.IndicadorTrasladoTotalDam == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorTrasladoTotalDAMoDS" }
                        : null,
                    guia.DatosEnvio?.IndicadorRegistroVehiculoConductor == true
                        ? new SpecialInstructions { _ = "SUNAT_Envio_IndicadorVehiculoConductoresTransp" }
                        : null,
                }
                .Where(x => x != null)
                .Select(y => new SpecialInstructions { _ = y._ })
                .ToList() : null,

                // Modalidad de traslado
                ShipmentStage = new List<ShipmentStage>{ new ShipmentStage {

                    // Tipo de transporte (PRIVADA o PUBLICA) - Modalidad de traslado
                    TransportModeCode = new List<TransportModeCode>{ new TransportModeCode { _ = guia.TipoTRansporteCode18() }},

                    // Fecha Inicio de Traslado y Fecha de entrega de bienes al transportista
                    TransitPeriod = new List<TransitPeriod>{ new TransitPeriod {
                        StartDate = new List<StartDate>{ new StartDate { _ = guia.DatosEnvio?.FechaEnvio != null ? guia.DatosEnvio!.FechaEnvio! : guia.DatosEnvio!.FechaEntregaTransportista! }}
                    }},
                }},

                // Dirección del punto de partida y dirección del punto de llegada, así como los datos del transportista (si el traslado se realiza en vehículos de terceros) y del conductor (si el traslado se realiza en vehículos de terceros o propios)
                Delivery = new List<Delivery>{ new Delivery {


                    // Dirección del punto de partida
                    Despatch = guia.MotivoTrasladoCodigo != "07" ? new List<Despatch>{ new Despatch {
                         DespatchAddress = new List<DespatchAddress>{ new DespatchAddress {
                            // Código ubigeo del punto de partida
                            ID = new List<ID>{ new ID { _ = guia.Origen.UbigeoId }},
                            // Dirección completa y detallada de partida
                            AddressLine = new List<AddressLine>{ new AddressLine { Line = new List<Line> { new Line { _  = guia.Origen.Direccion }}}},
                            // Se valida si tipoTrasLado es compra o no.
                            // Asimismo, se valida que no sea nulo el codSunat.
                            AddressTypeCode = ( guia.MotivoTrasladoCodigo == SunatCatalogoEnum.MotivoTraslado.COMPRA.GetDescription() ) ? string.IsNullOrWhiteSpace(guia.Proveedor.CodigoSunat) ? null : new List<AddressTypeCode>{ new AddressTypeCode {
                                // El código sunat del proveedor , se coloca aquí porque en el caso de compra, el traslado se realiza desde el proveedor hacia la empresa, por lo tanto el codSunat del proveedor va en el punto de partida, y el codSunat del destinatario (cliente) va en el punto de llegada.
                                _ = guia.Proveedor.CodigoSunat,
                                // El número de documento del proveedor, se coloca aquí porque en el caso de compra, el traslado se realiza desde el proveedor hacia la empresa, por lo tanto el número de documento del proveedor va en el punto de partida, y el número de documento del destinatario (cliente) va en el punto de llegada.
                                listID = guia.Proveedor.NumeroDocumento
                            }} : 
                            //En caso que no sea compra, se valida el codSunat del remitente 
                            string.IsNullOrWhiteSpace(guia.Remitente.CodeSunat) ? null: new List<AddressTypeCode>{new AddressTypeCode {
                                // El código sunat del remitente , se coloca aquí porque en el caso que no sea compra, el traslado se realiza desde la empresa hacia otro punto, por lo tanto el codSunat del remitente va en el punto de partida, y el codSunat del destinatario (cliente) va en el punto de llegada.
                                _ = guia.Remitente.CodeSunat,
                                // El número de documento del remitente, se coloca aquí porque en el caso que no sea compra, el traslado se realiza desde la empresa hacia otro punto, por lo tanto el número de documento del remitente va en el punto de partida, y el número de documento del destinatario (cliente) va en el punto de llegada.
                                listID = guia.Remitente.EntityDocumentNumber
                            }},
                            // Urbanizacion
                            CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Origen.Departamento } },
                            // Provincia
                            CityName = new List<CityName>{ new CityName { _ = guia.Origen.Provincia }},
                            // Departamento
                            CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _= guia.Origen.Departamento }},
                            // Distrito
                            District = new List<District>{ new District { _ = guia.Origen.Distrito}},
                            // Codigo de Pais
                            Country = new List<Country>{ new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Origen.Pais } }}}
                         }},
                    }} :
                    new List<Despatch>{ new Despatch {
                         DespatchAddress = new List<DespatchAddress>{ new DespatchAddress {
                            // Código ubigeo del punto de partida
                            ID = new List<ID>{ new ID { _ = guia.Proveedor.UbigeoId }},
                            // Dirección completa y detallada de partida
                            AddressLine = new List<AddressLine>{ new AddressLine { Line = new List<Line> { new Line { _  = guia.Proveedor.Direccion }}}},
                            // Se valida si tipoTrasLado es compra o no.
                            // Asimismo, se valida que no sea nulo el codSunat.
                            AddressTypeCode = new List<AddressTypeCode>{ new AddressTypeCode {
                                // El código sunat del proveedor , se coloca aquí porque en el caso de compra, el traslado se realiza desde el proveedor hacia la empresa, por lo tanto el codSunat del proveedor va en el punto de partida, y el codSunat del destinatario (cliente) va en el punto de llegada.
                                _ = guia.Proveedor.CodigoSunat,
                                // El número de documento del proveedor, se coloca aquí porque en el caso de compra, el traslado se realiza desde el proveedor hacia la empresa, por lo tanto el número de documento del proveedor va en el punto de partida, y el número de documento del destinatario (cliente) va en el punto de llegada.
                                listID = guia.Proveedor.NumeroDocumento
                            }},
                            // Urbanizacion
                            CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Proveedor.Departamento } },
                            // Provincia
                            CityName = new List<CityName>{ new CityName { _ = guia.Proveedor.Provincia }},
                            // Departamento
                            CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _= guia.Proveedor.Departamento }},
                            // Distrito
                            District = new List<District>{ new District { _ = guia.Proveedor.Distrito}},
                            // Codigo de Pais
                            Country = new List<Country>{ new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Proveedor.Pais } }}}
                         }},
                    }},

                    // Dirección del punto de llegada
                    DeliveryAddress = guia.Destino.Select(destino => new DeliveryAddress {
                        // Código ubigeo del punto de llegada
                        ID = new List<ID>{ new ID { _ = destino.UbigeoId }},
                        // Dirección completa y detallada de llegada
                        AddressLine = new List<AddressLine>{ new AddressLine { Line = new List<Line> { new Line { _ = destino.Direccion }}}},

                        AddressTypeCode = string.IsNullOrWhiteSpace(guia.Destinatario.CodeSunat) ? null : new List<AddressTypeCode>{ new AddressTypeCode {
                            // Código de establecimiento de punto de llegada
                            _ = guia.Destinatario.CodeSunat,
                            // Número de RUC asociado al punto de llegada
                            //Si es traslado, se coloca el ruc del remitente, ya que es traslado entre sucursales nomas; si es venta o compra, el numeroDoc del destinatario
                            //listID = guia.TipoTraslado=="TRASLADO"? guia.Remitente.NumeroDocumentoRemitente : guia.Destinatario.NumeroDocumento
                            listID = guia.Destinatario.EntityDocumentNumber
                        }},
                        // Urbanización
                        CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = destino.Departamento }},
                        // Departamento
                        CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = destino.Departamento }},
                        // Provincia
                        CityName = new List<CityName>{ new CityName { _ = destino.Provincia }},
                        // Distrito
                        District = new List<District>{ new District { _ = destino.Distrito }},
                        // Código de país
                        Country = new List<Country>{ new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = destino.Pais } }}}
                    }).ToList(),

                }},
            }};


            //////////if (guia.TipoTransporte == "PRIVADO")
            //////////{
            // Conductor en Lista
            // Datos del conductor (si el traslado se realiza en vehículos de terceros o propios)
            if (guia.DatosEnvio.GuiaRemisionConductores != null)
            {
                root.DespatchAdvice[0].Shipment[0].ShipmentStage[0].DriverPerson = guia.DatosEnvio.GuiaRemisionConductores.Select((conductor, i) => new DriverPerson
                {
                    // Tipo de Conductor (Principal o Secundario)
                    JobTitle = 
                        (guia.TipoTransporte == "PUBLICO" && guia.DatosEnvio.IndicadorRegistroVehiculoConductor) ? 
                        new List<JobTitle> { new JobTitle { _ = i == 0 ? "Principal" : "Secundario" } } :
                        new List<JobTitle> { new JobTitle { _ = i == 0 ? "Principal" : "Secundario" } }
                    ,


                    // Número y código SUNAT del tipo de documento de identidad del conductor
                    ID = new List<ID> { new ID { 
                        // Número de documento de identidad del conductor
                        _ = conductor.NumeroDocumento, 
                        // Código SUNAT del tipo de documento de identidad del conductor
                        schemeID = conductor.TipoDocumentoCodigoSunat
                    }},
                    // Nombres del conductor
                    FirstName = new List<FirstName> { new FirstName { _ = conductor.Nombres } },
                    // Apellidos del conductor
                    FamilyName = new List<FamilyName> { new FamilyName { _ = conductor.Apellidos } },
                    // Número de licencia de conducir del conductor
                    IdentityDocumentReference = new List<IdentityDocumentReference>{ new IdentityDocumentReference {
                        ID = new List<ID> { new ID { _ = conductor.Licencia  }}
                    }}
                }).ToList();
            }

            // Unidades de transporte en Lista
            // Datos de las unidades de transporte (si el traslado se realiza en vehículos de terceros o propios)
            if (guia.DatosEnvio.GuiaRemisionUnidadesTransporte != null && guia.DatosEnvio.GuiaRemisionUnidadesTransporte.Count() > 0)
            {
                var transportes = guia.DatosEnvio.GuiaRemisionUnidadesTransporte.ToList();
                var transportEquipments = new List<TransportEquipment>();

                // Si es modo PRIVADO
                if (guia.TipoTransporte == "PRIVADO")
                {

                    // Vehículo Principal
                    // Primer transporte como TransportEquipment principal
                    transportEquipments.Add(new TransportEquipment
                    {
                        // Número de placa del vehículo
                        ID = new List<ID> { new ID { _ = transportes[0]!.Placa } },
                        // Tarjeta Única de Circulación Electrónica o Certificado de Habilitación vehicular
                        ApplicableTransportMeans = guia.TipoTransporte != "PUBLICO" ? null : new List<ApplicableTransportMean>{ new ApplicableTransportMean {
                            //Tarjeta  IS ES PRIVADO NO VA??
                            RegistrationNationalityID = new List<RegistrationNationalityID>{ new RegistrationNationalityID { _ = transportes[0]!.Tarjeta }}
                        }},
                        // Entidad emisora de la autorización especial -vehículo principal
                        // Número de autorización especial del vehículo emitido por la entidad -vehículo principal
                        ShipmentDocumentReference = string.IsNullOrWhiteSpace(transportes[0]!.NroAutorizacion) ? null : new List<ShipmentDocumentReference>
                        {
                            new ShipmentDocumentReference
                            {
                                ID = new List<ID>
                                {
                                    new ID
                                    {
                                        // Número de autorización
                                        _ = transportes[0]!.NroAutorizacion,
                                        // Código de entidad emisora
                                        schemeID = transportes[0]!.CodEmisorVehicular
                                    }
                                }
                            }
                        },
                    });

                    // Los siguientes como TransportEquipment con AttachedTransportEquipment
                    if (transportes.Count > 1)
                    {
                        foreach (var t in transportes.Skip(1))
                        {
                            transportEquipments.Add(new TransportEquipment
                            {
                                AttachedTransportEquipment = new List<AttachedTransportEquipment>
                                {
                                    new AttachedTransportEquipment
                                    {
                                        // Número de placa del vehículo
                                        ID = new List<ID> { new ID { _ = t.Placa } },
                                        // Tarjeta Única de Circulación Electrónica o Certificado de Habilitación vehicular
                                        // Si el tipo de transporte no es público, no se registra la tarjeta, ya que la tarjeta es un requisito solo para transporte público.
                                        ApplicableTransportMeans = guia.TipoTransporte != "PUBLICO" ? null : new List<ApplicableTransportMean>{ new ApplicableTransportMean {//Tarjeta  IS ES PRIVADO NO VA??
                                            RegistrationNationalityID = new List<RegistrationNationalityID>{ new RegistrationNationalityID { _ = t.Tarjeta }}
                                        }},
                                        // Entidad emisora de la autorización especial - vehículo secundario
                                        // Número de autorización especial del vehículo emitido por la entidad - vehículo secundario
                                        ShipmentDocumentReference = string.IsNullOrWhiteSpace(t.NroAutorizacion) ? null : new List<ShipmentDocumentReference>
                                        {
                                            new ShipmentDocumentReference
                                            {
                                                ID = new List<ID>
                                                {
                                                    new ID
                                                    {
                                                        // Número de autorización
                                                        _ = t.NroAutorizacion,
                                                        // Código de entidad emisora
                                                        schemeID = t.CodEmisorVehicular
                                                    }
                                                }
                                            }
                                        },
                                    }
                                }
                            });
                        }
                    }

                }
                else
                {
                    if (guia.DatosEnvio.IndicadorRegistroVehiculoConductor)
                    {
                        // Vehículo Principal
                        // Primer transporte como TransportEquipment principal
                        transportEquipments.Add(new TransportEquipment
                        {
                            // Número de placa del vehículo
                            ID = new List<ID> { new ID { _ = transportes[0]!.Placa } },
                            // Tarjeta Única de Circulación Electrónica o Certificado de Habilitación vehicular
                            //ApplicableTransportMeans = guia.TipoTransporte != "PUBLICO" ? null : new List<ApplicableTransportMean>{ new ApplicableTransportMean {
                                //Tarjeta  IS ES PRIVADO NO VA??
                            //    RegistrationNationalityID = new List<RegistrationNationalityID>{ new RegistrationNationalityID { _ = transportes[0]!.Tarjeta }}
                            //}},
                            // Entidad emisora de la autorización especial -vehículo principal
                            // Número de autorización especial del vehículo emitido por la entidad -vehículo principal
                            ShipmentDocumentReference = string.IsNullOrWhiteSpace(transportes[0]!.NroAutorizacion) ? null : new List<ShipmentDocumentReference>
                        {
                            new ShipmentDocumentReference
                            {
                                ID = new List<ID>
                                {
                                    new ID
                                    {
                                        // Número de autorización
                                        _ = transportes[0]!.NroAutorizacion,
                                        // Código de entidad emisora
                                        schemeID = transportes[0]!.CodEmisorVehicular
                                    }
                                }
                            }
                        },
                        });

                        // Los siguientes como TransportEquipment con AttachedTransportEquipment
                        if (transportes.Count > 1)
                        {
                            foreach (var t in transportes.Skip(1))
                            {
                                transportEquipments.Add(new TransportEquipment
                                {
                                    AttachedTransportEquipment = new List<AttachedTransportEquipment>
                                {
                                    new AttachedTransportEquipment
                                    {
                                        // Número de placa del vehículo
                                        ID = new List<ID> { new ID { _ = t.Placa } },
                                        // Tarjeta Única de Circulación Electrónica o Certificado de Habilitación vehicular
                                        // Si el tipo de transporte no es público, no se registra la tarjeta, ya que la tarjeta es un requisito solo para transporte público.
                                        //ApplicableTransportMeans = guia.TipoTransporte != "PUBLICO" ? null : new List<ApplicableTransportMean>{ new ApplicableTransportMean {//Tarjeta  IS ES PRIVADO NO VA??
                                        //    RegistrationNationalityID = new List<RegistrationNationalityID>{ new RegistrationNationalityID { _ = t.Tarjeta }}
                                        //}},
                                        // Entidad emisora de la autorización especial - vehículo secundario
                                        // Número de autorización especial del vehículo emitido por la entidad - vehículo secundario
                                        ShipmentDocumentReference = string.IsNullOrWhiteSpace(t.NroAutorizacion) ? null : new List<ShipmentDocumentReference>
                                        {
                                            new ShipmentDocumentReference
                                            {
                                                ID = new List<ID>
                                                {
                                                    new ID
                                                    {
                                                        // Número de autorización
                                                        _ = t.NroAutorizacion,
                                                        // Código de entidad emisora
                                                        schemeID = t.CodEmisorVehicular
                                                    }
                                                }
                                            }
                                        },
                                    }
                                }
                                });
                            }
                        }
                    }
                }



                    // Añadimos los TransportEquipment al root
                    root.DespatchAdvice[0].Shipment[0].TransportHandlingUnit = new List<TransportHandlingUnit>
                {
                    new TransportHandlingUnit
                    {
                        TransportEquipment = transportEquipments
                    }
                };
            }

            // Transportista
            // Solo sí es PUBLICO y no se seleccionó el traslado en vehículos de categoría M1 o L, se registra la información del transportista; en caso contrario, no se registra la información del transportista.
            if (guia.TipoTransporte == "PUBLICO" && !guia.DatosEnvio.IndicadorTrasladoVehiculoCategoria)
            {
                root.DespatchAdvice[0].Shipment[0].ShipmentStage[0].CarrierParty = new List<CarrierParty>{ new CarrierParty {
                    // Número y código SUNAT del tipo de documento de identidad del transportista
                    PartyIdentification = new List<PartyIdentification>{ new PartyIdentification { ID = new List<ID> { new ID { 
                        // Número de documento de identidad del transportista
                        _ = guia.DatosEnvio.Transportista!.NumeroDocumento, 
                        // Código del tipo de documento de identidad del transportista, por defecto es RUC
                        schemeID=EspacioNombreGuiaRemision._DocumentType06 
                    }}}},
                    // Número de Registro MTC
                    PartyLegalEntity = new List<PartyLegalEntity>{ new PartyLegalEntity {
                        // Apellidos y Nombres o denominación o razón social del transportista
                        RegistrationName = new List<RegistrationName>{ new RegistrationName { _ = guia.DatosEnvio.Transportista.RazonSocial }},
                        //Condicional: Corresponde al número de registro del MTC del transportista.
                        CompanyID = new List<CompanyID>{ new CompanyID { _ = guia.DatosEnvio.Transportista.RegistroMTC ?? "NUMREGISTRANS" }}
                    }},
                }};
            }

            // Bienes a trasladar
            foreach (var (item, index) in guia.Productos.Select((value, i) => (value, i)))
            {
                root.DespatchAdvice[0].DespatchLine.Add(new DespatchLine
                {
                    // Número de orden del ítem
                    ID = new List<ID_Int> { new ID_Int { _ = index + 1 } },
                    // Descripción unidad medida
                    Note = new List<Note> { new Note { _ = string.IsNullOrWhiteSpace(item.DescripcionUM) ? "UNIDAD" : item.DescripcionUM } },
                    // Cantidad del bien y código de unidad de medida SUNAT
                    DeliveredQuantity = new List<DeliveredQuantity>
                    {
                        new DeliveredQuantity { 
                            // Cantidad del bien a trasladar
                            _ = item.Cantidad, 
                            // Código de unidad de medida SUNAT, si no se proporciona se asigna "ZZ" que significa "Unidad de medida sin especificar"
                            unitCode = string.IsNullOrWhiteSpace(item.CodigoUm) ? "ZZ" : item.CodigoUm
                        }
                    },
                    OrderLineReference = new List<OrderLineReference> { new OrderLineReference { LineID = new List<LineID> { new LineID { _ = index + 1 } } } },
                    // Producto
                    Item = new List<Item> { new Item {

                     
                        // Descripción detallada del bien
                        Description = new List<Description>{ new Description { _ = item.Descripcion }},
                        // Código GTIN del bien (Se registra solo si el código GTIN del bien fue proporcionado, de lo contrario no se registra este bloque)
                        // Código GTIN solo puede tener longitud de 8, 12, 13 o 14 caracteres, si el código GTIN proporcionado no cumple con esta validación, no se registra el bloque de StandardItemIdentification
                        StandardItemIdentification = string.IsNullOrWhiteSpace(item.Gtin) ? null : new List<StandardItemIdentification>{ new StandardItemIdentification {
                            // Código GTIN del bien
                            ID = new List<ID>{ new ID { 
                                // Código de producto GTIN
                                _ = item.Gtin,
                                // Tipo de estructura GTIN
                                // Varia según la longitud del código GTIN: GTIN-8 para códigos de 8 caracteres, GTIN-12 para códigos de 12 caracteres, GTIN-13 para códigos de 13 caracteres y GTIN-14 para códigos de 14 caracteres
                                schemeID = item.TipoGtin()
                            }}
                        }},
                        // Código del bien asignado por el vendedor (Se registra solo si el código del bien asignado por el vendedor fue proporcionado, de lo contrario no se registra este bloque)
                        SellersItemIdentification = string.IsNullOrWhiteSpace(item.Codigo) ? null : new List<SellersItemIdentification>{new SellersItemIdentification {
                            // Código del bien
                            ID = new List<ID>{new ID { _ = item.Codigo }}
                        }},
                        // Código de clasificación del bien según el catálogo de la SUNAT (Se registra solo si el código de clasificación del bien según el catálogo de la SUNAT fue proporcionado, de lo contrario no se registra este bloque)
                        CommodityClassification = string.IsNullOrWhiteSpace(item.CodigoSunat) ? null : new List<CommodityClassification>{ new CommodityClassification {
                            // Código de clasificación del bien según el catálogo de la SUNAT
                            ItemClassificationCode = new List<ItemClassificationCode>{ new ItemClassificationCode {
                                _ = item.CodigoSunat
                            }}
                        }},
                        // Atributos adicionales del bien, bien normalizado, aranca, etc. (Se registra solo si el bien es un bien normalizado, de lo contrario no se registra este bloque)
                        AdditionalItemProperty = new[]
                        {
                            !string.IsNullOrWhiteSpace(item.CodigoSubnacional)
                                ? new AdditionalItemProperty
                                {
                                    // Nombre del atributo adicional del bien, en este caso se registra "INDICADOR_BIEN_NORMALIZADO" para indicar que el bien es un bien normalizado.
                                    Name = new List<Name> { new Name { _ = "Partida arancelaria" } },
                                    // Código del concepto del atributo adicional del bien, en este caso se registra "7022" para indicar que el atributo adicional corresponde al indicador de bien normalizado según el catálogo de la SUNAT.
                                    NameCode = new List<NameCode> { new NameCode { _ = "7020" } },
                                    // Valor del atributo adicional del bien, en este caso se registra "1" para indicar que el bien es un bien normalizado.
                                    Value = new List<Value> { new Value { _ = item.CodigoSubnacional } }
                                }
                                : null,

                            item.IndicadorBienNormalizado == true
                                ? new AdditionalItemProperty
                                {
                                    // Nombre del atributo adicional del bien, en este caso se registra "INDICADOR_BIEN_NORMALIZADO" para indicar que el bien es un bien normalizado.
                                    Name = new List<Name> { new Name { _ = "Indicador de bien normalizado" } },
                                    // Código del concepto del atributo adicional del bien, en este caso se registra "7022" para indicar que el atributo adicional corresponde al indicador de bien normalizado según el catálogo de la SUNAT.
                                    NameCode = new List<NameCode> { new NameCode { _ = "7022" }},
                                    // Valor del atributo adicional del bien, en este caso se registra "1" para indicar que el bien es un bien normalizado.
                                    Value = new List<Value> { new Value { _ = "1" } }
                                }
                                : null,

                        }
                        .Where(x => x != null)
                        .Select(y => new AdditionalItemProperty { Name = y.Name, NameCode = y.NameCode, Value = y.Value })
                        .ToList().Count() > 0 ? new[]
                        {
                            !string.IsNullOrWhiteSpace(item.CodigoSubnacional)
                                ? new AdditionalItemProperty
                                {
                                    // Nombre del atributo adicional del bien, en este caso se registra "INDICADOR_BIEN_NORMALIZADO" para indicar que el bien es un bien normalizado.
                                    Name = new List<Name> { new Name { _ = "Partida arancelaria" } },
                                    // Código del concepto del atributo adicional del bien, en este caso se registra "7022" para indicar que el atributo adicional corresponde al indicador de bien normalizado según el catálogo de la SUNAT.
                                    NameCode = new List<NameCode> { new NameCode { _ = "7020" } },
                                    // Valor del atributo adicional del bien, en este caso se registra "1" para indicar que el bien es un bien normalizado.
                                    Value = new List<Value> { new Value { _ = item.CodigoSubnacional } }
                                }
                                : null,

                            item.IndicadorBienNormalizado == true
                                ? new AdditionalItemProperty
                                {
                                    // Nombre del atributo adicional del bien, en este caso se registra "INDICADOR_BIEN_NORMALIZADO" para indicar que el bien es un bien normalizado.
                                    Name = new List<Name> { new Name { _ = "Indicador de bien normalizado" } },
                                    // Código del concepto del atributo adicional del bien, en este caso se registra "7022" para indicar que el atributo adicional corresponde al indicador de bien normalizado según el catálogo de la SUNAT.
                                    NameCode = new List<NameCode> { new NameCode { _ = "7022" }},
                                    // Valor del atributo adicional del bien, en este caso se registra "1" para indicar que el bien es un bien normalizado.
                                    Value = new List<Value> { new Value { _ = "1" } }
                                }
                                : null,
                        }
                        .Where(x => x != null)
                        .Select(y => new AdditionalItemProperty { Name = y.Name, NameCode = y.NameCode, Value = y.Value })
                        .ToList() : null,
                    }},

                });
            }

            if (!string.IsNullOrEmpty(guia.Observacion))
                root.DespatchAdvice[0].Note = new List<Note> { new Note { _ = guia.Observacion } };



            string guiaJson = JsonConvert.SerializeObject(
                root,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );

            //Se está guardando en la carpeta ahora mismo. Actualización, se pasó a producción y ahora se guarda en AWS
            var nombreArchivo = guia.MotivoTrasladoCodigo != "07" ? getNombreDocumentoFile(guia, guia.Remitente.EntityDocumentNumber) : getNombreDocumentoFile(guia, guia.Destinatario.EntityDocumentNumber);
            //Helper.GuardarJSON(guiaJson, RutaDocPendiente(guia, guia.Remitente.NumeroDocumentoRemitente));//Ya no se usa

            // Convertir el string JSON a un stream de memoria
            var jsonBytes = System.Text.Encoding.UTF8.GetBytes(guiaJson);
            using var stream = new MemoryStream(jsonBytes);

            // Crear un IFormFile en memoria
            var formFile = new FormFile(stream, 0, jsonBytes.Length, "file", nombreArchivo)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/json"
            };

            await _s3FileService.UploadFileWithCustomNameAsync(formFile, nombreArchivo);
            return nombreArchivo;
        }
        public string RutaDocPendiente(GuiaRemitenteDTO guia, string ruc)
        {

            string expandedEnvString = _config["AppSettings:path_pendiente"].Replace("\\", @"\");
            var ruta = @"" + expandedEnvString + getNombreDocumentoFile(guia, ruc);
            return ruta;
        }

        public string getNombreDocumentoFile(GuiaRemitenteDTO guia, string ruc)
        {
            return ruc + "-09-" + guia.SerieCorrelativo + ".json";
        }
    }
}
