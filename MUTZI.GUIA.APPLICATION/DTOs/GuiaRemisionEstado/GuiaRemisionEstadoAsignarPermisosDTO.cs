using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionEstado
{
    public class GuiaRemisionEstadoAsignarPermisosDTO
    {
        [JsonPropertyName("id")]
        public int GuiaRemisionEstadoId { get; set; }

        [JsonPropertyName("permisos")]
        public List<int> Permisos { get; set; }
    }
}
