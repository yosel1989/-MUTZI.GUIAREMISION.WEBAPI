using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using Newtonsoft.Json;
using MUTZI.GUIA.APPLICATION.Helpers;
using Microsoft.Extensions.Configuration;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente
{
    public class GuiaRemitenteUBLService : IGuiaRemitenteUBLService
    {
        private readonly IConfiguration _config;
        public GuiaRemitenteUBLService(IConfiguration config)
        {
            _config = config;
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
                    UBLVersionID = new List<UBLVersionID>{ new UBLVersionID { _ =EspacioNombreGuiaRemision._UBLVersionID }},
                    CustomizationID = new List<CustomizationID>{ new CustomizationID {  _ = EspacioNombreGuiaRemision._CustomizationID }},
                    ID = new List<ID> { new ID { _ = guia.SerieCorrelativo }},//"T001-00000063"
                    IssueDate = new List<IssueDate> { new IssueDate { _ = guia.Fecha }},
                    IssueTime = new List<IssueTime> { new IssueTime { _ = guia.Hora }},
                    //catálogo N°01 del anexo 8 de la SUNAT, 09 Guía de remisión remitente
                    DespatchAdviceTypeCode = new List<DespatchAdviceTypeCode>{ new DespatchAdviceTypeCode { _ = EspacioNombreGuiaRemision._DespatchAdviceTypeCode }},
                    LineCountNumeric = new List<LineCountNumeric> { new LineCountNumeric { _ = guia.Productos.Count.ToString() }},
                    AdditionalDocumentReference = new List<AdditionalDocumentReference>{ new AdditionalDocumentReference {
                        ID = new List<ID>{ new ID { _  = guia.NumeroDocRef }}, //DNI de quien?
                        DocumentType = new List<DocumentType>{ new DocumentType { _ = guia.TipoDocRef}},//{ _ = EspacioNombreGuiaRemision._DocumentType01 }},
                        DocumentTypeCode = new List<DocumentTypeCode>{ new DocumentTypeCode { _  = guia.DocumentoAdicinalTipo() } },
                        IssuerParty = new List<IssuerParty>{ new IssuerParty {
                            PartyIdentification = new List<PartyIdentification> { new PartyIdentification {
                                    ID = new List<ID> { new ID {
                                            _ = guia.RucDocRef
                                        ,   schemeID = EspacioNombreGuiaRemision._DocumentType06 }
                                    }},
                            },
                        }},
                    }},

                    Signature = new List<Signature>{ new Signature{
                        ID = new List<ID>{ new ID { _ = "IDSignature" } },
                        SignatoryParty = new List<SignatoryParty>{ new SignatoryParty {
                            PartyIdentification= new List<PartyIdentification>{ new PartyIdentification { ID = new List<ID> { new ID {  _ = guia.Remitente.Ruc } } }},
                            PartyName = new List<PartyName>{ new PartyName { Name = new List<Name> { new Name {  _ = guia.Remitente.RazonSocial} } }},
                        }},
                        DigitalSignatureAttachment = new List<DigitalSignatureAttachment>{ new DigitalSignatureAttachment {
                            ExternalReference = new List<ExternalReference>{new ExternalReference { URI = new List<URI> { new URI() {_ = "IDSignature" }}}}
                        }}
                    }},
                    //Remitente
                    DespatchSupplierParty = new List<DespatchSupplierParty>{ new DespatchSupplierParty {
                        Party = new List<Party>{ new Party {
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification { ID  = new List<ID> { new ID { _ = guia.Remitente.Ruc,  schemeID = EspacioNombreGuiaRemision._DocumentType06  }}}},
                            PostalAddress = new List<PostalAddress>{ new PostalAddress {
                                ID = new List<ID> { new ID { _ = guia.Remitente.UbigeoID } },
                                CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = guia.Remitente.Departamento } },
                                CityName = new List<CityName>{ new CityName { _ = guia.Remitente.Provincia} },
                                StreetName = new List<StreetName>{ new StreetName { _ = guia.Remitente.Direccion } },
                                CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Remitente.Departamento } },
                                District= new List<District>{ new District { _ = guia.Remitente.Distrito } },
                                Country = new List<Country>{ new Country {  IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Remitente.Pais } }}}
                            }},
                            PartyLegalEntity = new List<PartyLegalEntity>{ new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _  = guia.Remitente.RazonSocial }}}}
                        }},
                    }},
                    //Destinatario
                    DeliveryCustomerParty = new List<DeliveryCustomerParty>{ new DeliveryCustomerParty {
                        Party = new List<Party>{ new Party {
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification {
                                ID = new List<ID>{ new ID { _ = guia.Destinatario.NumeroDocumento , schemeID = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento)  }}
                            }},
                            PostalAddress = new List<PostalAddress>{ new PostalAddress {
                                ID = new List<ID> { new ID { _ = guia.Destinatario.UbigeoId}},
                                StreetName = new List<StreetName>{ new StreetName { _ = guia.Destinatario.Direccion } },
                                CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Destinatario.Departamento } },
                                CityName = new List<CityName>{ new CityName { _ = guia.Destinatario.Provincia } },
                                CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = guia.Destinatario.Departamento } },
                                District= new List<District>{ new District { _ = guia.Destinatario.Distrito } },
                                Country = new List<Country>{ new Country {  IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Destinatario.Pais } }}}
                            }},
                            PartyLegalEntity = new List<PartyLegalEntity>{ new PartyLegalEntity {
                                RegistrationName = new List<RegistrationName>{ new RegistrationName { _ = guia.Destinatario.RazonSocial } }
                            }},
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
                        }}
                    }},
                    //El proveedor se crea solo si el tipo de traslado es compra
                    SellerSupplierParty = guia.TipoTraslado == "COMPRA" ? new List<SellerSupplierParty>{ new SellerSupplierParty {
                        Party = new List<Party>{ new Party {
                            PartyIdentification = new List<PartyIdentification>{ new PartyIdentification { ID  = new List<ID> { new ID { _ = guia.Proveedor.NumeroDocumento,  schemeID = guia.getTipoCodigoDocumento(guia.Proveedor.TipoDocumento)  }}}},
                            PostalAddress = new List<PostalAddress>{ new PostalAddress {
                                ID = new List<ID> { new ID { _ = guia.Proveedor.UbigeoId } },
                                CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = guia.Proveedor.Departamento } },
                                CityName = new List<CityName>{ new CityName { _ = guia.Proveedor.Provincia} },
                                StreetName = new List<StreetName>{ new StreetName { _ = guia.Proveedor.Direccion } },
                                CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Proveedor.Departamento } },
                                District= new List<District>{ new District { _ = guia.Proveedor.Distrito } },
                                Country = new List<Country>{ new Country {  IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Proveedor.Pais } }}}
                            }},
                            PartyLegalEntity = new List<PartyLegalEntity>{ new PartyLegalEntity { RegistrationName = new List<RegistrationName> { new RegistrationName { _  = guia.Proveedor.RazonSocial }}}}
                        }},
                    }} : null,






                    DespatchLine = new List<DespatchLine>{}
                }
            };

            root.DespatchAdvice[0].Shipment = new List<Shipment> { new Shipment {
                ID = new List<ID>{ new ID { _ = "SUNAT_Envio" }},
                HandlingCode = new List<HandlingCode>{ new HandlingCode { _ = guia.TipoTrasladoCode20() }},
                HandlingInstructions = new List<HandlingInstruction>{ new HandlingInstruction { _ = guia.DatosEnvio.MotivoEnvio }},
                GrossWeightMeasure = new List<GrossWeightMeasure>{ new GrossWeightMeasure { _ =guia.DatosEnvio.PesoBruto.ToString() , unitCode = guia.DatosEnvio?.CodigoUM??"KGM" }},

                ShipmentStage = new List<ShipmentStage>{ new ShipmentStage {
                    TransportModeCode = new List<TransportModeCode>{ new TransportModeCode { _ = guia.TipoTRansporteCode18() }},
                    TransitPeriod = new List<TransitPeriod>{ new TransitPeriod {
                        StartDate = new List<StartDate>{ new StartDate { _ = guia.DatosEnvio.FechaEnvio }}
                    }},

                }},
                Delivery= new List<Delivery>{ new Delivery {
                    //DeliveryAddress = new List<DeliveryAddress>{ new DeliveryAddress {
                    //    ID = new List<ID>{ new ID { _ = guia.Destino.UbigeoId }},
                    //    AddressTypeCode = new List<AddressTypeCode>{new AddressTypeCode {
                    //        _ ="0000",
                    //        listID = guia.Destinatario.NumeroDocumento
                    //    }},
                    //    CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Destino.Departamento } },
                    //    CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _= guia.Destino.Departamento }},
                    //    CityName = new List<CityName>{ new CityName { _ = guia.Destino.Provincia }},
                    //    District = new List<District>{ new District { _ = guia.Destino.Distrito}},
                    //    AddressLine = new List<AddressLine>{ new AddressLine { Line = new List<Line> { new Line { _  = guia.Destino.Direccion }}}},
                    //    Country = new List<Country>{ new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Destino.Pais } }}}
                    //}},
                    DeliveryAddress = guia.Destino.Select(destino => new DeliveryAddress {
                        ID = new List<ID>{ new ID { _ = destino.UbigeoId }},
                        AddressTypeCode = new List<AddressTypeCode>{ new AddressTypeCode {
                            _ = "0000",
                            //Si es traslado, se coloca el ruc del remitente, ya que es traslado entre sucursales nomas; si es venta o compra, el numeroDoc del destinatario
                            listID = guia.TipoTraslado=="TRASLADO"? guia.Remitente.Ruc : guia.Destinatario.NumeroDocumento
                        }},
                        CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = destino.Departamento }},
                        CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _ = destino.Departamento }},
                        CityName = new List<CityName>{ new CityName { _ = destino.Provincia }},
                        District = new List<District>{ new District { _ = destino.Distrito }},
                        AddressLine = new List<AddressLine>{ new AddressLine { Line = new List<Line> { new Line { _ = destino.Direccion }}}},
                        Country = new List<Country>{ new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = destino.Pais } }}}
                    }).ToList(),
                    Despatch = new List<Despatch>{ new Despatch {
                         DespatchAddress = new List<DespatchAddress>{ new DespatchAddress {
                            ID = new List<ID>{ new ID { _ = guia.Origen.UbigeoId }},
                             AddressTypeCode = new List<AddressTypeCode>{new AddressTypeCode {
                                _ ="0001",
                                //Si es compra, se coloca el numeroDoc del proveedor, si es venta o traslado, el ruc del remitente
                                listID = guia.TipoTraslado=="COMPRA" ? guia.Proveedor.NumeroDocumento: guia.Remitente.Ruc
                            }},
                            CitySubdivisionName = new List<CitySubdivisionName>{ new CitySubdivisionName { _ = guia.Origen.Departamento } },
                            CityName = new List<CityName>{ new CityName { _ = guia.Origen.Provincia }},
                            District = new List<District>{ new District { _ = guia.Origen.Distrito}},
                            CountrySubentity = new List<CountrySubentity>{ new CountrySubentity { _= guia.Origen.Departamento }},
                            AddressLine = new List<AddressLine>{ new AddressLine { Line = new List<Line> { new Line { _  = guia.Origen.Direccion }}}},
                            Country = new List<Country>{ new Country { IdentificationCode = new List<IdentificationCode> { new IdentificationCode { _ = guia.Origen.Pais } }}}
                         }},
                         //Edit: Aquí se va a colocar la autorización y el código que corresponde en caso la empresa haya sido autorizada, ya sea por DIGESA, DIGEMID,etc, para poder trasladar productos específicos.
                         //DespatchParty = new List<DespatchParty>{new DespatchParty { AgentParty = new List<AgentParty> {
                         //    new AgentParty {
                         //       PartyLegalEntity = new List<PartyLegalEntity>{new PartyLegalEntity {
                         //               CompanyID = new List<CompanyID>{new CompanyID {
                         //                   _ = "NUMAUTORIREMIT123",
                         //                   schemeID = "06",
                         //               }}
                         //       }}
                         //   }}
                         //}}
                    }}
                }},
            }};



            if (guia.DatosEnvio.Conductor != null)
                root.DespatchAdvice[0].Shipment[0].ShipmentStage[0].DriverPerson = new List<DriverPerson>{
                    new DriverPerson {
                        ID = new List<ID>{ new ID { _ = guia.DatosEnvio.Conductor.DNI , schemeID = EspacioNombreGuiaRemision._DocumentType01 }},
                        FirstName = new List<FirstName>{ new FirstName { _ = guia.DatosEnvio.Conductor.Nombres }},
                        FamilyName = new List<FamilyName>{ new FamilyName { _ = guia.DatosEnvio.Conductor.Apellidos }},
                        JobTitle = new List<JobTitle>{ new JobTitle {_= "Principal" } },//{ _ = guia.datosEnvio.conductor.cargo }},
                        IdentityDocumentReference = new List<IdentityDocumentReference>{ new IdentityDocumentReference {
                            ID = new List<ID> { new ID { _ = guia.DatosEnvio.Conductor.Licencia  }}
                        }}
                    }};

            if (guia.TipoTransporte == "PRIVADO")
            {
                if (guia.DatosEnvio.UnidadTransporte != null)
                    root.DespatchAdvice[0].Shipment[0].TransportHandlingUnit = new List<TransportHandlingUnit> { new TransportHandlingUnit {
                        TransportEquipment = new List<TransportEquipment>{new TransportEquipment {
                            ID = new List<ID>{ new ID { _ = "PLACA1"}},
                            ShipmentDocumentReference = new List<ShipmentDocumentReference>{new ShipmentDocumentReference {
                                ID = new List<ID>{new ID {
                                    _ =  guia.DatosEnvio.UnidadTransporte.Placa,
                                    schemeID = "06"
                                }}
                            }}
                            //ApplicableTransportMeans = new List<ApplicableTransportMean>{ new ApplicableTransportMean {//Tarjeta  IS ES PRIVADO NO VA??
                            //    RegistrationNationalityID = new List<RegistrationNationalityID>{ new RegistrationNationalityID { _ = guia.datosEnvio.unidadTransporte.Tarjeta }}
                            //}}
                        }}
                    }};
            }

            if (guia.TipoTransporte == "PUBLICO")
            {
                root.DespatchAdvice[0].Shipment[0].ShipmentStage[0].CarrierParty = new List<CarrierParty>{ new CarrierParty {
                        PartyIdentification = new List<PartyIdentification>{ new PartyIdentification { ID = new List<ID> { new ID { _ = guia.DatosEnvio.RucEmpresaCurrier, schemeID=EspacioNombreGuiaRemision._DocumentType06 }}}},
                        PartyLegalEntity = new List<PartyLegalEntity>{ new PartyLegalEntity {
                            RegistrationName = new List<RegistrationName>{ new RegistrationName { _ = guia.DatosEnvio.RazonSocialCurrier }},
                            //Condicional: Corresponde al número de registro del MTC del transportista.
                            CompanyID = new List<CompanyID>{ new CompanyID { _ = guia.DatosEnvio.RegistroMtcCurrier ?? "NUMREGISTRANS" }}
                        }},
                        //AgentParty = new List<AgentParty>{ new AgentParty { 
                        //    PartyLegalEntity = new List<PartyLegalEntity> { new PartyLegalEntity { 
                        //        //Condicional: Corresponde al Número de la autorización especial emitido por la entidad emisora autorizada.
                        //        CompanyID = new List<CompanyID> { new CompanyID { _ = "NUMAUTRANS" ,schemeID = "06" }}
                        //    }}
                        //}},
                   }};
            }

            for (var i = 0; i < guia.Productos.Count; i++)
            {
                var item = guia.Productos[i];
                root.DespatchAdvice[0].DespatchLine.Add(new DespatchLine
                {
                    ID = new List<ID_Int> { new ID_Int { _ = i + 1 } },
                    Note = new List<Note> { new Note { _ = string.IsNullOrWhiteSpace(item.DescripcionUM) ? "UNIDAD" : item.DescripcionUM } },
                    DeliveredQuantity = new List<DeliveredQuantity> 
                    { 
                        new DeliveredQuantity { _ = item.cantidad, unitCode = string.IsNullOrWhiteSpace(item.CodigoUM) ? "ZZ" : item.CodigoUM}
                    },
                    OrderLineReference = new List<OrderLineReference> { new OrderLineReference { LineID = new List<LineID> { new LineID { _ = i + 1 } } } },
                    Item = new List<Item> { new Item {
                        Description = new List<Description>{ new Description { _ = item.descripcion }},
                        SellersItemIdentification = new List<SellersItemIdentification>{new SellersItemIdentification {
                            ID = new List<ID>{new ID { _ = item.codigo }}
                        }}
                    }}
                });
            }

            if (!string.IsNullOrEmpty(guia.Observacion))
                root.DespatchAdvice[0].Note = new List<Note> { new Note { _ = guia.Observacion } };



            string guiaJson = JsonConvert.SerializeObject(
            root,
            Formatting.None,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
        );

            //Se está guardando en la carpeta ahora mismo.
            var nombreArchivo = getNombreDocumentoFile(guia, guia.Remitente.Ruc);
            Helper.GuardarJSON(guiaJson, RutaDocPendiente(guia, guia.Remitente.Ruc));

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
