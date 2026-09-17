using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.EntityBranchSerie
{
    public class EntityBranchSerieUpdateDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Recibe un número, aquí se coloca el identificador del registro a editar", Nullable = false)]
        public int EntityBranchSerieId { get; set; }

        [JsonPropertyName("entity_branch_id")]
        [SwaggerSchema("Recibe un número, aquí se coloca el identificador del establecimiento", Nullable = false)]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("invoice_type_id")]
        [SwaggerSchema("Recibe un número, aquí se coloca el identificador del documento comercial", Nullable = false)]
        public int InvoiceTypeId { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la seríe a registrar para el establecimiento relacionado al documento comercial.", Nullable = false)]
        public string Serie { get; set; }

        [JsonPropertyName("area")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el area al cual pertenecera la seríe o su uso.", Nullable = false)]
        public string Area { get; set; }

    }
}
