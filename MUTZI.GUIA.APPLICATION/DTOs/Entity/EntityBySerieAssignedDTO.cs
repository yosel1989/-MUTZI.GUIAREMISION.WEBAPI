using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Entity
{
    public class EntityBySerieAssignedDTO
    {
        [JsonPropertyName("entity")]
        [Description("Datos de la entidad")]
        public EntityBySerieAssigned_EntityDTO Entity { get; set; }

        [JsonPropertyName("entity_branch")]
        [Description("Datos del establecimiento")]
        public EntityBySerieAssigned_EntityBranchDTO EntityBranch { get; set; }

        [JsonPropertyName("entity_branch_serie")]
        [Description("Datos de la serie")]
        public EntityBySerieAssigned_EntityBranchSerieDTO EntityBranchSerie { get; set; }
    }


    public class EntityBySerieAssigned_EntityDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único de la entidad")]
        public int EntityId { get; set; }

        [JsonPropertyName("name")]
        [Description("Razón social si es empresa")]
        public string? Name { get; set; }

        [JsonPropertyName("document_number")]
        [Description("Número de documento de identidad o RUC")]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [Description("Código de ubicación geográfica (Ubigeo)")]
        public string? UbigeoId { get; set; }

        [JsonPropertyName("address")]
        [Description("Dirección física de la entidad")]
        public string? Address { get; set; }

        [JsonPropertyName("department")]
        [Description("Departamento donde esta ubicado la entidad.")]
        public string Department { get; set; }

        [JsonPropertyName("province")]
        [Description("Provincia donde esta ubicado la entidad.")]
        public string Province { get; set; }

        [JsonPropertyName("district")]
        [Description("Distrito donde esta ubicado la entidad.")]
        public string District { get; set; }

        /*[JsonPropertyName("country_id")]
        [Description("Identificador del país")]
        public int? CountryId { get; set; }*/

        [JsonPropertyName("mtc_code")]
        [Description("Número de registro MTC ")]
        public string? MtcCode { get; set; }
    }

    public class EntityBySerieAssigned_EntityBranchDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único del establecimiento.")]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("description")]
        [Description("Descripción o alias del establecimiento.")]
        public string Description { get; set; }

        [JsonPropertyName("address")]
        [Description("Dirección del establecimiento.")]
        public string Address { get; set; }

        [JsonPropertyName("ubigeo_id")]
        [Description("Código de ubicación geográfica (Ubigeo)")]
        public string? UbigeoId { get; set; }

        [JsonPropertyName("department")]
        [Description("Departamento donde esta ubicado el establecimiento.")]
        public string Department { get; set; }

        [JsonPropertyName("province")]
        [Description("Provincia donde esta ubicado el establecimiento.")]
        public string Province { get; set; }

        [JsonPropertyName("district")]
        [Description("Distrito donde esta ubicado el establecimiento.")]
        public string District { get; set; }

        [JsonPropertyName("code_sunat")]
        [Description("Código sunat del establecimiento.")]
        public string CodeSunat { get; set; }

    }

    public class EntityBySerieAssigned_EntityBranchSerieDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único de la serie")]
        public int EntityBranchSerieId { get; set; }

        [JsonPropertyName("serie")]
        [Description("Serie")]
        public string Serie { get; set; }

        [JsonPropertyName("area")]
        [Description("VWArea asignada a la serie")]
        public string Area { get; set; }
    }


}
