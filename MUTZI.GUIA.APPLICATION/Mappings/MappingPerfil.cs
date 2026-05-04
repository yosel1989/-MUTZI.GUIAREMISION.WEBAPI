using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.Perfil;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingPerfil : AutoMapper.Profile
    {
        public MappingPerfil()
        {
            CreateMap<Perfil, PerfilDTO>();
            CreateMap<Perfil, PerfilListDTO>();
            CreateMap<PerfilCreateDTO, Perfil>();
            CreateMap<PerfilUpdateDTO, Perfil>();
        }
    }
}
