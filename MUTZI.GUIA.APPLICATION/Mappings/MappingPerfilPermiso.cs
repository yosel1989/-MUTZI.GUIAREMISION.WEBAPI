using MUTZI.GUIA.APPLICATION.DTOs.Perfil;
using MUTZI.GUIA.APPLICATION.DTOs.Permiso;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingPerfilPermiso : AutoMapper.Profile
    {
        public MappingPerfilPermiso()
        {
            CreateMap<Perfil, PerfilDTO>();
        }
    }
}
