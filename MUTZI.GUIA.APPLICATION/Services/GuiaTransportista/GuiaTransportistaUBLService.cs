using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;
using Newtonsoft.Json;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaTransportista
{
    public class GuiaTransportistaUBLService : IGuiaTransportistaUBLService
    {
        private readonly IConfiguration _config;
        private readonly IS3FileService _s3FileService;
        private readonly string _baseUrl = "";
        public GuiaTransportistaUBLService(IConfiguration config, IS3FileService s3FileService)
        {
            _config = config;
            _s3FileService = s3FileService;
            _baseUrl = _config["Aws:BaseUrl"] ?? "";
        }
        public async Task<string> GenerarGuiaTransportistaJson(GuiaTransportistaDTO guia)
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
                    UBLVersionID           = new List<UBLVersionID>           { new UBLVersionID { _ = EspacioNombreGuiaRemision._UBLVersionID }}, // "2.1"
                    CustomizationID        = new List<CustomizationID>        { new CustomizationID { _ = EspacioNombreGuiaRemision._CustomizationID }}, // "2.0"
                    ID                     = new List<ID>                     { new ID { _ = guia.SerieCorrelativo }}, // Ej. "T001-00001234"
                    IssueDate              = new List<IssueDate>              { new IssueDate { _ = guia.Fecha }},
                    IssueTime              = new List<IssueTime>              { new IssueTime { _ = guia.Hora }},
                    DespatchAdviceTypeCode = new List<DespatchAdviceTypeCode> { new DespatchAdviceTypeCode { _ = "31" }}, // Transportista
                    LineCountNumeric       = new List<LineCountNumeric>       { new LineCountNumeric { _ = (guia.Productos?.Count ?? 0).ToString() }},

                    // Doc. de referencia (Factura/Boleta)
                    //Agregar en lista los documentos relacionados
                    AdditionalDocumentReference = guia.DocRelacionados.Select(docRelacionado => new AdditionalDocumentReference {
                        ID = new List<ID>{ new ID { _ = docRelacionado.NumeroDocumentoReferencia }},
                        DocumentTypeCode = new List<DocumentTypeCode>{ new DocumentTypeCode { _ = docRelacionado.DocumentoAdicionalTipo() }},
                        DocumentType=new List<DocumentType>{new DocumentType { _=docRelacionado.TipoDocumentoReferencia} },
                        IssuerParty = new List<IssuerParty>{ new IssuerParty {
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification {
                                ID = new List<ID>{ new ID { _ = docRelacionado.RucDocumentoReferencia , schemeID = EspacioNombreGuiaRemision._DocumentType06 }}
                            }}
                        }}
                    }).ToList(),

                    // Firma (emite la guía: el transportista)
                    // Edit 2: Aquí se pone quien tiene acceso a EFACT, es decir; Carolina SAC u otro con acceso
                    // Edit 3: En este contexto de GuiaRemision Transportista el que hace la guía debe ser el transportista(supongo que Carolina SAC a veces hará de transportista o no lo sé).
                    Signature = new List<Signature> {
                        new Signature {
                            ID = new List<ID> { new ID { _ = "IDSignature" }},
                            SignatoryParty = new List<SignatoryParty> {
                                new SignatoryParty {
                                    PartyIdentification = new List<PartyIdentification> {
                                        new PartyIdentification { ID = new List<ID> { new ID { _ = guia.Remitente.EntityDocumentNumber } } }
                                    },
                                    PartyName = new List<PartyName> {
                                        new PartyName { Name = new List<Name> { new Name { _ = guia.Remitente.Description } } }
                                    }
                                }
                            },
                            DigitalSignatureAttachment = new List<DigitalSignatureAttachment> {
                                new DigitalSignatureAttachment {
                                    ExternalReference = new List<ExternalReference> {
                                        new ExternalReference { URI = new List<URI> { new URI { _ = "IDSignature" } } }
                                    }
                                }
                            }
                        }
                    },

                    // Aquí va el transportista en si, osea si Carolina SAC actúa como transportista, irán sus datos
                    DespatchSupplierParty = new List<DespatchSupplierParty> {
                        new DespatchSupplierParty {
                            Party = new List<Party> {
                                new Party {
                                    PartyIdentification = new List<PartyIdentification> {
                                        new PartyIdentification {
                                            ID = new List<ID> { new ID { _ = guia.Remitente.EntityDocumentNumber, schemeID = EspacioNombreGuiaRemision._DocumentType06 } }
                                        }
                                    },
                                    PostalAddress = guia.Origen == null ? null : new List<PostalAddress> {
                                        new PostalAddress {
                                            ID                 = new List<ID>                { new ID                { _ = guia.Remitente.UbigeoId }},
                                            StreetName         = new List<StreetName>        { new StreetName        { _ = guia.Remitente.Address }},
                                            CitySubdivisionName= new List<CitySubdivisionName>{ new CitySubdivisionName{ _ = guia.Remitente.Department }},
                                            CityName           = new List<CityName>          { new CityName          { _ = guia.Remitente.Province }},
                                            CountrySubentity   = new List<CountrySubentity>  { new CountrySubentity  { _ = guia.Remitente.Department }},
                                            District           = new List<District>          { new District          { _ = guia.Remitente.District }},
                                            Country            = new List<Country>           { new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Remitente.EntityCountry }}}}
                                        }
                                    },
                                    PartyLegalEntity = new List<PartyLegalEntity> {
                                        new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _ = guia.Remitente.Description } } }
                                    }
                                }
                            }
                        }
                    },

                    // Destinatario
                    DeliveryCustomerParty = new List<DeliveryCustomerParty> {
                        new DeliveryCustomerParty {
                            Party = new List<Party> {
                                new Party {
                                    PartyIdentification = new List<PartyIdentification> {
                                        new PartyIdentification {
                                            ID = new List<ID> { new ID { _ = guia.Destinatario.EntityDocumentNumber, schemeID = guia.getTipoCodigoDocumento("RUC") } }
                                        }
                                    },
                                    PostalAddress = guia.Destino == null ? null : new List<PostalAddress> {
                                        new PostalAddress {
                                            ID                 = new List<ID>                { new ID                { _ = guia.Destinatario.UbigeoId }},
                                            StreetName         = new List<StreetName>        { new StreetName        { _ = guia.Destinatario.Address }},
                                            CitySubdivisionName= new List<CitySubdivisionName>{ new CitySubdivisionName{ _ = guia.Destinatario.Department}},
                                            CityName           = new List<CityName>          { new CityName          { _ = guia.Destinatario.Province }},
                                            CountrySubentity   = new List<CountrySubentity>  { new CountrySubentity  { _ = guia.Destinatario.Department }},
                                            District           = new List<District>          { new District          { _ = guia.Destinatario.District }},
                                            Country            = new List<Country>           { new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Destinatario.EntityCountry }}}}
                                        }
                                    },
                                    PartyLegalEntity = new List<PartyLegalEntity> {
                                        new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _ = guia.Destinatario.EntityName } } }
                                    },
                                    Contact = guia.DestinatarioEmail.Count > 0 ? new List<Contact>
                                    {
                                        new Contact
                                        {
                                            ElectronicMail = new List<ElectronicMail>
                                            {
                                                new ElectronicMail
                                                {
                                                    _ = string.Join(", ", guia.DestinatarioEmail.Select(email => email.Email))
                                                }
                                            }
                                        }
                                    } : null
                                }
                            }
                        }
                    },

                    DespatchLine = new List<DespatchLine>() // se llena abajo
                }
            };

            // ------ Shipment (envío) ------
            var despatch = root.DespatchAdvice[0];
            despatch.Shipment = new List<Shipment> {
                new Shipment {
                    ID = new List<ID> { new ID { _ = "SUNAT_Envio" }},

                    GrossWeightMeasure = new List<GrossWeightMeasure> {
                        new GrossWeightMeasure { _ = (guia.DatosEnvio?.PesoBruto ?? 0M).ToString(), unitCode = guia.DatosEnvio?.CodigoUM ?? "KGM" }
                    },

                    ShipmentStage = new List<ShipmentStage> {
                        new ShipmentStage {
                            ////EDit 2: Ya no va el código de modalidad de transporte en la guía del transportista
                            //TransportModeCode = new List<TransportModeCode> { new TransportModeCode { _ = MapModalidadTransporte18(guia.DatosEnvioDto?.TipoTransporte) }},
                            TransitPeriod = string.IsNullOrWhiteSpace(guia.DatosEnvio?.FechaEnvio) ? null : new List<TransitPeriod> {
                                new TransitPeriod { StartDate = new List<StartDate> { new StartDate { _ = guia.DatosEnvio.FechaEnvio } } }
                            },
                            //Edit 2: Aquí en transportista no va público ni privado
                            //// CarrierParty solo si es PÚBLICO y tienes datos
                            //CarrierParty = string.Equals(guia.DatosEnvioDto?.TipoTransporte, "PUBLICO", StringComparison.OrdinalIgnoreCase)
                            //    && !string.IsNullOrWhiteSpace(guia.DatosEnvioDto?.RucEmpresaEnvio)
                            //    ? new List<CarrierParty> {
                            //        new CarrierParty {
                            //            PartyIdentification = new List<PartyIdentification> {
                            //                new PartyIdentification { ID = new List<ID> { new ID { _ = guia.DatosEnvioDto.RucEmpresaEnvio, schemeID = EspacioNombreGuiaRemision._DocumentType06 } } }
                            //            },
                            //            PartyLegalEntity = new List<PartyLegalEntity> {
                            //                new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _ = guia.DatosEnvioDto.RazonSocialEmpresaEnvio } } }
                            //            }
                            //        }
                            //    }
                            //    : null,
                            // Conductor (opcional)
                            DriverPerson = guia.DatosEnvio?.ConductorGuia == null ? null : guia.DatosEnvio.ConductorGuia.Select(conductor => new DriverPerson
                                {
                                    ID = new List<ID> { new ID { _ = conductor.NumeroDocumento, schemeID = guia.getTipoCodigoDocumento(conductor.TipoDocumento) } },
                                    FirstName = new List<FirstName> { new FirstName { _ = conductor.Nombres } },
                                    FamilyName = new List<FamilyName> { new FamilyName { _ = conductor.Apellidos } },
                                    JobTitle = new List<JobTitle> { new JobTitle { _ = conductor.JobTitle } },
                                    IdentityDocumentReference = new List<IdentityDocumentReference>{ new IdentityDocumentReference {
                                        ID = new List<ID> { new ID { _ = conductor.Licencia  }}
                                    }}
                                }).ToList()
                        }
                    },

                    Delivery = new List<Delivery> {
                        new Delivery {
                            //Punto de llegada
                            DeliveryAddress = guia.Destino.Select(destino => new DeliveryAddress {
                        ID = new List<ID>{ new ID { _ = destino.UbigeoId }},
                        AddressTypeCode = string.IsNullOrWhiteSpace(guia.Destinatario.CodeSunat)?null: new List<AddressTypeCode>{ new AddressTypeCode {
                            _ = guia.Destinatario.CodeSunat,
                            listID = guia.Destinatario.EntityDocumentNumber
                        }},
                            CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = destino.Departamento }},
                            CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = destino.Departamento }},
                            CityName = new List<CityName>{ new CityName { _ = destino.Provincia }},
                            District = new List<District>{ new District { _ = destino.Distrito }},
                            AddressLine = new List<AddressLine>{ new AddressLine { Line = new List<Line> { new Line { _ = destino.Direccion }}}},
                            Country = new List<Country>{ new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = destino.Pais } }}}
                        }).ToList(),
                            Despatch = guia.Origen == null ? null : new List<Despatch> {
                                new Despatch {
                                    DespatchAddress = new List<DespatchAddress> {
                                        //Punto de partida
                                        new DespatchAddress {
                                            ID               = new List<ID>               { new ID { _ = guia.Origen.UbigeoId }},
                                            AddressTypeCode  = string.IsNullOrWhiteSpace(guia.RemiteTransportista.CodigoSunat)?null: new List<AddressTypeCode>  { new AddressTypeCode { _ = guia.RemiteTransportista.CodigoSunat, listID = guia.RemiteTransportista.NumeroDocumento }},
                                            CitySubdivisionName = new List<CitySubdivisionName> { new CitySubdivisionName { _ = guia.Origen.Departamento }},
                                            CityName         = new List<CityName>         { new CityName         { _ = guia.Origen.Provincia }},
                                            CountrySubentity = new List<CountrySubentity> { new CountrySubentity { _ = guia.Origen.Departamento }},
                                            District         = new List<District>         { new District         { _ = guia.Origen.Distrito }},
                                            AddressLine      = new List<AddressLine>      { new AddressLine { Line = new List<Line> { new Line { _ = guia.Origen.Direccion } } } },
                                            Country          = new List<Country> { new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Origen.Pais }}}}
                                        }
                                    },
                                    // En transportista, EFACT suele incluir datos del remitente aquí; dejamos PartyIdentification con el RUC del remitente
                                    //Edit 2: Aquí se coloca los datos del remitente, osea de no de CArolina SAC, ya que Carolina SAC actúa como transportista, el remitente puede ser otro y si el remitente sigue siendo CArolina SAC, entonces mejor que HAGA GUIA DE REMISIÓN REMITENTE Y NO TRANSPORTISTA
                                    DespatchParty = new List<DespatchParty> {
                                        new DespatchParty {
                                            PartyIdentification = new List<PartyIdentification> {
                                                new PartyIdentification { ID = new List<ID> { new ID { _ = guia.RemiteTransportista.NumeroDocumento, schemeID = guia.getTipoCodigoDocumento(guia.RemiteTransportista.TipoDocumento) } } }
                                            },
                                            PostalAddress = guia.RemiteTransportista == null ? null : new List<PostalAddress> {
                                                new PostalAddress {
                                                    ID                 = new List<ID>                { new ID                { _ = guia.RemiteTransportista.UbigeoId }},
                                                    StreetName         = new List<StreetName>        { new StreetName        { _ = guia.RemiteTransportista.Direccion }},
                                                    CitySubdivisionName= new List<CitySubdivisionName>{ new CitySubdivisionName{ _ = guia.RemiteTransportista.Departamento}},
                                                    CityName           = new List<CityName>          { new CityName          { _ = guia.RemiteTransportista.Provincia }},
                                                    CountrySubentity   = new List<CountrySubentity>  { new CountrySubentity  { _ = guia.RemiteTransportista.Departamento }},
                                                    District           = new List<District>          { new District          { _ = guia.RemiteTransportista.Distrito }},
                                                    Country            = new List<Country>           { new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.RemiteTransportista.Pais }}}}
                                                }
                                            },

                                            PartyLegalEntity = new List<PartyLegalEntity> {
                                                new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _ = guia.RemiteTransportista.RazonSocial } } }
                                            },
                                            Contact = guia.RemiteTransportista.EmailRemiteTransportista != null ? new List<Contact>
                                            {
                                                new Contact
                                                {
                                                    ElectronicMail = new List<ElectronicMail>
                                                    {
                                                        new ElectronicMail
                                                        {
                                                            _ = string.Join(", ", guia.RemiteTransportista.EmailRemiteTransportista.Select(email => email.Email))
                                                        }
                                                    }
                                                }
                                            } : null
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            if (guia.DatosEnvio.TransporteGuia != null && guia.DatosEnvio.TransporteGuia.Any())
            {
                var transportes = guia.DatosEnvio.TransporteGuia.ToList();
                var transportEquipments = new List<TransportEquipment>();

                // Primer transporte como TransportEquipment principal
                transportEquipments.Add(new TransportEquipment
                {
                    ID = new List<ID> { new ID { _ = transportes[0].Placa } },
                    ShipmentDocumentReference = string.IsNullOrWhiteSpace(transportes[0].NroAutorizacion) ? null : new List<ShipmentDocumentReference>
                        {
                            new ShipmentDocumentReference
                            {
                                ID = new List<ID>
                                {
                                    new ID
                                    {
                                        _ = transportes[0].NroAutorizacion,
                                        schemeID = transportes[0].CodEmisorVehicular
                                        //schemeID = "06"//ya no va
                                    }
                                }
                            }
                        }
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
                                        ID = new List<ID> { new ID { _ = t.Placa } },
                                        ShipmentDocumentReference = string.IsNullOrWhiteSpace(t.NroAutorizacion)?null: new List<ShipmentDocumentReference>
                                        {
                                            new ShipmentDocumentReference
                                            {
                                                ID = new List<ID>
                                                {
                                                    new ID
                                                    {
                                                        _ = t.NroAutorizacion,
                                                        schemeID = t.CodEmisorVehicular
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                        });
                    }
                }

                root.DespatchAdvice[0].Shipment[0].TransportHandlingUnit = new List<TransportHandlingUnit>
                    {
                        new TransportHandlingUnit
                        {
                            TransportEquipment = transportEquipments
                        }
                    };
            }

            // ------ Items ------
            if (guia.Productos != null)
            {
                for (int i = 0; i < guia.Productos.Count; i++)
                {
                    var p = guia.Productos[i];
                    despatch.DespatchLine.Add(new DespatchLine
                    {
                        ID = new List<ID_Int> { new ID_Int { _ = i + 1 } },
                        Note = new List<Note> { new Note { _ = string.IsNullOrWhiteSpace(p.DescripcionUM) ? "UNIDAD" : p.DescripcionUM } },
                        DeliveredQuantity = new List<DeliveredQuantity> {
                            new DeliveredQuantity { _ = p.Cantidad, unitCode = string.IsNullOrWhiteSpace(p.CodigoUm) ? "ZZ" : p.CodigoUm }
                        },
                        OrderLineReference = new List<OrderLineReference> { new OrderLineReference { LineID = new List<LineID> { new LineID { _ = i + 1 } } } },
                        Item = new List<Item> {
                            new Item {
                                Description = new List<Description> { new Description { _ = p.Descripcion } },
                                SellersItemIdentification = string.IsNullOrWhiteSpace(p.Codigo) ? null : new List<SellersItemIdentification> {
                                    new SellersItemIdentification { ID = new List<ID> { new ID { _ = p.Codigo } } }
                                }
                            }
                        }
                    });
                }
            }

            // Observación general
            if (!string.IsNullOrWhiteSpace(guia.Observacion))
                despatch.Note = new List<Note> { new Note { _ = guia.Observacion } };

            // Serializar ignorando nulos (igual que remitente)
            string guiaJson = JsonConvert.SerializeObject(
                root,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );

            var nombreArchivo = getNombreDocumentoFile(guia, guia.Remitente.EntityDocumentNumber);
            //Helper.GuardarJSON(guiaJson, RutaDocPendiente(guia, guia.Remitente.EntitySenderDocumentNumber));

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

        public string RutaDocPendiente(GuiaTransportistaDTO guia, string ruc)
        {
            string basePath = _config["AppSettings:path_pendiente"].Replace("\\", @"\");
            return Path.Combine(basePath, getNombreDocumentoFile(guia, ruc));
        }

        public string getNombreDocumentoFile(GuiaTransportistaDTO guia, string ruc)
        {
            return $"{ruc}-31-{guia.SerieCorrelativo}.json";
        }
    }
}
