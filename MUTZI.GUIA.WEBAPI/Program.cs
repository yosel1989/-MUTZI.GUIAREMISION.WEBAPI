using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Mappings;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Transporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.VentaConfig;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Documento;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Transporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.VentaConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.Services.Documento;
using MUTZI.GUIA.APPLICATION.Services.Envio;
using MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Transporte;
using MUTZI.GUIA.APPLICATION.Services.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Services.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Services.VentaConfig;
using MUTZI.GUIA.INFRASTRUCTURE.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new Conversion.PeruTimeZoneConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var allErrors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            var errorResponse = new BadRequestResponseDTO
            {
                Error = "Error de validación.",
                Detalle = string.Join(" | ", allErrors)
            };

            return new BadRequestObjectResult(errorResponse);
        };
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

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
