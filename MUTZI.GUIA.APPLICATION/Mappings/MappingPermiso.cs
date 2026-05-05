using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.Permiso;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingPermiso : AutoMapper.Profile
    {
        public MappingPermiso()
        {
            CreateMap<Permiso, PermisoDTO>();
            CreateMap<Permiso, PermisoConPerfilesDTO>()
                .ForMember(dest => dest.Perfiles, opt => opt.MapFrom(src => src.Perfiles));
        }
    }
}
