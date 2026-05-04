using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.User
{
    public class UserJwtDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string FirstName { get; set; }

        [JsonPropertyName("apellido")]
        public string LastName { get; set; }

        [JsonPropertyName("usuario")]
        public string UserName { get; set; }

        [JsonPropertyName("id_empleado")]
        public int EmployeeId { get; set; }

        [JsonPropertyName("id_tipo_local")]
        public int LocalTypeId { get; set; }

        [JsonPropertyName("id_local")]
        public int LocalId { get; set; }

        [JsonPropertyName("id_area")]
        public int AreaId { get; set; }

        [JsonPropertyName("id_usuario_local")]
        public int UserLocalId { get; set; }

        [JsonPropertyName("desc_usuario_local")]
        public string? UserLocalDesc { get; set; }

        [JsonPropertyName("id_usuario_local2")]
        public int UserLocalId2 { get; set; }

        [JsonPropertyName("id_usuario_local3")]
        public int UserLocalId3 { get; set; }

        [JsonPropertyName("desc_usuario_local3")]
        public string? UserLocalDesc3 { get; set; }

        [JsonPropertyName("id_usuario_local_comercial")]
        public int UserLocalComercialId { get; set; }

        [JsonPropertyName("id_usuario_sn")]
        public int IdUsuarioSN { get; set; }

        [JsonPropertyName("perfiles")]
        public List<PerfilJWTDto> Profiles { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
    }

    public class PerfilJWTDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("appId")]
        public int IdApp { get; set; }
    }
}
