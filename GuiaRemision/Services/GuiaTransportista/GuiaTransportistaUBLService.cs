using GuiaRemision.Constantes;
using GuiaRemision.DTOs.Efact;
using GuiaRemision.DTOs.GuiaTransportista;
using GuiaRemision.Helpers;
using Newtonsoft.Json;

namespace GuiaRemision.Services.GuiaTransportista
{
    public class GuiaTransportistaUBLService : IGuiaTransportistaUBLService
    {
        private readonly IConfiguration _config;
        public GuiaTransportistaUBLService(IConfiguration config)
        {
            _config = config;
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
                    AdditionalDocumentReference = string.IsNullOrWhiteSpace(guia.NumeroDocRef) ? null : new List<AdditionalDocumentReference> {
                        new AdditionalDocumentReference {
                            ID               = new List<ID>               { new ID               { _ = guia.NumeroDocRef }},
                            DocumentType     = string.IsNullOrWhiteSpace(guia.TipoDocRef) ? null : new List<DocumentType>    { new DocumentType    { _ = guia.TipoDocRef }},
                            DocumentTypeCode = new List<DocumentTypeCode>{ new DocumentTypeCode{ _ = guia.DocumentoAdicinalTipo() } },
                            IssuerParty      = string.IsNullOrWhiteSpace(guia.RucDocRef)        ? null : new List<IssuerParty> {
                                new IssuerParty {
                                    PartyIdentification = new List<PartyIdentification> {
                                        new PartyIdentification {
                                            ID = new List<ID> { new ID { _ = guia.RucDocRef, schemeID = EspacioNombreGuiaRemision._DocumentType06 }}
                                        }
                                    }
                                }
                            }
                        }
                    },

                    // Firma (emite la guía: el transportista)
                    // Edit 2: Aquí se pone quien tiene acceso a EFACT, es decir; Carolina SAC u otro con acceso
                    // Edit 3: En este contexto de Guia Transportista el que hace la guía debe ser el transportista(supongo que Carolina SAC a veces hará de transportista o no lo sé).
                    Signature = new List<Signature> {
                        new Signature {
                            ID = new List<ID> { new ID { _ = "IDSignature" }},
                            SignatoryParty = new List<SignatoryParty> {
                                new SignatoryParty {
                                    PartyIdentification = new List<PartyIdentification> {
                                        new PartyIdentification { ID = new List<ID> { new ID { _ = guia.Remitente.Ruc } } }
                                    },
                                    PartyName = new List<PartyName> {
                                        new PartyName { Name = new List<Name> { new Name { _ = guia.Remitente.RazonSocial } } }
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
                                            ID = new List<ID> { new ID { _ = guia.Remitente.Ruc, schemeID = EspacioNombreGuiaRemision._DocumentType06 } }
                                        }
                                    },
                                    PostalAddress = guia.Origen == null ? null : new List<PostalAddress> {
                                        new PostalAddress {
                                            ID                 = new List<ID>                { new ID                { _ = guia.Remitente.UbigeoID }},
                                            StreetName         = new List<StreetName>        { new StreetName        { _ = guia.Remitente.Direccion }},
                                            CitySubdivisionName= new List<CitySubdivisionName>{ new CitySubdivisionName{ _ = guia.Remitente.Departamento }},
                                            CityName           = new List<CityName>          { new CityName          { _ = guia.Remitente.Provincia }},
                                            CountrySubentity   = new List<CountrySubentity>  { new CountrySubentity  { _ = guia.Remitente.Departamento }},
                                            District           = new List<District>          { new District          { _ = guia.Remitente.Distrito }},
                                            Country            = new List<Country>           { new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Remitente.Pais }}}}
                                        }
                                    },
                                    PartyLegalEntity = new List<PartyLegalEntity> {
                                        new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _ = guia.Remitente.RazonSocial } } }
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
                                            ID = new List<ID> { new ID { _ = guia.Destinatario.NumeroDocumento, schemeID = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento) } }
                                        }
                                    },
                                    PostalAddress = guia.Destino == null ? null : new List<PostalAddress> {
                                        new PostalAddress {
                                            ID                 = new List<ID>                { new ID                { _ = guia.Destinatario.UbigeoId }},
                                            StreetName         = new List<StreetName>        { new StreetName        { _ = guia.Destinatario.Direccion }},
                                            CitySubdivisionName= new List<CitySubdivisionName>{ new CitySubdivisionName{ _ = guia.Destinatario.Departamento}},
                                            CityName           = new List<CityName>          { new CityName          { _ = guia.Destinatario.Provincia }},
                                            CountrySubentity   = new List<CountrySubentity>  { new CountrySubentity  { _ = guia.Destinatario.Departamento }},
                                            District           = new List<District>          { new District          { _ = guia.Destinatario.Distrito }},
                                            Country            = new List<Country>           { new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Destinatario.Pais }}}}
                                        }
                                    },
                                    PartyLegalEntity = new List<PartyLegalEntity> {
                                        new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _ = guia.Destinatario.RazonSocial } } }
                                    },
                                    Contact=new List<Contact>
                                    {
                                        new Contact
                                        {
                                            ElectronicMail=new List<ElectronicMail>
                                            {
                                                new ElectronicMail
                                                {
                                                    _ = guia.Destinatario.Email
                                                    //_ = "sistemas7@carolina-peru.com"
                                                }
                                            }
                                        }
                                    }
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

                    //// Edit 2: No va Motivo de traslado como: HandlingInstructions en transportista
                    //HandlingInstructions = string.IsNullOrWhiteSpace(guia.DatosEnvioDto?.MotivoTraslado) ? null
                    //    : new List<HandlingInstruction> { new HandlingInstruction { _ = guia.DatosEnvioDto.MotivoTraslado } },
                    //HandlingCode = ToCode20OrNull(guia.DatosEnvioDto?.MotivoTraslado),

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
                            DriverPerson = guia.DatosEnvio?.Conductor == null ? null : new List<DriverPerson> {
                                new DriverPerson {
                                    ID = new List<ID> { new ID { _ = guia.DatosEnvio.Conductor.DNI, schemeID = EspacioNombreGuiaRemision._DocumentType01 } },
                                    FirstName  = new List<FirstName>  { new FirstName  { _ = guia.DatosEnvio.Conductor.Nombres } },
                                    FamilyName = new List<FamilyName> { new FamilyName { _ = guia.DatosEnvio.Conductor.Apellidos } },
                                    JobTitle   = new List<JobTitle>   { new JobTitle   { _ = "Principal" } },
                                    IdentityDocumentReference = new List<IdentityDocumentReference> {
                                        new IdentityDocumentReference { ID = new List<ID> { new ID { _ = guia.DatosEnvio.Conductor.Licencia } } }
                                    }
                                }
                            }
                        }
                    },

                    // Direcciones de entrega y despacho (desde tu DTO)
                    Delivery = new List<Delivery> {
                        new Delivery {
                            DeliveryAddress = guia.Destino.Select(destino => new DeliveryAddress {
                        ID = new List<ID>{ new ID { _ = destino.UbigeoId }},
                        AddressTypeCode = new List<AddressTypeCode>{ new AddressTypeCode {
                            _ = "0000",
                            listID = guia.Destinatario.NumeroDocumento
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
                                        new DespatchAddress {
                                            ID               = new List<ID>               { new ID { _ = guia.Origen.UbigeoId }},
                                            AddressTypeCode  = new List<AddressTypeCode>  { new AddressTypeCode { _ = "0001", listID = guia.Remitente.Ruc }},
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
                                                new PartyIdentification { ID = new List<ID> { new ID { _ = guia.RemiteTransportista.Ruc, schemeID = EspacioNombreGuiaRemision._DocumentType06 } } }
                                            },
                                            PostalAddress = guia.RemiteTransportista == null ? null : new List<PostalAddress> {
                                                new PostalAddress {
                                                    ID                 = new List<ID>                { new ID                { _ = guia.RemiteTransportista.UbigeoID }},
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
                                            Contact=new List<Contact>
                                            {
                                                new Contact
                                                {
                                                    ElectronicMail=new List<ElectronicMail>
                                                    {
                                                        new ElectronicMail
                                                        {
                                                            _ = guia.RemiteTransportista.Email
                                                            //_ = "sistemas7@carolina-peru.com"
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },

                    TransportHandlingUnit =
                        new List<TransportHandlingUnit> {
                            new TransportHandlingUnit {
                                TransportEquipment = new List<TransportEquipment> {
                                    new TransportEquipment {
                                        ID = new List<ID> { new ID { _ = "PLACA1" }}, // etiqueta (placeholder)
                                        ShipmentDocumentReference = new List<ShipmentDocumentReference> {
                                            new ShipmentDocumentReference {
                                                ID = new List<ID> { new ID { _ = guia.DatosEnvio.UnidadTransporte.Placa, schemeID = "06" } }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }
            };

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
                            new DeliveredQuantity { _ = p.cantidad, unitCode = string.IsNullOrWhiteSpace(p.CodigoUM) ? "ZZ" : p.CodigoUM }
                        },
                        OrderLineReference = new List<OrderLineReference> { new OrderLineReference { LineID = new List<LineID> { new LineID { _ = i + 1 } } } },
                        Item = new List<Item> {
                            new Item {
                                Description = new List<Description> { new Description { _ = p.descripcion } },
                                SellersItemIdentification = string.IsNullOrWhiteSpace(p.codigo) ? null : new List<SellersItemIdentification> {
                                    new SellersItemIdentification { ID = new List<ID> { new ID { _ = p.codigo } } }
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

            var nombreArchivo = getNombreDocumentoFile(guia, guia.Remitente.Ruc);
            Helper.GuardarJSON(guiaJson, RutaDocPendiente(guia, guia.Remitente.Ruc));
            return nombreArchivo;
        }

        //private static List<HandlingCode> ToCode20OrNull(string? motivo)
        //{
        //    if (string.IsNullOrWhiteSpace(motivo)) return null!;
        //    // Si ya te llega "01","02", etc., lo respetamos.
        //    var trimmed = motivo.Trim();
        //    if (trimmed.All(char.IsDigit) && (trimmed.Length == 2 || trimmed.Length == 1))
        //        return new List<HandlingCode> { new HandlingCode { _ = trimmed.PadLeft(2, '0') } };

        //    // Caso contrario, no forzamos mapeo para no inventar códigos.
        //    return null!;
        //}

        //private static string MapModalidadTransporte18(string? tipo)
        //{
        //    if (string.IsNullOrWhiteSpace(tipo)) return "01"; // por defecto público
        //    var t = tipo.Trim().ToUpperInvariant();
        //    // Catálogo 18: 01 Público, 02 Privado
        //    if (t == "PRIVADO" || t == "02") return "02";
        //    return "01";
        //}

        public string RutaDocPendiente(GuiaTransportistaDTO guia, string ruc)
        {
            string basePath = _config["AppSettings:path_pendiente"].Replace("\\", @"\");
            return System.IO.Path.Combine(basePath, getNombreDocumentoFile(guia, ruc));
        }

        public string getNombreDocumentoFile(GuiaTransportistaDTO guia, string ruc)
        {
            return $"{ruc}-31-{guia.SerieCorrelativo}.json";
        }
    }
}
