using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Area
{
    public class AreaDTO
    {
        [JsonPropertyName("id")]
        [Description("Identificador único del área")]
        public int AreaId { get; set; }

        [JsonPropertyName("code")]
        [Required]
        [Description("Código del área: 2 dígitos si es principal (10), 4 dígitos si es subárea (1001)")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("name")]
        [Required]
        [Description("Nombre completo del área. En subáreas incluye al padre: 'Producción – Envasado'")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("parent_area_id")]
        [Description("Id del área padre. NULL si es un área principal")]
        public int? ParentAreaId { get; set; }

        [JsonPropertyName("level")]
        [Description("Nivel jerárquico: 1 = área principal, 2 = subárea")]
        public byte Level { get; set; }

        [JsonPropertyName("active")]
        [Description("Estado del área: activa o inactiva")]
        public bool Active { get; set; } = true;

        [JsonPropertyName("created_at")]
        [Description("Fecha de creación del registro")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        [Description("Usuario que creó el registro")]
        public string CreatedAtUser { get; set; } = null!;

        [JsonPropertyName("created_at_user_name")]
        [Description("Nombre del usuario que creó el registro")]
        public string CreatedAtUserName { get; set; } = null!;

        [JsonPropertyName("created_at_employee_id")]
        [Description("Id del empleado que creó el registro, si el usuario está vinculado a un empleado")]
        public int? CreatedAtEmployeeId { get; set; }

        [JsonPropertyName("updated_at")]
        [Description("Fecha de última modificación")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("updated_at_user")]
        [Description("Usuario que modificó el registro")]
        public string? UpdatedAtUser { get; set; }

        [JsonPropertyName("updated_at_user_name")]
        [Description("Nombre del usuario que modificó el registro")]
        public string? UpdatedAtUserName { get; set; }

        [JsonPropertyName("updated_at_employee_id")]
        [Description("Id del empleado que modificó el registro, si el usuario está vinculado a un empleado")]
        public int? UpdatedAtEmployeeId { get; set; }

    }
}
