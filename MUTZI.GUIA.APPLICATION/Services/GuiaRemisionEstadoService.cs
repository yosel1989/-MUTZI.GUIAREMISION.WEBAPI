using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionEstado;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class GuiaRemisionEstadoService: IGuiaRemisionEstadoService
    {
        private readonly IGuiaRemisionEstadoRepository _guiaRemisionEstadoRepository;
        private readonly IGuiaRemisionEstadoPermisoRepository _guiaRemisionEstadoPermisoRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public GuiaRemisionEstadoService(
            IGuiaRemisionEstadoRepository guiaRemisionEstadoRepository,
            IGuiaRemisionEstadoPermisoRepository guiaRemisionEstadoPermisoRepository,
            IMapper mapper,
            IAuthService authService
        )
        {
            _guiaRemisionEstadoRepository = guiaRemisionEstadoRepository;
            _guiaRemisionEstadoPermisoRepository = guiaRemisionEstadoPermisoRepository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<GuiaRemisionEstadoDTO>> GetAllAsync()
        {
            try
            {
                var estados = await _guiaRemisionEstadoRepository.GetAllAsync();
                return _mapper.Map<List<GuiaRemisionEstadoDTO>>(estados);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GuiaRemisionEstadoWithPermisoDTO>> GetAllWithPermisosAsync()
        {
            try
            {
                var estadosConPermisos = await _guiaRemisionEstadoRepository.GetAllWithPermisosAsync();
                return _mapper.Map<List<GuiaRemisionEstadoWithPermisoDTO>>(estadosConPermisos);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AsignarPermisosAsync(List<GuiaRemisionEstadoAsignarPermisosDTO> permisos)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var entidades = permisos.SelectMany(dto =>
                {
                    dto.Permisos = dto.Permisos.Distinct().ToList();

                    dto.Permisos.ForEach(perfilId =>
                    {
                        if (perfilId <= 0)
                            throw new KeyNotFoundException($"El ID de perfil '{perfilId}' no es válido. Debe ser un número positivo.");
                    });

                    return dto.Permisos.Select(perfilId => new GuiaRemisionEstadoPermiso
                    {
                        GuiaRemisionEstadoId = dto.GuiaRemisionEstadoId,
                        GuiaRemisionEstadoIdPermitido = perfilId,
                        UsuarioRegistro = currentUser.UserName,
                        UsuarioRegistroNombre = $"{currentUser.FirstName} {currentUser.LastName}",
                        FechaRegistro = DateTime.UtcNow
                    });
                }).ToList();

                await _guiaRemisionEstadoPermisoRepository.SyncAsync(entidades);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }
    }
}
