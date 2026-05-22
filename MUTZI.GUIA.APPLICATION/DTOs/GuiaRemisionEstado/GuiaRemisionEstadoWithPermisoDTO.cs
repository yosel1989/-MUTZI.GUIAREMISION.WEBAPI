using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionEstado
{
    public class GuiaRemisionEstadoWithPermisoDTO
    {
        [JsonPropertyName("id")]
        public int GuiaRemisionEstadoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("Descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("permisos")]
        public List<int> Permisos { get; set; } = new List<int>();
    }
}
