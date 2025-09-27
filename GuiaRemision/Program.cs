using GuiaRemision.Mappings;
using GuiaRemision.Models.DBMS;
using GuiaRemision.Repositories.GuiaRemitente;
using GuiaRemision.Repositories.GuiaRemitente.Conductor;
using GuiaRemision.Repositories.GuiaRemitente.Destinatario;
using GuiaRemision.Repositories.GuiaRemitente.Proveedor;
using GuiaRemision.Repositories.GuiaRemitente.Remitente;
using GuiaRemision.Repositories.GuiaRemitente.Transporte;
using GuiaRemision.Repositories.GuiaTransportista;
using GuiaRemision.Repositories.VentaConfig;
using GuiaRemision.Services.DocumentEfact;
using GuiaRemision.Services.Envio;
using GuiaRemision.Services.GuiaRemision;
using GuiaRemision.Services.GuiaRemitente.Conductor;
using GuiaRemision.Services.GuiaRemitente.Destinatario;
using GuiaRemision.Services.GuiaRemitente.Proveedor;
using GuiaRemision.Services.GuiaRemitente.Remitente;
using GuiaRemision.Services.GuiaRemitente.Transporte;
using GuiaRemision.Services.GuiaTransportista;
using GuiaRemision.Services.VentaConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using static GuiaRemision.Constantes.Conversion;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new PeruTimeZoneConverter());
    })
    ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar el DbContext con la cadena de conexión desde appsettings.json
builder.Services.AddDbContext<LogisticaContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("cs_logistica"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("cs_logistica"))
    )
);
//Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

//Registrar repositorios
builder.Services.AddScoped<IVentaConfigRepository, VentaConfigRepository>();
builder.Services.AddScoped<IGuiaRemitenteRepository, GuiaRemitenteRepository>();
builder.Services.AddScoped<IGuiaTransportistaRepository, GuiaTransportistaRepository>();

builder.Services.AddScoped<IRemitenteRepository, RemitenteRepository>();
builder.Services.AddScoped<IDestinatarioRepository, DestinatarioRepository>();
builder.Services.AddScoped<IConductorRepository, ConductorRepository>();
builder.Services.AddScoped<ITransporteRepository, TransporteRepository>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();

//Registrar servicios
builder.Services.AddHttpClient();
builder.Services.AddScoped<IVentaConfigService, VentaConfigService>();
builder.Services.AddScoped<IGuiaRemitenteUBLService, GuiaRemitenteUBLService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();
builder.Services.AddScoped<IGuiaTransportistaUBLService, GuiaTransportistaUBLService>();
builder.Services.AddScoped<IGuiaRemitenteService, GuiaRemitenteService>();
builder.Services.AddScoped<IGuiaTransportistaService, GuiaTransportistaService>();
builder.Services.AddScoped<IEnvioService, EnvioService>();

builder.Services.AddScoped<IRemitenteService, RemitenteService>();
builder.Services.AddScoped<IDestinatarioService, DestinatarioService>();
builder.Services.AddScoped<IConductorService, ConductorService>();
builder.Services.AddScoped<ITransporteService, TransporteService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

//aGREGAR SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API DE GUÍA DE REMISIÓN",
        Version = "v1",
        Description = "Documentación de la API en .NET 8 con Swagger"
        //Contact = new OpenApiContact
        //{
        //    Name = "Helio Pozo",
        //    Email = "heliohubert10@gmail.com"
        //}
    });
    c.EnableAnnotations();
});

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
