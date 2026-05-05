
namespace MUTZI.GUIA.APPLICATION.Configuration
{
    public class JwtSettings
    {
        public JwtSettingsRol Admin { get; set; }
        public JwtSettingsRol Customer { get; set; }
    }

    public class JwtSettingsRol
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
    }
}
