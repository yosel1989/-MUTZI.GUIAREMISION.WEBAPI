using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.Permiso;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class PermisoService: IPermisoService
    {
        private readonly IAuthService _authService;
        private readonly IPermisoRepository _repository;
        private readonly IPerfilPermisoRepository _perfilPermisorepository;
        private readonly IMapper _mapper;
        public PermisoService(
            IPermisoRepository repository,
            IPerfilPermisoRepository perfilPermisorepository,
            IMapper mapper,
            IAuthService authService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
            _perfilPermisorepository = perfilPermisorepository;
        }

        public async Task<List<PermisoConPerfilesDTO>> ListAsync()
        {
            try
            {
                var data = await _repository.GetListAsync();
                return _mapper.Map<List<PermisoConPerfilesDTO>>(data);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }


        public async Task AsignarPerfiles(List<PermisoAsignarPerfilesDTO> permisoPerfiles)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                var entidades = permisoPerfiles.SelectMany(dto =>
                {
                    dto.Perfiles = dto.Perfiles.Distinct().ToList();

                    dto.Perfiles.ForEach(perfilId =>
                    {
                        if (perfilId <= 0)
                            throw new KeyNotFoundException($"El ID de perfil '{perfilId}' no es válido. Debe ser un número positivo.");
                    });

                    return dto.Perfiles.Select(perfilId => new PerfilPermiso
                    {
                        PermisoId = dto.PermisoId,
                        PerfilId = perfilId,
                        UsuarioRegistro = currentUser.UserName,
                        FechaRegistro = DateTime.UtcNow
                    });
                }).ToList();

                await _perfilPermisorepository.SyncAsync(entidades);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(msg);
            }
        }

    }
}
