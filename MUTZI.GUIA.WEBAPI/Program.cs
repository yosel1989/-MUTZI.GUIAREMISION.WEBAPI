using FluentValidation;
using Genexs.AWS.Library;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MUTZI.GUIA.APPLICATION.Configuration;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.Hubs;
using MUTZI.GUIA.APPLICATION.Interfaces.Ports;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.Ubigeo;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Documento;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Ubigeo;
using MUTZI.GUIA.APPLICATION.Mappings;
using MUTZI.GUIA.APPLICATION.Services;
using MUTZI.GUIA.APPLICATION.Services.Catalogos;
using MUTZI.GUIA.APPLICATION.Services.Documento;
using MUTZI.GUIA.APPLICATION.Services.Envio;
using MUTZI.GUIA.APPLICATION.Services.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Services.Ubigeo;
using MUTZI.GUIA.APPLICATION.Validators;
using MUTZI.GUIA.DOMAIN.Interfaces;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE;
using MUTZI.GUIA.INFRASTRUCTURE.Adapters;
using MUTZI.GUIA.INFRASTRUCTURE.Interceptors;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.INFRASTRUCTURE.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo;
using MUTZI.GUIA.WEBAPI.Filters;
using MUTZI.GUIA.WEBAPI.Middlewares;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//AWS S3 Configuration
builder.Services.AddSingleton<IS3Configuration, S3Configuration>();


builder.Services.AddScoped<PermissionFilter>();
builder.Services.AddControllers(
    options =>
    {
        options.Filters.Add<PermissionFilter>();
        options.Filters.Add(new ProducesAttribute("application/json"));
    }
)
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
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar el DbContext con la cadena de conexión desde appsettings.json
// Registrar el interceptor
builder.Services.AddScoped<GuiaRemisionInterceptor>();


var connectionString = builder.Configuration.GetConnectionString("cs_logistica");
builder.Services.AddDbContext<LogisticaContext>((sp,options) =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
    options.AddInterceptors(sp.GetRequiredService<GuiaRemisionInterceptor>());
});


//Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(MappingEntityBranch).Assembly);
builder.Services.AddAutoMapper(typeof(MappingPerfil).Assembly);
builder.Services.AddAutoMapper(typeof(MappingPerfilPermiso).Assembly);
builder.Services.AddAutoMapper(typeof(MappingPermiso).Assembly);
builder.Services.AddAutoMapper(typeof(MappingConductor).Assembly);
builder.Services.AddAutoMapper(typeof(MappingUnidadTransporte).Assembly);
builder.Services.AddAutoMapper(typeof(MappingTransportista).Assembly);
builder.Services.AddAutoMapper(typeof(MappingGuiaRemisionHistorial).Assembly);
builder.Services.AddAutoMapper(typeof(MappingGuiaRemisionEstado).Assembly);
builder.Services.AddAutoMapper(typeof(MappingArea).Assembly);
builder.Services.AddAutoMapper(typeof(MappingMotivoTraslado).Assembly);
builder.Services.AddAutoMapper(typeof(MappingCatalogo).Assembly);
builder.Services.AddAutoMapper(typeof(MappingPersona).Assembly);
builder.Services.AddAutoMapper(typeof(MappingOperacionResponsable).Assembly);
builder.Services.AddAutoMapper(typeof(MappingEntity).Assembly);
builder.Services.AddAutoMapper(typeof(MappingIntegrationCredential).Assembly);

builder.Services.AddSignalR();

//Registrar repositorios
builder.Services.AddScoped<IVentaConfigRepository, VentaConfigRepository>();
builder.Services.AddScoped<IGuiaRemitenteRepository, GuiaRemitenteRepository>();
builder.Services.AddScoped<IGuiaTransportistaRepository, GuiaTransportistaRepository>();

builder.Services.AddScoped<IConductorRepository, ConductorRepository>();
builder.Services.AddScoped<IUnidadTransporteRepository, UnidadTransporteRepository>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IUbigeoRepository, UbigeoRepository>();
builder.Services.AddScoped<ITransportistaRepository, TransportistaRepository>();
builder.Services.AddScoped<IEstablecimientoRepository, EstablecimientoRepository>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();
builder.Services.AddScoped<IPermisoRepository, PermisoRepository>();
builder.Services.AddScoped<IPerfilPermisoRepository, PerfilPermisoRepository>();
builder.Services.AddScoped<IGuiaRemisionHistorialRepository, GuiaRemisionHistorialRepository>();
builder.Services.AddScoped<IGuiaRemisionEstadoRepository, GuiaRemisionEstadoRepository>();
builder.Services.AddScoped<IGuiaRemisionEstadoPermisoRepository, GuiaRemisionEstadoPermisoRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IMotivoTrasladoRepository, MotivoTrasladoRepository>();
builder.Services.AddScoped<IUnidadMedidaRepository, UnidadMedidaRepository>();
builder.Services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
builder.Services.AddScoped<IEntidadReguladoraRepository, EntidadReguladoraRepository>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IOperacionResponsableRepository, OperacionResponsableRepository>();
builder.Services.AddScoped<IBienNormalizadoRepository, BienNormalizadoRepository>();
builder.Services.AddScoped<IDocumentoRelacionadoRepository, DocumentoRelacionadoRepository>();
builder.Services.AddScoped<IGuiaRemisionIndicadorTrasladoRepository, GuiaRemisionIndicadorTrasladoRepository>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>();
builder.Services.AddScoped<IEntityRoleRepository, EntityRoleRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IIntegrationCredentialRepository, IntegrationCredentialRepository>();

//Registrar servicios
builder.Services.AddHttpClient();
builder.Services.AddScoped<IVentaConfigService, VentaConfigService>();
builder.Services.AddScoped<IGuiaRemitenteUBLService, GuiaRemitenteUBLService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();
builder.Services.AddScoped<IGuiaTransportistaUBLService, GuiaTransportistaUBLService>();
builder.Services.AddScoped<IGuiaRemitenteService, GuiaRemitenteService>();
builder.Services.AddScoped<IGuiaTransportistaService, GuiaTransportistaService>();
builder.Services.AddScoped<IEnvioService, EnvioService>();
builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IPermisoService, PermisoService>();

builder.Services.AddScoped<IEntityInfoPort, EntityInfoAdapter>();

builder.Services.AddScoped<IConductorService, ConductorService>();
builder.Services.AddScoped<IUnidadTransporteService, UnidadTransporteService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IUbigeoService, UbigeoService>();
builder.Services.AddScoped<IS3FileService, S3FileService>();
builder.Services.AddScoped<IEstablecimientoService, EstablecimientoService>();
builder.Services.AddScoped<ITransportistaService, TransportistaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGuiaRemisionHistorialService, GuiaRemisionHistorialService>();
builder.Services.AddScoped<IGuiaRemisionEstadoService, GuiaRemisionEstadoService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IMotivoTrasladoService, MotivoTrasladoService>();
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<IOperacionResponsableService, OperacionResponsableService>();
builder.Services.AddScoped<IGuiaRemisionIndicadorTrasladoService, GuiaRemisionIndicadorTrasladoService>();
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<IEntityInfoService, EntityInfoService>();
builder.Services.AddScoped<IEntityRoleService, EntityRoleService>();
builder.Services.AddScoped<IIntegrationCredentialService, IntegrationCredentialService>();
builder.Services.AddMemoryCache();

builder.Services.AddValidatorsFromAssemblyContaining<EntityCreateValidator>();


builder.Services.AddScoped<IAppDBContext, AppDBContext>();

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
    c.SupportNonNullableReferenceTypes();
    //c.OperationFilter<FileUploadOperationFilter>();
    c.EnableAnnotations();
});


builder.Services.AddHttpContextAccessor();
var adminJwt = builder.Configuration.GetSection("JwtSettings:Admin").Get<JwtSettingsRol>();
builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(adminJwt.Key)),
        ValidIssuer = adminJwt.Issuer,
        ValidAudience = adminJwt.Audience,
        ValidateIssuer = true,
        ValidateAudience = true
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwt"];
            return Task.CompletedTask;
        }
    };
});

// Cors
var env = builder.Environment;

var corsPolicy = "AllowOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://localhost:4201",
            "http://localhost:5000",
            "https://*.cloudfront.net",
            "https://*.mutzi.pe"
        )
              .SetIsOriginAllowedToAllowWildcardSubdomains()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Serilog configuración
var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System.Net.Http", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .Enrich.FromLogContext();

Log.Logger = loggerConfig.CreateLogger();

var app = builder.Build();

app.UseStaticFiles();

app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(corsPolicy);

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<FluentValidationMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<GuiasHub>("/guias-signal");
});

app.Run();
