using Newtonsoft.Json;

namespace MUTZI.GUIA.APPLICATION.DTOs.Efact
{
    public class AdditionalDocumentReference
    {
        public List<ID> ID { get; set; }
        public List<DocumentTypeCode> DocumentTypeCode { get; set; }
        public List<DocumentType> DocumentType { get; set; }
        public List<IssuerParty> IssuerParty { get; set; }
    }

    public class AddressLine
    {
        public List<Line> Line { get; set; }
    }

    public class AgentParty
    {
        public List<PartyLegalEntity> PartyLegalEntity { get; set; }
    }

    public class CarrierParty
    {
        public List<PartyIdentification> PartyIdentification { get; set; }
        public List<PartyLegalEntity> PartyLegalEntity { get; set; }
        public List<AgentParty> AgentParty { get; set; }
    }

    public class CityName
    {
        public string _ { get; set; }
    }

    public class CitySubdivisionName
    {
        public string _ { get; set; }
    }

    public class CompanyID
    {
        public string _ { get; set; }
        public string schemeID { get; set; }
    }

    public class Contact
    {
        public List<ElectronicMail> ElectronicMail { get; set; }
    }

    public class Country
    {
        public List<IdentificationCode> IdentificationCode { get; set; }
    }

    public class CountrySubentity
    {
        public string _ { get; set; }
    }

    public class CustomizationID
    {
        public string _ { get; set; }
    }

    public class DeliveredQuantity
    {
        public string _ { get; set; }
        public string unitCode { get; set; }
    }

    public class Delivery
    {
        public List<DeliveryAddress> DeliveryAddress { get; set; }
        public List<Despatch> Despatch { get; set; }
    }

    public class AddressTypeCode
    {
        public string _ { get; set; }
        public string listID { get; set; }
    }

    public class DeliveryAddress
    {
        public List<ID> ID { get; set; }

        public List<AddressTypeCode> AddressTypeCode { get; set; }

        public List<CitySubdivisionName> CitySubdivisionName { get; set; }
        public List<CityName> CityName { get; set; }
        public List<CountrySubentity> CountrySubentity { get; set; }
        public List<District> District { get; set; }
        public List<AddressLine> AddressLine { get; set; }
        public List<Country> Country { get; set; }
    }
    //Para cliente
    public class DeliveryCustomerParty
    {
        public List<Party> Party { get; set; }
    }
    //Para proveedor
    public class SellerSupplierParty
    {
        public List<Party> Party { get; set; }
    }

    public class Description
    {
        public string _ { get; set; }
    }



    public class Despatch
    {
        public List<DespatchAddress> DespatchAddress { get; set; }
        public List<DespatchParty> DespatchParty { get; set; }
    }

    public class DespatchAddress
    {
        public List<ID> ID { get; set; }
        public List<AddressTypeCode> AddressTypeCode { get; set; }
        public List<CitySubdivisionName> CitySubdivisionName { get; set; }
        public List<CityName> CityName { get; set; }
        public List<CountrySubentity> CountrySubentity { get; set; }
        public List<District> District { get; set; }
        public List<AddressLine> AddressLine { get; set; }
        public List<Country> Country { get; set; }
    }

    public class DespatchAdvice
    {
        public List<UBLVersionID> UBLVersionID { get; set; }
        public List<CustomizationID> CustomizationID { get; set; }
        public List<ID> ID { get; set; }
        public List<IssueDate> IssueDate { get; set; }
        public List<IssueTime> IssueTime { get; set; }
        public List<DespatchAdviceTypeCode> DespatchAdviceTypeCode { get; set; }
        public List<Note>? Note { get; set; }
        public List<LineCountNumeric> LineCountNumeric { get; set; }
        public List<AdditionalDocumentReference> AdditionalDocumentReference { get; set; }
        public List<Signature> Signature { get; set; }
        public List<DespatchSupplierParty> DespatchSupplierParty { get; set; }
        public List<DeliveryCustomerParty> DeliveryCustomerParty { get; set; }
        public List<SellerSupplierParty> SellerSupplierParty { get; set; }
        public List<Shipment> Shipment { get; set; }
        public List<DespatchLine> DespatchLine { get; set; }
    }

    public class DespatchAdviceTypeCode
    {
        public string _ { get; set; }
    }

    public class DespatchLine
    {
        public List<ID_Int> ID { get; set; }
        public List<Note> Note { get; set; }
        public List<DeliveredQuantity> DeliveredQuantity { get; set; }
        public List<OrderLineReference> OrderLineReference { get; set; }
        public List<Item> Item { get; set; }
    }

    public class DespatchParty
    {
        public List<AgentParty> AgentParty { get; set; }
        public List<PartyIdentification> PartyIdentification { get; set; } //new
        public List<PostalAddress> PostalAddress { get; set; } //New
        public List <PartyLegalEntity> PartyLegalEntity { get; set; } //new
        public List<Contact> Contact { get; set; }
        //public List<Party> Party { get; set; } //new
    }

    public class DespatchSupplierParty
    {
        public List<Party> Party { get; set; }
    }

    public class DigitalSignatureAttachment
    {
        public List<ExternalReference> ExternalReference { get; set; }
    }

    public class District
    {
        public string _ { get; set; }
    }

    public class DocumentType
    {
        public string _ { get; set; }
    }

    public class DocumentTypeCode
    {
        public string _ { get; set; }
    }

    public class ElectronicMail
    {
        public string _ { get; set; }
    }

    public class ExternalReference
    {
        public List<URI> URI { get; set; }
    }

    public class GrossWeightMeasure
    {
        public string _ { get; set; }
        public string unitCode { get; set; }
    }

    public class HandlingCode
    {
        public string _ { get; set; }
    }

    public class HandlingInstruction
    {
        public string _ { get; set; }
    }

    public class ID
    {
        public string _ { get; set; }
        public string? schemeID { get; set; }
    }

    [JsonObject("ID")]
    public class ID_Int
    {
        public int _ { get; set; }
    }

    public class IdentificationCode
    {
        public string _ { get; set; }
    }

    public class IssueDate
    {
        public string _ { get; set; }
    }

    public class IssuerParty
    {
        public List<PartyIdentification> PartyIdentification { get; set; }
    }

    public class IssueTime
    {
        public string _ { get; set; }
    }

    public class Item
    {
        public List<Description> Description { get; set; }
        public List<SellersItemIdentification> SellersItemIdentification { get; set; }
    }

    public class Line
    {
        public string _ { get; set; }
    }

    public class LineCountNumeric
    {
        public string _ { get; set; }
    }

    public class LineID
    {
        public int _ { get; set; }
    }

    public class Name
    {
        public string _ { get; set; }
    }

    public class Note
    {
        public string _ { get; set; }
    }

    public class OrderLineReference
    {
        public List<LineID> LineID { get; set; }
    }

    public class Party
    {
        public List<PartyIdentification> PartyIdentification { get; set; }
        public List<PostalAddress> PostalAddress { get; set; }
        public List<PartyLegalEntity> PartyLegalEntity { get; set; }
        public List<Contact> Contact { get; set; }
    }

    public class PartyIdentification
    {
        public List<ID> ID { get; set; }
    }

    public class PartyLegalEntity
    {
        public List<RegistrationName> RegistrationName { get; set; }
        public List<CompanyID> CompanyID { get; set; }
    }


    public class PartyName
    {
        public List<Name> Name { get; set; }
    }

    public class PostalAddress
    {
        public List<ID> ID { get; set; }
        public List<StreetName> StreetName { get; set; }
        public List<CitySubdivisionName> CitySubdivisionName { get; set; }
        public List<CityName> CityName { get; set; }
        public List<CountrySubentity> CountrySubentity { get; set; }
        public List<District> District { get; set; }
        public List<Country> Country { get; set; }
    }

    public class RegistrationName
    {
        public string _ { get; set; }
    }

    public class GuiaRemisionRemitente
    {
        public string _D { get; set; }
        public string _A { get; set; }
        public string _B { get; set; }
        public string _E { get; set; }
        public List<DespatchAdvice> DespatchAdvice { get; set; }
    }

    public class SellersItemIdentification
    {
        public List<ID> ID { get; set; }
    }

    public class Shipment
    {
        public List<ID> ID { get; set; }
        public List<HandlingCode> HandlingCode { get; set; }
        public List<HandlingInstruction> HandlingInstructions { get; set; }
        public List<GrossWeightMeasure> GrossWeightMeasure { get; set; }
        public List<ShipmentStage> ShipmentStage { get; set; }
        public List<Delivery> Delivery { get; set; }
        public List<TransportHandlingUnit>? TransportHandlingUnit { get; set; }
    }

    public class ShipmentStage
    {
        public List<TransportModeCode> TransportModeCode { get; set; }
        public List<TransitPeriod> TransitPeriod { get; set; }
        public List<CarrierParty> CarrierParty { get; set; }
        public List<DriverPerson>? DriverPerson { get; set; }
    }

    public class SignatoryParty
    {
        public List<PartyIdentification> PartyIdentification { get; set; }
        public List<PartyName> PartyName { get; set; }
    }

    public class Signature
    {
        public List<ID> ID { get; set; }
        public List<SignatoryParty> SignatoryParty { get; set; }
        public List<DigitalSignatureAttachment> DigitalSignatureAttachment { get; set; }
    }

    public class DriverPerson
    {
        public List<ID> ID { get; set; }
        public List<FirstName> FirstName { get; set; }
        public List<FamilyName> FamilyName { get; set; }
        public List<JobTitle> JobTitle { get; set; }
        public List<IdentityDocumentReference> IdentityDocumentReference { get; set; }
    }

    public class FamilyName
    {
        public string _ { get; set; }
    }

    public class FirstName
    {
        public string _ { get; set; }
    }


    public class IdentityDocumentReference
    {
        public List<ID> ID { get; set; }
    }

    public class JobTitle
    {
        public string _ { get; set; }
    }

    public class StartDate
    {
        public string _ { get; set; }
    }

    public class StreetName
    {
        public string _ { get; set; }
    }

    public class TransitPeriod
    {
        public List<StartDate> StartDate { get; set; }
    }

    public class TransportModeCode
    {
        public string _ { get; set; }
    }

    public class UBLVersionID
    {
        public string _ { get; set; }
    }

    public class URI
    {
        public string _ { get; set; }
    }

    public class RegistrationNationalityID
    {
        public string _ { get; set; }
    }



    public class ApplicableTransportMean
    {
        public List<RegistrationNationalityID> RegistrationNationalityID { get; set; }
    }

    public class TransportEquipment
    {
        public List<ID> ID { get; set; }
        public List<ShipmentDocumentReference> ShipmentDocumentReference { get; set; }
        public List<ApplicableTransportMean> ApplicableTransportMeans { get; set; }

    }

    public class TransportHandlingUnit
    {
        public List<TransportEquipment> TransportEquipment { get; set; }

    }

    public class ShipmentDocumentReference
    {
        public List<ID> ID { get; set; }
    }
}
